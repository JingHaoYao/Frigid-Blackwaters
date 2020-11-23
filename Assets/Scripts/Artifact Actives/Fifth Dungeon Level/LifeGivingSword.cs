using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGivingSword : ArtifactEffect
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] GameObject healingCircle;

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    heal();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    heal();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    heal();
                }
            }
        }
    }

    public override void artifactDestroyed()
    {
        StopAllCoroutines();
        PlayerProperties.playerScript.removeRootingObject();
    }

    void heal()
    {
        StartCoroutine(healLoop());
        StartCoroutine(rootLoop());
    }

    IEnumerator healLoop()
    {
        GameObject healingCircleInstant = Instantiate(healingCircle, PlayerProperties.playerShipPosition, Quaternion.identity);
        healingCircleInstant.GetComponent<LifeGivingSwordCircle>().Initialize(10);
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1f);
            PlayerProperties.playerScript.healPlayer(1000);
        }
    }

    IEnumerator rootLoop()
    {
        float period = 0;
        while(period < 10)
        {
            period += Time.deltaTime;
            PlayerProperties.playerScript.addRootingObject();

            yield return null;
        }

        PlayerProperties.playerScript.removeRootingObject();
    }

}
