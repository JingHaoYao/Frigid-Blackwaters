using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusoryStaff : ArtifactEffect
{
    public override void firedFrontWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        int randomDamage = Random.Range(-4, 5);
        foreach(GameObject projectile in bullet)
        {
            projectile.GetComponent<DamageAmount>().addDamage(randomDamage);
        }
    }

    public override void firedRightWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        int randomDamage = Random.Range(-4, 5);
        foreach (GameObject projectile in bullet)
        {
            projectile.GetComponent<DamageAmount>().addDamage(randomDamage);
        }
    }

    public override void firedLeftWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        int randomDamage = Random.Range(-4, 5);
        foreach (GameObject projectile in bullet)
        {
            projectile.GetComponent<DamageAmount>().addDamage(randomDamage);
        }
    }
}
