using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleOfSearingEmbers : ArtifactEffect
{
    int healAmount;
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        healAmount += 600;
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    Heal();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    Heal();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    Heal();
                }
            }
        }
    }

    void Heal()
    {
        PlayerProperties.playerScript.healPlayer(healAmount);
        healAmount = 0;
        FindObjectOfType<AudioManager>().PlaySound("Generic Artifact Sound");
    }
}
