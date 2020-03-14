using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScale : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    Inventory inventory;
    ArtifactBonus artifactBonus;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        inventory = FindObjectOfType<Inventory>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }

    public override void updatedInventory()
    {
        artifactBonus.speedBonus = -0.1f * Mathf.RoundToInt(inventory.tallyGold() / 100f);
        artifactBonus.healthBonus = 75 * Mathf.RoundToInt(inventory.tallyGold() / 100f);
        artifacts.UpdateUI();
    }
}
