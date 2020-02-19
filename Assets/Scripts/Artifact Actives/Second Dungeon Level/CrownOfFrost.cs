using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownOfFrost : ArtifactBonus
{
    [SerializeField] DisplayItem displayItem;
    public GameObject icePrison;
    private GameObject icePrisonInstant;

    void summonIcePrison()
    {
        PlayerProperties.playerArtifacts.numKills -= killRequirement;
        icePrisonInstant = Instantiate(icePrison, PlayerProperties.playerShipPosition, Quaternion.identity);
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    summonIcePrison();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    summonIcePrison();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    summonIcePrison();
                }
            }
        }
    }
}
