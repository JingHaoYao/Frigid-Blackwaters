using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlingTangleVine : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    public override void addedKill(string tag, Vector3 deathPos, Enemy enemy)
    {
        artifactBonus.healthBonus += 50;
        PlayerProperties.playerArtifacts.UpdateUI();
    }
}
