using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoliathQuarterstaff : ArtifactEffect
{
    [SerializeField] GameObject lightningEffect;
    [SerializeField] GameObject goliathProjectile;
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if (Random.Range(0, 4) == 0)
        {
            Instantiate(lightningEffect, PlayerProperties.playerShipPosition, Quaternion.identity);
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45 * Mathf.Deg2Rad;
                GameObject instant = Instantiate(goliathProjectile, PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 0.5f, Quaternion.identity);
                instant.GetComponent<GoliathQuarterstaffProjectile>().Initialize(PlayerProperties.playerShipPosition);
            }
        }
    }
}
