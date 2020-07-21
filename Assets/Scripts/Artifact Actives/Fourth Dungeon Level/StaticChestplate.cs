using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticChestplate : ArtifactEffect
{
    [SerializeField] GameObject lightningStrike;

    int accumulatedDamage;
    int numberTimesTookDamage = 0;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if(numberTimesTookDamage == 3)
        {
            GameObject lightningInstant = Instantiate(lightningStrike, enemy.transform.position, Quaternion.identity);
            int damageToInflict = Mathf.FloorToInt((float)accumulatedDamage / 400);
            enemy.dealDamage(damageToInflict);
            lightningInstant.transform.localScale = new Vector3(damageToInflict / 2f, damageToInflict / 2f);
             
            numberTimesTookDamage = 0;
            accumulatedDamage = 0;
        }
        else
        {
            numberTimesTookDamage++;
            accumulatedDamage += amountDamage;
        }
    }
}
