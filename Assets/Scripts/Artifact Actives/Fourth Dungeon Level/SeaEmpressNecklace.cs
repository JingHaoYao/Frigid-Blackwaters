using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaEmpressNecklace : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;
    public override void playerDashed()
    {
        StartCoroutine(speedBonus());
        PlayerProperties.durationUI.addTile(displayItem.displayIcon, 1f);   
    }

    IEnumerator speedBonus()
    {
        artifactBonus.speedBonus = 2;
        PlayerProperties.playerArtifacts.UpdateStats();

        yield return new WaitForSeconds(1f);

        artifactBonus.speedBonus = 0;
        PlayerProperties.playerArtifacts.UpdateStats();
    }
}
