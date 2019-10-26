using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScale : ArtifactBonus
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    Inventory inventory;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true && updatedInventory == true)
        {
            updatedInventory = false;
            speedBonus = -0.1f * Mathf.RoundToInt(inventory.tallyGold() / 100f);
            healthBonus = 75 * Mathf.RoundToInt(inventory.tallyGold() / 100f);
            artifacts.UpdateUI();
        }
    }
}
