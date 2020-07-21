using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZephyrBlade : ArtifactBonus
{
    [SerializeField] GameObject slash;
    [SerializeField] DisplayItem displayItem;
    [SerializeField] AudioSource audioSource;

    void summonSlashes()
    {
        PlayerProperties.playerArtifacts.numKills -= killRequirement;
        audioSource.Play();

        for(int i = 0; i < 3; i++)
        {
            float angle = PlayerProperties.currentPlayerTravelDirection - 15 + 15 * i;
            GameObject slashInstant = Instantiate(slash, PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)), Quaternion.identity);
            slashInstant.GetComponent<BasicProjectile>().angleTravel = angle;
        }
    }

    private void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    summonSlashes();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    summonSlashes();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    summonSlashes();
                }
            }
        }
    }
}
