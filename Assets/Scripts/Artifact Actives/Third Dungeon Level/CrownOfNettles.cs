using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownOfNettles : ArtifactEffect
{
    public GameObject nettle;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if(Vector2.Distance(enemy.transform.position, PlayerProperties.playerShipPosition) < 4)
        {
            Instantiate(nettle, enemy.transform.position, Quaternion.identity);
        }
    }
}
