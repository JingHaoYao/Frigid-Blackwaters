using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperThrusterBlaster : ArtifactEffect
{
    [SerializeField] GameObject blasterProjectile;
    [SerializeField] ArtifactBonus artifactBonus;

    public override void playerDashed()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject blasterProjectileInstant = Instantiate(blasterProjectile, PlayerProperties.playerShipPosition, Quaternion.identity);
            blasterProjectileInstant.GetComponent<BasicProjectile>().angleTravel = PlayerProperties.playerScript.whatAngleTraveled + 180 - 10 + 5 * i;
        }

        StartCoroutine(burstOfSpeed());
    }

    IEnumerator burstOfSpeed()
    {
        artifactBonus.speedBonus += 3;
        PlayerProperties.playerArtifacts.UpdateStats();

        yield return new WaitForSeconds(1f);

        artifactBonus.speedBonus -= 3;
        PlayerProperties.playerArtifacts.UpdateStats();
    }
}
