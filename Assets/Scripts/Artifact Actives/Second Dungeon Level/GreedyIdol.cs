using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyIdol : ArtifactBonus
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true && updatedInventory == true)
        {
            updatedInventory = false;
            healthBonus = 500 - 300 * Mathf.FloorToInt(FindObjectOfType<Inventory>().tallyGold() / 1000f);
            artifacts.UpdateUI();
        }
    }
}
