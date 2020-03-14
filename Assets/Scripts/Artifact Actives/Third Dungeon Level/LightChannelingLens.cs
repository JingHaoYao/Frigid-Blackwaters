using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChannelingLens : MonoBehaviour
{
    [SerializeField] DisplayItem displayItem;
    PlayerScript playerScript;
    public GameObject lightRay;
    [SerializeField] ArtifactBonus artifactBonus;
    GameObject lightRayInstant;

    void Start()
    {
        playerScript = PlayerProperties.playerScript;
    }

    void spawnLaser()
    {
        if (lightRayInstant == null)
        {
            PlayerProperties.durationUI.addTile(displayItem.displayIcon, 10 / 12 + 4);
            PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
            lightRayInstant = Instantiate(lightRay, PlayerProperties.cursorPosition, Quaternion.identity);
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
                    spawnLaser();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    spawnLaser();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    spawnLaser();
                }
            }
        }
    }
}
