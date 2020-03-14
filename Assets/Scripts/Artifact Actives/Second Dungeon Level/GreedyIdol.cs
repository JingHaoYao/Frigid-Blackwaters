using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyIdol : ArtifactEffect
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

    public override void updatedInventory()
    {
        GetComponent<ArtifactBonus>().healthBonus = 500 - 300 * Mathf.FloorToInt(FindObjectOfType<Inventory>().tallyGold() / 1000f);
        artifacts.UpdateUI();
    }
}
