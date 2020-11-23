using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientLavaCore : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;
    bool alreadyApplyingBonus = false;

    IEnumerator applyBuff(int speedBonus, int attackBonus)
    {
        PlayerProperties.durationUI.addTile(displayItem.displayIcon, 5f);
        alreadyApplyingBonus = true;
        artifactBonus.attackBonus += attackBonus;
        artifactBonus.speedBonus += speedBonus;
        PlayerProperties.playerArtifacts.UpdateStats();
        yield return new WaitForSeconds(5f);
        alreadyApplyingBonus = false;
        artifactBonus.attackBonus -= attackBonus;
        artifactBonus.speedBonus -= speedBonus;
        PlayerProperties.playerArtifacts.UpdateStats();
    }

    public override void ignitedPlayer()
    {
        if(!alreadyApplyingBonus)
        {
            StartCoroutine(applyBuff(6, 8));
        } 
    }

    public override void addedFlammableStack(int numberStacks)
    {
        if (!alreadyApplyingBonus)
        {
            StartCoroutine(applyBuff(3, 4));
        }
    }
}
