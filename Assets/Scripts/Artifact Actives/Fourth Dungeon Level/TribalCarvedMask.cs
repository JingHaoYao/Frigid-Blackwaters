using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalCarvedMask : ArtifactEffect
{
    bool abilityActive = true;
    [SerializeField] DisplayItem displayItem;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if (abilityActive)
        {
            PlayerProperties.playerScript.healPlayer(Mathf.RoundToInt(amountDamage * 0.75f));
            StartCoroutine(timer());
            PlayerProperties.durationUI.addTile(displayItem.displayIcon, 8f);
        }
    }

    IEnumerator timer()
    {
        abilityActive = false;
        yield return new WaitForSeconds(8f);
        abilityActive = true;
    }
}
