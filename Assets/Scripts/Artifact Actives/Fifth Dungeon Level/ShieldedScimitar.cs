using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedScimitar : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    private void Update()
    {
        if (((float)PlayerProperties.playerScript.shipHealth / PlayerProperties.playerScript.shipHealthMAX) > 0.5f)
        {
            artifactBonus.attackBonus = 3;
            artifactBonus.defenseBonus = 0;
            PlayerProperties.playerArtifacts.UpdateStats();
        }
        else
        {
            artifactBonus.defenseBonus = 0.2f;
            artifactBonus.attackBonus = 0;
            PlayerProperties.playerArtifacts.UpdateStats();
        }
    }
}
