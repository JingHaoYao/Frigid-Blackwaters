using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmethystCluster : ArtifactBonus
{
    [SerializeField] DisplayItem displayItem;
    bool isHealing = false;

    private void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    if (!isHealing)
                    {
                        StartCoroutine(damageAndHeal());
                    }
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    if (!isHealing)
                    {
                        StartCoroutine(damageAndHeal());
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    if (!isHealing)
                    {
                        StartCoroutine(damageAndHeal());
                    }
                }
            }
        }
    }

    IEnumerator damageAndHeal()
    {
        PlayerProperties.playerArtifacts.numKills -= killRequirement;
        isHealing = true;
        PlayerProperties.playerScript.dealTrueDamageToShip(500);
        PlayerProperties.durationUI.addTile(displayItem.displayIcon, 3f);
        yield return new WaitForSeconds(3f);
        PlayerProperties.playerScript.healPlayer(1300);
        isHealing = false;
    }
}
