using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentOfTheWheel : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;
    Coroutine resetRoutine;

    public override void artifactEquipped()
    {
        resetRoutine = StartCoroutine(resetBonus());
    }

    public override void playerDashed()
    {
        artifactBonus.attackBonus = 0;
        PlayerProperties.playerArtifacts.UpdateStats();
        StopCoroutine(resetRoutine);
        resetRoutine = StartCoroutine(resetBonus());
    }

    public override void artifactUnequipped()
    {
        StopAllCoroutines();
    }

    IEnumerator resetBonus()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);

            if(artifactBonus.attackBonus < 8)
            {
                artifactBonus.attackBonus++;
                PlayerProperties.playerArtifacts.UpdateStats();
            }
        }
    }
}
