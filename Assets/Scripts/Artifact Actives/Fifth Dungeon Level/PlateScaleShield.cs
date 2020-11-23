using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateScaleShield : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    bool inLoop = false;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if (!inLoop)
        {
            StartCoroutine(damageRoutine());
        }

        if(artifactBonus.defenseBonus < 0.5f)
        {
            artifactBonus.defenseBonus += 0.1f;
            PlayerProperties.playerArtifacts.UpdateStats();
        }
    }

    IEnumerator damageRoutine()
    {
        inLoop = true;
        yield return new WaitForSeconds(8f);

        inLoop = false;
        artifactBonus.defenseBonus = 0.1f;
        PlayerProperties.playerArtifacts.UpdateStats();
    }
}
