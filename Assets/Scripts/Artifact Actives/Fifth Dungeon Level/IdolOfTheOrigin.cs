using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdolOfTheOrigin : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    public override void cameraMovedPosition(Vector3 currentPosition)
    {
        float initialDistance = Vector2.Distance(PlayerProperties.playerShipPosition, Vector3.zero);
        int score = Mathf.RoundToInt(initialDistance / 20);
        artifactBonus.periodicHealing = 400 + 200 * score;
        PlayerProperties.playerArtifacts.UpdateStats();
    }
}
