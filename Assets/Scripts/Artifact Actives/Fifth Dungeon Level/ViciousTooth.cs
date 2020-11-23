using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViciousTooth : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
        if(artifactBonus.speedBonus < 4)
        {
            StartCoroutine(grantSpeedBonus());
        }
    }

    IEnumerator grantSpeedBonus()
    {
        artifactBonus.speedBonus += 1;
        PlayerProperties.playerArtifacts.UpdateStats();

        yield return new WaitForSeconds(3f);
        artifactBonus.speedBonus -= 1;
        PlayerProperties.playerArtifacts.UpdateStats();
    }
}
