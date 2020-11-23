using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblersDice : ArtifactEffect
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] GameObject successDice, failureDice;
    ItemTemplates itemTemplates;

    private void Start()
    {
        itemTemplates = FindObjectOfType<ItemTemplates>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement && PlayerProperties.playerInventory.itemList.Count < PlayerItems.maxInventorySize)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    Gamble();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    Gamble();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    Gamble();
                }
            }
        }
    }

    void Gamble()
    {
        DisplayItem gambleItemInstant = gambleItem();
        if (gambleItemInstant == null)
        {
            Instantiate(failureDice, PlayerProperties.playerShipPosition + Vector3.up * 2, Quaternion.identity);
        }
        else
        {
            Instantiate(successDice, PlayerProperties.playerShipPosition + Vector3.up * 2, Quaternion.identity);
            PlayerProperties.playerInventory.itemList.Add(gambleItemInstant.gameObject);
            PlayerProperties.playerInventory.UpdateUI();
        }
    }

    DisplayItem gambleItem()
    {
        int percentItem = Random.Range(1, 101);
        if (percentItem <= 40)
        {
            return null;
        }
        else if (percentItem > 40 && percentItem < 70)
        {
            if (Random.Range(0, 3) != 1)
            {
                GameObject newItem = itemTemplates.loadRandomItem(5);
                newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                return newItem.GetComponent<DisplayItem>();
            }
            else
            {
                GameObject newItem = itemTemplates.loadRandomItem(6);
                newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                return newItem.GetComponent<DisplayItem>();
            }
        }
        else
        {
            int whatArtifact = Random.Range(1, 101);
            if (whatArtifact <= 60)
            {
                GameObject newItem = itemTemplates.loadRandomItem(1);
                newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                return newItem.GetComponent<DisplayItem>();
            }
            else if (whatArtifact > 60 && whatArtifact <= 90)
            {
                GameObject newItem = itemTemplates.loadRandomItem(2);
                newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                return newItem.GetComponent<DisplayItem>();
            }
            else
            {
                GameObject newItem = itemTemplates.loadRandomItem(3);
                newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                return newItem.GetComponent<DisplayItem>();
            }
        }
    }
}
