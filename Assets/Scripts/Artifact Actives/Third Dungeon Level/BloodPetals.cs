using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPetals : ArtifactEffect
{
    bool isReadyToHeal = false;
    [SerializeField] DisplayItem displayItem;
    IEnumerator waitToHeal(float waitDuration)
    {
        PlayerProperties.durationUI.addTile(displayItem.displayIcon, waitDuration);
        yield return new WaitForSeconds(waitDuration);
        isReadyToHeal = true;
    }

    public override void addedKill(string tag, Vector3 deathPos, Enemy enemy)
    {
        if (isReadyToHeal)
        {
            PlayerProperties.playerScript.healPlayer(75 * enemy.maxHealth);
            isReadyToHeal = false;
            StartCoroutine(waitToHeal(10));
        }
    }
}
