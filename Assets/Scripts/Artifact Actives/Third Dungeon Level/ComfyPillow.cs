using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComfyPillow : ArtifactEffect
{
    bool isReadyToHeal = false;
    [SerializeField] DisplayItem displayItem;
    IEnumerator waitToHeal(float waitDuration)
    {
        PlayerProperties.durationUI.addTile(displayItem.displayIcon, waitDuration);
        yield return new WaitForSeconds(waitDuration);
        isReadyToHeal = true;
    }


    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if (isReadyToHeal)
        {
            PlayerProperties.playerScript.healPlayer(Mathf.RoundToInt(amountDamage / 2f));
            isReadyToHeal = false;
            StartCoroutine(waitToHeal(5));
        }
    }
}
