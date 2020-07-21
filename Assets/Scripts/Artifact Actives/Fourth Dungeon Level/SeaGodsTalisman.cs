using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGodsTalisman : ArtifactEffect
{
    [SerializeField] Sprite[] upgradeSprites;
    [SerializeField] int[] upgradeAmounts;
    [SerializeField] DisplayItem displayItem;

    [SerializeField] float speedBonusUpgrade;
    [SerializeField] float defenseBonusUpgrade;
    [SerializeField] int attackBonusUpgrade;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] AudioSource upgradeAudio;

    int currentUpgrade = 0;

    [SerializeField] GameObject upgradeLightning;
    private int numberLifeAdded;

    void upgradeArtifact()
    {
        if (currentUpgrade < 4)
        {
            PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
            currentUpgrade++;
            displayItem.displayIcon = upgradeSprites[currentUpgrade];

            switch(currentUpgrade)
            {
                case 1:
                    this.artifactBonus.defenseBonus = defenseBonusUpgrade;
                    break;
                case 2:
                    this.artifactBonus.speedBonus = speedBonusUpgrade;
                    break;
                case 3:
                    this.artifactBonus.attackBonus = attackBonusUpgrade;
                    break;
                case 4:
                    PlayerProperties.playerScript.numberLives++;
                    numberLifeAdded = PlayerProperties.playerScript.numberLives;
                    break;
            }

            GameObject instant = Instantiate(upgradeLightning, PlayerProperties.playerShipPosition, Quaternion.identity);
            instant.GetComponent<FollowObject>().objectToFollow = PlayerProperties.playerShip;

            if (currentUpgrade < 4)
            {
                artifactBonus.killRequirement = upgradeAmounts[currentUpgrade];
            }
            PlayerProperties.playerArtifacts.UpdateUI();
            upgradeAudio.Play();
        }
    }

    public override void playerDied()
    {
        if(PlayerProperties.playerScript.numberLives == numberLifeAdded && currentUpgrade == 4)
        {
            currentUpgrade = 3;
            displayItem.displayIcon = upgradeSprites[currentUpgrade];
            artifactBonus.killRequirement = upgradeAmounts[currentUpgrade];
            PlayerProperties.playerArtifacts.UpdateUI();
        }
    }

    private void Start()
    {
        artifactBonus.killRequirement = upgradeAmounts[0];
    }

    private void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    upgradeArtifact();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    upgradeArtifact();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    upgradeArtifact();
                }
            }
        }
    }
}
