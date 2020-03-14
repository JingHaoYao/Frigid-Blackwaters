using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadshotMushrooms : ArtifactEffect
{
    public GameObject mushroomProjectile;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;
    int numberShotsRemaining = 0;

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    numberShotsRemaining = 2;
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    numberShotsRemaining = 2;
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    numberShotsRemaining = 2;
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                }
            }
        }
    }


    public override void firedFrontWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        shootMushroomProjectiles(angleTravel, spawnPosition);
    }

    public override void firedLeftWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        shootMushroomProjectiles(angleTravel, spawnPosition);
    }

    public override void firedRightWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        shootMushroomProjectiles(angleTravel, spawnPosition);
    }

    void shootMushroomProjectiles(float angleTravel, Vector3 position)
    {
        if (numberShotsRemaining > 0)
        {
            numberShotsRemaining--;
            for (int i = 0; i < 3; i++)
            {
                float angle = (angleTravel - 10 + 10 * i);
                GameObject mushroomInstant = Instantiate(mushroomProjectile, position, Quaternion.Euler(0, 0, angle));
                mushroomInstant.GetComponent<BasicProjectile>().angleTravel = angle;
            }
        }
    }
}
