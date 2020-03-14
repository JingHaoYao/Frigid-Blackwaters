using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedSprout : ArtifactEffect
{

    int numberBulletsFired = 0;
    public override void firedFrontWeapon(GameObject[] bullet, Vector3 whichPositionFiredFrom, float angleTravel)
    {
        firePowerShot(bullet);
    }

    public override void firedLeftWeapon(GameObject[] bullet, Vector3 whichPositionFiredFrom, float angleTravel)
    {
        firePowerShot(bullet);
    }

    public override void firedRightWeapon(GameObject[] bullet, Vector3 whichPositionFiredFrom, float angleTravel)
    {
        firePowerShot(bullet);
    }

    void firePowerShot(GameObject[] bullets)
    {
        numberBulletsFired++;
        if(numberBulletsFired == 4)
        {
            foreach(GameObject bullet in bullets)
            {
                DamageAmount damageAmount = bullet.GetComponent<DamageAmount>();
                damageAmount.originDamage += 3;
                damageAmount.updateDamage();
            }

            PlayerScript playerScript = PlayerProperties.playerScript;
            float angleTravel = Mathf.Atan2(PlayerProperties.cursorPosition.y - PlayerProperties.playerShipPosition.y, PlayerProperties.cursorPosition.x - PlayerProperties.playerShipPosition.x) + Mathf.PI;

            playerScript.momentumVector = new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel)) * 8;
            playerScript.momentumMagnitude = 8;
            playerScript.momentumDuration = 0.75f;
            numberBulletsFired = 0;
        }
    }
}
