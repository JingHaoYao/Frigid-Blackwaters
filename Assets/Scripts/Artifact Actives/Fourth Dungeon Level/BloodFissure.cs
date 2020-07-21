using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFissure : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        artifactBonus.attackBonus = Mathf.FloorToInt((PlayerProperties.playerScript.shipHealthMAX - PlayerProperties.playerScript.shipHealth) / 500f);
        PlayerProperties.playerArtifacts.UpdateStats();
    }

    public override void healed(int healingAmount)
    {
        artifactBonus.attackBonus = Mathf.FloorToInt((PlayerProperties.playerScript.shipHealthMAX - PlayerProperties.playerScript.shipHealth) / 500f);
        PlayerProperties.playerArtifacts.UpdateStats();
    }

}
