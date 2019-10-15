﻿using System.Collections;
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
        GameObject.Find("QuestManager").GetComponent<QuestManager>().addKill(nameID);
    }

    public void heal(int healAmount)
    {
        if(health + healAmount > maxHealth)
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
        health -= damageDealt;
    }
}
