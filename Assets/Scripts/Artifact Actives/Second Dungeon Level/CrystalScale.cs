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

    public override void addedKill(string tag, Vector3 deathPos) {
    }
    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy) { }
    // Whenever the player fires the left weapon, and so on
    public override void firedLeftWeapon(GameObject[] bullet) { }
    public override void firedFrontWeapon(GameObject[] bullet) { }
    public override void firedRightWeapon(GameObject[] bullet) { }
    // Whenever the player enters a previously unentered room
    public override void exploredNewRoom(int whatRoomType) { }
    // Whenever the player picks up an item (updates the inventory)
    public override void updatedInventory()
    {
        artifactBonus.speedBonus = -0.1f * Mathf.RoundToInt(inventory.tallyGold() / 100f);
        artifactBonus.healthBonus = 75 * Mathf.RoundToInt(inventory.tallyGold() / 100f);
        artifacts.UpdateUI();
    }
    // whenever the player dashes
    public override void playerDashed() { }

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
    }
}
