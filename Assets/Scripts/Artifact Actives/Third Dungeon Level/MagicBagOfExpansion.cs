using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBagOfExpansion : ArtifactEffect {
    [SerializeField] ArtifactBonus artifactBonus;

    public override void updatedInventory()
    {
        artifactBonus.healthBonus = 150 * PlayerProperties.playerInventory.itemList.Count;
        PlayerProperties.playerArtifacts.UpdateUI();
    }
}
