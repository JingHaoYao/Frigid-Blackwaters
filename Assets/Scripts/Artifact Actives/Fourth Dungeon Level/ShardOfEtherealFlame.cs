using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardOfEtherealFlame : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    public override void updatedInventory()
    {
        int count = 0;
        foreach(GameObject item in PlayerProperties.playerInventory.itemList)
        {
            if (item.GetComponent<DisplayItem>().isArtifact)
            {
                count++;
            }
        }

        count += PlayerProperties.playerArtifacts.activeArtifacts.Count;

        artifactBonus.periodicHealing = count * 150;
        PlayerProperties.playerArtifacts.UpdateStats();
    }
}
