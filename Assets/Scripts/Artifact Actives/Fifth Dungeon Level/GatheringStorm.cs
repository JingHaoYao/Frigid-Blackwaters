using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringStorm : ArtifactEffect
{
    int damageBonus;

    IEnumerator damageIncreaseRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            if (damageBonus < 8)
            {
                damageBonus += 1;
            }
        }
    }

    private void Start()
    {
        StartCoroutine(damageIncreaseRoutine());
    }

    public override void firedFrontWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        foreach(GameObject bulletInstant in bullet)
        {
            DamageAmount damageAmount = bulletInstant.GetComponent<DamageAmount>();
            if(damageAmount != null)
            {
                damageAmount.addDamage(damageBonus);
                damageBonus = 0;
            }
        }
    }

    public override void firedLeftWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        foreach (GameObject bulletInstant in bullet)
        {
            DamageAmount damageAmount = bulletInstant.GetComponent<DamageAmount>();
            if (damageAmount != null)
            {
                damageAmount.addDamage(damageBonus);
                damageBonus = 0;
            }
        }
    }

    public override void firedRightWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        foreach (GameObject bulletInstant in bullet)
        {
            DamageAmount damageAmount = bulletInstant.GetComponent<DamageAmount>();
            if (damageAmount != null)
            {
                damageAmount.addDamage(damageBonus);
                damageBonus = 0;
            }
        }
    }
}
