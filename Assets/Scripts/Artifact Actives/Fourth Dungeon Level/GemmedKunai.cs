using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemmedKunai : ArtifactEffect
{
    int currentWeaponFiredCount = 0;

    void refreshCooldown(ShipWeaponScript script)
    {
        currentWeaponFiredCount++;
        if(currentWeaponFiredCount >= 5)
        {
            currentWeaponFiredCount = 0;
            script.setCoolDownPeriod(0);
        }
    }

    public override void firedFrontWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        refreshCooldown(PlayerProperties.frontWeapon);
    }

    public override void firedLeftWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        refreshCooldown(PlayerProperties.leftWeapon);
    }

    public override void firedRightWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        refreshCooldown(PlayerProperties.rightWeapon);
    }
}
