using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptingScepter : MonoBehaviour
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject speedParticles, attackParticles, healingParticles;
    [SerializeField] Sprite speedSprite, attackSprite, healingSprite;

    void Cycle()
    {
        if (PlayerProperties.playerScript.enemiesDefeated == false)
        {
            audioSource.Play();
            if (artifactBonus.speedBonus == 4)
            {
                artifactBonus.attackBonus = 6;
                artifactBonus.speedBonus = 0;
                Instantiate(attackParticles, PlayerProperties.playerShipPosition, Quaternion.identity);
                displayItem.displayIcon = attackSprite;
                PlayerProperties.playerArtifacts.UpdateUI();
            }
            else if (artifactBonus.attackBonus == 6)
            {
                artifactBonus.attackBonus = 0;
                artifactBonus.periodicHealing = 1200;
                Instantiate(healingParticles, PlayerProperties.playerShipPosition, Quaternion.identity);
                displayItem.displayIcon = healingSprite;
                PlayerProperties.playerArtifacts.UpdateUI();
            }
            else
            {
                artifactBonus.speedBonus = 4;
                artifactBonus.periodicHealing = 0;
                Instantiate(speedParticles, PlayerProperties.playerShipPosition, Quaternion.identity);
                displayItem.displayIcon = speedSprite;
                PlayerProperties.playerArtifacts.UpdateUI();
            }
        }
    }

    int healAmount = 0;

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    Cycle();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    Cycle();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    Cycle();
                }
            }
        }
    }
}
