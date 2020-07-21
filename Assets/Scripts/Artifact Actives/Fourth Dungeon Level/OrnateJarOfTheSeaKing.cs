using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrnateJarOfTheSeaKing : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    public override void updatedInventory()
    {
        artifactBonus.defenseBonus = Mathf.Clamp(0.3f - Mathf.FloorToInt(PlayerItems.totalGoldAmount / 1000) * 0.05f, 0, 0.3f);
        PlayerProperties.playerArtifacts.UpdateStats();
    }
}
