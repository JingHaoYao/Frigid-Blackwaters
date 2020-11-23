using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplifyingCrystal : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    void UpdateStats()
    {
        int totalHealthBonus = 0;

        foreach (GameObject artifact in PlayerProperties.playerArtifacts.activeArtifacts)
        {
            if(artifact != this.gameObject)
            {
                totalHealthBonus += artifact.GetComponent<ArtifactBonus>().healthBonus;
            }
        }

        if (artifactBonus.healthBonus != totalHealthBonus)
        {
            artifactBonus.healthBonus = Mathf.RoundToInt(totalHealthBonus * 1.5f);
            PlayerProperties.playerArtifacts.UpdateStats();
        }
    }

    public override void artifactEquipped()
    {
        UpdateStats();
    }

    public override void updatedInventory()
    {
        UpdateStats();
    }

    
}
