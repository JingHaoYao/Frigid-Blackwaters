using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RejuvenatingCrystal : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;
    [SerializeField] AudioSource audioSource;

    [SerializeField] GameObject plant;
    List<RejuvenatingCrystalPlant> plantInstances = new List<RejuvenatingCrystalPlant>();

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    SpawnCrystal();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    SpawnCrystal();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    SpawnCrystal();
                }
            }
        }
    }

    void SpawnCrystal()
    {
        audioSource.Play();
        GameObject newCrystal = Instantiate(plant, PlayerProperties.playerShipPosition, Quaternion.identity);
        plantInstances.Add(newCrystal.GetComponent<RejuvenatingCrystalPlant>());
    }

    public override void exploredNewRoom(int whatRoomType)
    {
        foreach(RejuvenatingCrystalPlant plant in plantInstances)
        {
            if (plant != null)
            {
                plant.growPlant();
            }
        }
    }

}
