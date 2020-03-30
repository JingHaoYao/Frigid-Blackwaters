using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiganticPodlingSeed : ArtifactEffect
{
    public GameObject airBurst;


    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        GameObject airBurstInstant = Instantiate(airBurst, PlayerProperties.playerShipPosition + Vector3.up * 1.5f, Quaternion.identity);
        airBurstInstant.GetComponent<PodAirEffect>().targetEnemy = enemy;
    }
}
