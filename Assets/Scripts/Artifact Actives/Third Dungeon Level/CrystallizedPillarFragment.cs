using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystallizedPillarFragment : MonoBehaviour
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] GameObject effectCircle;
    GameObject effectCircleInstant;

    void spawnCircle()
    {
        if (effectCircleInstant == null)
        {
            PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
            effectCircleInstant = Instantiate(effectCircle, PlayerProperties.playerShipPosition, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    spawnCircle();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    spawnCircle();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    spawnCircle();
                }
            }
        }
    }
}
