using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormInABottle : ArtifactEffect
{
    [SerializeField] GameObject stormElemental;
    StormElemental elementalScriptInstant;

    public override void artifactEquipped()
    {
        GameObject stormInstant = Instantiate(stormElemental, PlayerProperties.playerShipPosition, Quaternion.identity);
        elementalScriptInstant = stormInstant.GetComponent<StormElemental>();
    }

    public override void artifactUnequipped()
    {
        elementalScriptInstant.removeTornado();
    }

    public override void cameraMovedPosition(Vector3 currentPosition)
    {
        elementalScriptInstant.transform.position = PlayerProperties.playerShipPosition;
    }
}
