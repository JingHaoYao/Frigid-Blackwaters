using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorptionGel : ArtifactEffect
{
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        StartCoroutine(dealDamageOverTime(amountDamage));
    }

    IEnumerator dealDamageOverTime(int damageToReduce)
    {
        int damageTotal = damageToReduce;
        while(damageTotal > 0)
        {
            if(damageTotal > 200)
            {
                damageTotal -= 200;
                PlayerProperties.playerScript.dealTrueDamageToShip(200);
            }
            else
            {
                PlayerProperties.playerScript.dealTrueDamageToShip(damageTotal);
                damageTotal = 0;
            }
            yield return new WaitForSeconds(1.25f);
        }
    }
}
