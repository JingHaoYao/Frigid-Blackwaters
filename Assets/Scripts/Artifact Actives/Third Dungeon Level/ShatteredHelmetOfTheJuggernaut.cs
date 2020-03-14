using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredHelmetOfTheJuggernaut : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;
    bool isBuffed = false;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if (!isBuffed)
        {
            PlayerProperties.durationUI.addTile(displayItem.displayIcon, 1);
            StartCoroutine(buff());
        }
    }

    IEnumerator buff()
    {
        isBuffed = true;
        artifactBonus.attackBonus = 4;
        PlayerProperties.playerArtifacts.UpdateUI();
        yield return new WaitForSeconds(1f);
        artifactBonus.attackBonus = 0;
        PlayerProperties.playerArtifacts.UpdateUI();
        isBuffed = false;
    }
}
