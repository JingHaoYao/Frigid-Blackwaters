using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeDrops : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    public override void exploredNewRoom(int whatRoomType)
    {
        artifactBonus.healthBonus += 350;
        PlayerProperties.playerArtifacts.UpdateStats();
    }
}
