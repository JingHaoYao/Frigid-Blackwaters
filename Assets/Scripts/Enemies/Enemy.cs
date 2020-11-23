
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

    private bool isStunned = false;

    [SerializeField] public bool moveable = true;

    [System.Serializable]
    public struct FogInfluenceStats
    {
        //used to influence things on the fourth level, we don't need to modify this for any other enemies
        public float fogDurationIncrease;
        public float fogCoolDownDecrease;
    }

    private float currentStunDuration = 0;

    public List<EnemyStatusEffect> statuses;

    bool spawnArtifactKills = true;

    public bool EnemyStunned
    {
        get {
            return isStunned;
        }
    }

    public void SpawnArtifactKillsAndGoOnCooldown(float yOffset = 0)
    {
        if (spawnArtifactKills)
        {
            int numberSouls = Random.Range(1, 4);
            PlayerProperties.soulTrailSpawner.SpawnEnemyDeathSouls(numberSouls, transform.position + Vector3.up * yOffset);
            PlayerProperties.playerArtifacts.numKills += numberSouls;
            StartCoroutine(CooldownDuration(Random.Range(3.0f, 4.0f)));
        }
    }

    IEnumerator CooldownDuration(float duration)
    {
        spawnArtifactKills = false;

        yield return new WaitForSeconds(duration);

        spawnArtifactKills = true;
    }

    public void addKills()
    {
        PlayerProperties.playerArtifacts.numKills += killNumber;
        foreach (ArtifactSlot slot in PlayerProperties.playerArtifacts.artifactSlots)
        {
            if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                slot.displayInfo.GetComponent<ArtifactEffect>().addedKill(this.gameObject.tag, transform.position, this);
        }

        foreach(ShipWeaponScript script in PlayerProperties.playerScript.GetShipWeaponScripts())
        {
            script.shipWeaponTemplate.GetComponent<WeaponFireTemplate>().KilledEnemy(this);
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
    
    IEnumerator hitSleep(float damageProportion)
    {
        Time.timeScale = 0.001f;

        yield return new WaitForSecondsRealtime(0.05f + 0.03f * Mathf.Clamp(damageProportion, 0, 1) * Time.timeScale);

        Time.timeScale = 1;
    }

    public void dealDamage(int damageAmount, bool trueDeath = true)
    {
        // PlayerProperties.soulTrailSpawner.spawnRuneMarks(transform.position);
        // Commented out due to problems in build

        if (health > 0)
        {
            int damageDealt = damageAmount - armorMitigation;
            if (damageDealt < 1)
            {
                damageDealt = 1;
            }
            EnemyPool.showDamageNumbers(damageDealt, this);
            health -= damageDealt;
            PlayerProperties.cameraShake.shakeCamFunction(0.2f + Mathf.Clamp(((float)damageDealt / maxHealth), 0.1f, 0.3f) * 0.3f, 0.3f * Mathf.Clamp(((float)damageDealt / maxHealth), 0.1f, 1f));

            StartCoroutine(hitSleep((float)damageDealt / maxHealth));

            damageProcedure(damageDealt);

            Artifacts artifacts = PlayerProperties.playerArtifacts;

            foreach (ArtifactSlot slot in artifacts.artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                {
                    slot.displayInfo.GetComponent<ArtifactEffect>().dealtDamage(damageDealt, this);
                }

            }

            slightKnockBack();

            if (health <= 0)
            {
                destroyProcedure(trueDeath);
            }
        }
    }

    private void slightKnockBack()
    {
        if (moveable)
        {
            float angle = Mathf.Atan2(transform.position.y - PlayerProperties.playerShipPosition.y, transform.position.x - PlayerProperties.playerShipPosition.x);
            transform.position += new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 0.25f;
        }
    }

    public void destroyProcedure(bool trueDeath = true)
    {
        if (GetComponent<SpriteRenderer>())
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        removeAllStatuses();
        if (trueDeath)
        {
            PlayerProperties.soulTrailSpawner.SpawnEnemyDeathSouls(killNumber, transform.position);
            EnemyPool.removeEnemy(this);
            addKills();
            Time.timeScale = 1;
        }
        deathProcedure();
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

        if(!isStunned)
        {
            StartCoroutine(stunEnemy());
        }
    }

    IEnumerator stunEnemy()
    {
        isStunned = true;
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
        isStunned = false;
    }

    public abstract void deathProcedure();

    public abstract void damageProcedure(int damage);

    public void updateSpeed(float speedUpdate)
    {
        this.speed = Mathf.Clamp(speedUpdate, 0, int.MaxValue);
    }
}
