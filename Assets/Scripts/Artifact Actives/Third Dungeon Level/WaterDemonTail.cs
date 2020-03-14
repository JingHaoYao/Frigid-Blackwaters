using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDemonTail : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;
    public override void updatedInventory()
    {
        if (PlayerProperties.playerInventory.itemList.Count > 0 && PlayerProperties.playerInventory.itemList.Count % 2 == 0)
        {
            artifactBonus.healthBonus = 900;
        }
        else
        {
            artifactBonus.healthBonus = -300;
        }
        PlayerProperties.playerArtifacts.UpdateUI();
    }
}
