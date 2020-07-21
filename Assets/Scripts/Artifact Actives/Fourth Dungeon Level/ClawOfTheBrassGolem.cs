using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawOfTheBrassGolem : ArtifactEffect
{
    int accumulationDamage = 0;

    [SerializeField] ArtifactBonus artifactBonus;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        accumulationDamage += amountDamage;

        if(accumulationDamage >= 1500 && artifactBonus.speedBonus != 2)
        {
            artifactBonus.speedBonus = 2;
            PlayerProperties.playerArtifacts.UpdateStats();
        }
    }
}
