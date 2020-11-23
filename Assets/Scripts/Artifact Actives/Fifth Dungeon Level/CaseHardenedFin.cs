using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseHardenedFin : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        StartCoroutine(applySpeedBonus());
    }

    IEnumerator applySpeedBonus()
    {
        artifactBonus.speedBonus += 3;
        PlayerProperties.playerArtifacts.UpdateStats();
        yield return new WaitForSeconds(3f);
        artifactBonus.speedBonus -= 3;
        PlayerProperties.playerArtifacts.UpdateStats();
    }
}
