using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjectionPistol : ArtifactBonus
{
    [SerializeField] DisplayItem displayItem;
    int numberUsages = 0;

    private void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    healPlayerExtra();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    healPlayerExtra();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    healPlayerExtra();
                }
            }
        }
    }

    void healPlayerExtra()
    {
        PlayerProperties.playerArtifacts.numKills -= killRequirement;
        PlayerProperties.playerScript.healPlayer(300 + numberUsages * 300);
        numberUsages++;
    }
}
