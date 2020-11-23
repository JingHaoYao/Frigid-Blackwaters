using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheSadMachine : ArtifactEffect
{
    [SerializeField] GameObject sadMachineMinion;
    GameObject currentInstant;
    public override void artifactEquipped()
    {
        currentInstant = Instantiate(sadMachineMinion, PlayerProperties.playerShipPosition, Quaternion.identity);
    }

    public override void artifactUnequipped()
    {
        Destroy(currentInstant);
    }
}
