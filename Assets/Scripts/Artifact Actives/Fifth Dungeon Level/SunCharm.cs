using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCharm : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    IEnumerator buffAndHeal()
    {
        artifactBonus.healthBonus += 2000;
        PlayerProperties.playerArtifacts.UpdateStats();
        PlayerProperties.playerScript.healPlayer(2000);

        yield return new WaitForSeconds(20f);

        artifactBonus.healthBonus -= 2000;
        PlayerProperties.playerArtifacts.UpdateStats();
    }

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if(PlayerProperties.playerArtifacts.numKills >= 8 && ((float)PlayerProperties.playerScript.shipHealth / PlayerProperties.playerScript.shipHealthMAX) <= 0.25f)
        {
            PlayerProperties.playerArtifacts.numKills -= 8;
            StartCoroutine(buffAndHeal());
        }
    }
}
