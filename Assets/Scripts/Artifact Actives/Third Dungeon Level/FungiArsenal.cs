using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungiArsenal : MonoBehaviour
{
    public GameObject[] fungiConsumableList;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;
    [SerializeField] AudioSource audioSource;


    void grantMushroom()
    {
        if (PlayerProperties.playerInventory.itemList.Count < PlayerItems.maxInventorySize)
        {
            PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
            GameObject newItem = Instantiate(fungiConsumableList[Random.Range(0, 3)]);
            PlayerProperties.playerInventory.itemList.Add(newItem);
            audioSource.Play();
        }
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    grantMushroom();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    grantMushroom();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    grantMushroom();
                }
            }
        }
    }
}
