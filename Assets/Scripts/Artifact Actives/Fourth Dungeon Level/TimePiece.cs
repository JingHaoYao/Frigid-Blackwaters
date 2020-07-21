using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePiece : ArtifactEffect
{
    int damageToRevert;
    [SerializeField] GameObject stopWatchEffect;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;

    IEnumerator delayStasisPeriod()
    {
        yield return new WaitForEndOfFrame();
        int damage = PlayerProperties.playerScript.trueDamage;
        yield return new WaitForSeconds(3f);
        damageToRevert = damage;
    }

    void activateArtifact()
    {
        PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
        GameObject instant = Instantiate(stopWatchEffect, PlayerProperties.playerShipPosition, Quaternion.identity);
        instant.GetComponent<FollowObject>().objectToFollow = PlayerProperties.playerShip;
        PlayerProperties.playerScript.trueDamage = damageToRevert;
    }

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        StartCoroutine(delayStasisPeriod());
    }

    public override void healed(int healingAmount)
    {
        StartCoroutine(delayStasisPeriod());
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    activateArtifact();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    activateArtifact();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    activateArtifact();
                }
            }
        }
    }

}
