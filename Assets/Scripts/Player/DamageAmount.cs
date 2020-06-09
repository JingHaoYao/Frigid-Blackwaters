using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAmount : MonoBehaviour {
    public int damage = 0;
    public int originDamage = 0;
    public bool addBonuses = true;
    private int miscBonusDamage = 0;
    PlayerScript playerScript;

    private void Start()
    {
        playerScript = PlayerProperties.playerScript;
        if (addBonuses == true)
        {
            damage = Mathf.Clamp(originDamage + playerScript.attackBonus + playerScript.conAttackBonus + miscBonusDamage, 1, int.MaxValue);
        }
        else
        {
            damage = originDamage;
        }
    }

    public void updateDamage()
    {
        if (addBonuses == true)
        {
            damage = Mathf.Clamp(originDamage + playerScript.attackBonus + playerScript.conAttackBonus + miscBonusDamage, 1, int.MaxValue);
        }
        else
        {
            damage = originDamage;
        }
    }

    public void addDamage(int addedDamage)
    {
        miscBonusDamage += addedDamage;
        updateDamage();
    }
}
