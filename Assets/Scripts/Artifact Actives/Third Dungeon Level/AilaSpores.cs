using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AilaSpores : ArtifactEffect
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;
    int damageTakenTotal = 0;

    void Start()
    {
        
    }

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        StartCoroutine(addToDamageTotalThenTakeAway(amountDamage));
    }

    IEnumerator addToDamageTotalThenTakeAway(int damageTaken)
    {
        damageTakenTotal += damageTaken;
        yield return new WaitForSeconds(3f);
        damageTakenTotal -= damageTaken;
    }

    void healPlayer()
    {
        PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
        PlayerProperties.playerScript.healPlayer(Mathf.RoundToInt(damageTakenTotal * 0.75f));
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    healPlayer();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    healPlayer();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    healPlayer();
                }
            }
        }
    }
}
