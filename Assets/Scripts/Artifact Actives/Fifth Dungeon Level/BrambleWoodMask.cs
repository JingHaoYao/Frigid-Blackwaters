using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrambleWoodMask : ArtifactEffect
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] AudioSource audioSource;
    int healAmount = 0;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        healAmount += Mathf.RoundToInt(amountDamage * 0.5f);
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
        audioSource.Play();
        PlayerProperties.playerScript.healPlayer(200);
        StartCoroutine(healDamage());
    }

    IEnumerator healDamage()
    {
        healAmount = 0;
        yield return new WaitForSeconds(4f);
        PlayerProperties.playerScript.healPlayer(healAmount);
    }
}
