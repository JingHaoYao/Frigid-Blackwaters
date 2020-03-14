using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalHeartPendant : ArtifactEffect
{
    int numberTimesTakenDamage = 0;
    ArtifactBonus artifactBonus;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        numberTimesTakenDamage++;
        if(numberTimesTakenDamage == 3)
        {
            artifactBonus.defenseBonus = 0.3f;
            PlayerProperties.playerArtifacts.UpdateUI();
        }
        else if(numberTimesTakenDamage == 4)
        {
            artifactBonus.defenseBonus = 0;
            PlayerProperties.playerArtifacts.UpdateUI();
            numberTimesTakenDamage = 0;
        }
    }
}
