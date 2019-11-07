using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAmount : MonoBehaviour {
    public int damage = 0;
    public int originDamage = 0;
    public bool addBonuses = true;
    PlayerScript playerScript;

    private void Awake()
    {
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        if (addBonuses == true)
        {
            damage += Mathf.Clamp(originDamage + playerScript.attackBonus + playerScript.conAttackBonus, 1, int.MaxValue);
        }
        else
        {
            damage += originDamage;
        }
    }

    public void updateDamage()
    {
        if (addBonuses == true)
        {
            damage = originDamage + playerScript.attackBonus + playerScript.conAttackBonus;
        }
        else
        {
            damage = originDamage;
        }
    }
}
