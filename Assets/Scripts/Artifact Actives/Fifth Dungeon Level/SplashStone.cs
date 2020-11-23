using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashStone : MonoBehaviour
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] AudioSource splashAudio;
    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    HealAndCleanse();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    HealAndCleanse();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    HealAndCleanse();
                }
            }
        }
    }
    
    void HealAndCleanse()
    {
        splashAudio.Play();
        PlayerProperties.playerScript.healPlayer(800);
        if (PlayerProperties.flammableController != null)
        {
            PlayerProperties.flammableController.RemoveAllFlammableStacks();
        }
    }
}
