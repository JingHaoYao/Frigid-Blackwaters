using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy: MonoBehaviour
{
    public string nameID;
    public int health;
    public int maxHealth;
    public int dangerValue;
    public int killNumber;
    public bool stopAttacking = false;
    public int percentSpawnChance = 33;
    public int armorMitigation;
    public float speed;
    public FogInfluenceStats fogStats;

    [System.Serializable]
    public struct FogInfluenceStats
    {
        //used to influence things on the fourth level, we don't need to modify this for any other enemies
        public float fogDurationIncrease;
        public float fogCoolDownDecrease;
    }

    private float currentStunDuration = 0;

    public List<EnemyStatusEffect> statuses;

    
    public void addKills()
    {
        GameObject.Find("PlayerShip").GetComponent<Artifacts>().numKills += killNumber;
        foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
        {
            if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                slot.displayInfo.GetComponent<ArtifactEffect>().addedKill(this.gameObject.tag, transform.position, this);
        }
    }

    public void heal(int healAmount)
    {
        if (health + healAmount > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += healAmount;
        }
    }

    public void addStatus(EnemyStatusEffect status, float duration = 0)
    {
        statuses.Add(status);
        status.duration = duration;
        status.targetEnemy = this;
        statusUpdated(status);
    }

    public bool containsStatus(string statusName)
    {
        foreach(EnemyStatusEffect statusEffect in statuses)
        {
            if(statusEffect != null && (statusEffect.name == statusName || statusEffect.name == statusName + "(Clone)"))
            {
                return true;
            }
        }
        return false;
    }

    public virtual void statusUpdated(EnemyStatusEffect newStatus)
    {

    }

    public virtual void statusRemoved(EnemyStatusEffect removedStatus)
    {

    }

    public void removeStatus(EnemyStatusEffect status)
    {
        this.statuses.Remove(status);
        statusRemoved(status);
    }

    public void dealDamage(int damageAmount)
    {
        if (health > 0)
        {
            int damageDealt = damageAmount - armorMitigation;
            if (damageDealt < 1)
            {
                damageDealt = 1;
            }
            EnemyPool.showDamageNumbers(damageDealt, this);
            health -= damageDealt;
            FindObjectOfType<CameraShake>().shakeCamFunction(0.1f, 0.3f * Mathf.Clamp(((float)damageDealt / maxHealth), 0.1f, 5f));

            damageProcedure(damageDealt);

            Artifacts artifacts = FindObjectOfType<Artifacts>();

            foreach (ArtifactSlot slot in artifacts.artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                {
                    slot.displayInfo.GetComponent<ArtifactEffect>().dealtDamage(damageDealt, this);
                }

            }
            if (health <= 0)
            {
                destroyProcedure();
            }
        }
    }

    public void destroyProcedure()
    {
        if (GetComponent<SpriteRenderer>())
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        EnemyPool.removeEnemy(this);
        addKills();
        deathProcedure();
        removeAllStatuses();
    }

    void removeAllStatuses()
    {
        foreach(EnemyStatusEffect statusEffect in statuses.ToArray())
        {
            if (statusEffect != null)
            {
                statusEffect.durationFinishedProcedure();
                statuses.Remove(statusEffect);
            }
        }
    }

    public void stunEnemy(float duration)
    {
        if(currentStunDuration < duration)
        {
            currentStunDuration = duration;
        }

        if(currentStunDuration <= 0)
        {
            StartCoroutine(stunEnemy());
        }
        
    }

    IEnumerator stunEnemy()
    {
        float stunTimer = 0;
        stopAttacking = true;
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

        while(currentStunDuration > 0)
        {
            currentStunDuration -= Time.deltaTime;
            yield return null;
        }

        currentStunDuration = 0;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        stopAttacking = false;
    }

    public abstract void deathProcedure();

    public abstract void damageProcedure(int damage);

    public void updateSpeed(float speedUpdate)
    {
        this.speed = Mathf.Clamp(speedUpdate, 0, int.MaxValue);
    }
}
