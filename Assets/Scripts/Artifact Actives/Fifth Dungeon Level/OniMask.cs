using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniMask : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;
    public override void addedKill(string tag, Vector3 deathPos, Enemy enemy)
    {
        artifactBonus.speedBonus += 0.2f;
        artifactBonus.healthBonus += 200;
        PlayerProperties.playerScript.healPlayer(200);
    }
}
