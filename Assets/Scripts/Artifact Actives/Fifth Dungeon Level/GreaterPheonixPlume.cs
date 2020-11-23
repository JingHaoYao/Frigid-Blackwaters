using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreaterPheonixPlume : ArtifactBonus
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] GameObject pheonixPlumeFeather;

    void SummonPlumeFeathers()
    {
        PlayerProperties.playerArtifacts.numKills -= killRequirement;
        for (int i = 0; i < 8; i++)
        {
            float angleTravel = i * 45;
            GameObject pheonixPlumeFeatherInstant = Instantiate(pheonixPlumeFeather, PlayerProperties.playerShipPosition, Quaternion.identity);
            pheonixPlumeFeatherInstant.GetComponent<GreaterPheonixPlumeFeather>().Initialize(angleTravel);
        }
    }

    private void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    SummonPlumeFeathers();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    SummonPlumeFeathers();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    SummonPlumeFeathers();
                }
            }
        }
    }
}
