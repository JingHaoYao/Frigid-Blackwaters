using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySafeUpgradeManager : MonoBehaviour {
    Inventory inventory;
    int prevNumberUpgrades1;
    int prevNumberUpgrades2;

    void applyUpgrades()
    {
        if (PlayerUpgrades.inventoryUpgrades.Count == 1)
        {
            PlayerItems.maxInventorySize = 15;
        }
        else if (PlayerUpgrades.inventoryUpgrades.Count == 2)
        {
            PlayerItems.maxInventorySize = 20;
        }
        else if (PlayerUpgrades.inventoryUpgrades.Count == 3)
        {
            PlayerItems.maxInventorySize = 25;
        }
        else if(PlayerUpgrades.inventoryUpgrades.Count == 0)
        {
            PlayerItems.maxInventorySize = 10;
        }

        inventory.inventorySize = PlayerItems.maxInventorySize;

        if(PlayerUpgrades.safeUpgrades.Count == 1)
        {

        }
        else if(PlayerUpgrades.safeUpgrades.Count == 2)
        {

        }
        else if(PlayerUpgrades.safeUpgrades.Count == 3)
        {

        }
        else
        {

        }
    }

    void Start()
    {
        inventory = GetComponent<Inventory>();
        prevNumberUpgrades1 = PlayerUpgrades.inventoryUpgrades.Count;
        prevNumberUpgrades2 = PlayerUpgrades.hullUpgrades.Count;
        applyUpgrades();
    }

    void Update()
    {
        if (prevNumberUpgrades1 != PlayerUpgrades.inventoryUpgrades.Count || prevNumberUpgrades2 != PlayerUpgrades.safeUpgrades.Count)
        {
            prevNumberUpgrades1 = PlayerUpgrades.inventoryUpgrades.Count;
            prevNumberUpgrades2 = PlayerUpgrades.safeUpgrades.Count;
            applyUpgrades();
        }
    }
}
