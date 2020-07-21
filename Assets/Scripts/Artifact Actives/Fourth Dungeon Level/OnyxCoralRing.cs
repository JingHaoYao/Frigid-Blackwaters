using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnyxCoralRing : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    public override void exploredNewRoom(int whatRoomType)
    {
        artifactBonus.healthBonus += 200;
        PlayerProperties.playerArtifacts.UpdateStats();
    }
}
