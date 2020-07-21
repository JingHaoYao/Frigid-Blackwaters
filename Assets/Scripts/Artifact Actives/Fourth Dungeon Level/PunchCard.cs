using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchCard : ArtifactEffect
{
    ItemTemplates itemTemplates;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject coinEffect;

    private void Start()
    {
        itemTemplates = FindObjectOfType<ItemTemplates>();
    }

    public override void addedKill(string tag, Vector3 deathPos, Enemy enemy)
    {
        int goldToAdd = 200;
        foreach (GameObject item in PlayerProperties.playerInventory.itemList)
        {
            DisplayItem displayItem = item.GetComponent<DisplayItem>();
            if (displayItem.goldValue > 0 && displayItem.goldValue < 1000) {
                int goldToSubtract = Mathf.Clamp(goldToAdd, 0, 1000 - displayItem.goldValue);
                goldToAdd -= goldToSubtract;
                displayItem.goldValue += goldToSubtract;
            }

            if (goldToAdd <= 0)
            {
                break;
            }
        }

        if(goldToAdd > 0 && PlayerProperties.playerInventory.itemList.Count < PlayerItems.maxInventorySize) {
            GameObject goldInstant = Instantiate(itemTemplates.gold);
            goldInstant.GetComponent<DisplayItem>().goldValue = goldToAdd;
            PlayerProperties.playerInventory.itemList.Add(goldInstant);
        }

        if(goldToAdd < 200)
        {
            Instantiate(coinEffect, PlayerProperties.playerShipPosition, Quaternion.identity);
            audioSource.Play();
        }

        PlayerItems.totalGoldAmount += 200 - goldToAdd;
    }
}
