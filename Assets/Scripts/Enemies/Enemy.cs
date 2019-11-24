using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy: MonoBehaviour
{
    public string nameID;
    public int health;
    public int maxHealth;
    public int dangerValue;
    public int killNumber;
    public bool stopAttacking = false;
    public int percentSpawnChance = 33;
    public int armorMitigation;
    
    public void addKills()
    {
        GameObject.Find("PlayerShip").GetComponent<Artifacts>().numKills += killNumber;
        foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
        {
            if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                slot.displayInfo.GetComponent<ArtifactEffect>().addedKill(this.gameObject.tag, transform.position);
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

    public void dealDamage(int damageAmount)
    {
        int damageDealt = damageAmount - armorMitigation;
        if(damageDealt < 1)
        {
            damageDealt = 1;
        }
        FindObjectOfType<EnemyDamageNumbersUI>().addEnemyDamageUI(damageDealt, this.gameObject);
        health -= damageDealt;
        FindObjectOfType<CameraShake>().shakeCamFunction(0.1f, 0.3f * Mathf.Clamp(((float)damageDealt / maxHealth), 0.1f, 5f));

        foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
        {
            if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                slot.displayInfo.GetComponent<ArtifactEffect>().dealtDamage(damageDealt, this);
        }
    }
}
