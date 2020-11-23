using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinisterMace : ArtifactEffect
{
    [SerializeField] GameObject bloodOrb;
    public override void addedKill(string tag, Vector3 deathPos, Enemy enemy)
    {
        Instantiate(bloodOrb, deathPos + Vector3.up, Quaternion.identity);
    }
}
