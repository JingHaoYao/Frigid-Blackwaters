using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MithrilPlatedBullets : ArtifactEffect
{
    public override void firedFrontWeapon(GameObject[] bullet, Vector3 whichPositionFiredFrom, float angleTravel)
    {
        foreach (GameObject shot in bullet)
        {
            DamageAmount damageAmount = shot.GetComponent<DamageAmount>();
            if (damageAmount != null)
            {
                damageAmount.addDamage(3);
            }
        }
    }
}
