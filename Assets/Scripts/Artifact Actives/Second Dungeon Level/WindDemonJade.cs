using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindDemonJade : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    ArtifactBonus artifactBonus;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }

    public override void addedKill(string tag)
    {
    }
    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {

    }
    // Whenever the player fires the left weapon, and so on
    public override void firedLeftWeapon(GameObject[] bullet)
    {

    }
    public override void firedFrontWeapon(GameObject[] bullet)
    {

    }
    public override void firedRightWeapon(GameObject[] bullet)
    {

    }
    // Whenever the player enters a previously unentered room
    public override void exploredNewRoom(int whatRoomType)
    {
        artifactBonus.speedBonus += 0.2f;
        artifacts.UpdateUI();
    }
    // Whenever the player picks up an item (updates the inventory)
    public override void updatedInventory()
    {
    }
    // whenever the player dashes
    public override void playerDashed()
    {
    }

    public override void dealtDamage(int damageDealt)
    {
    }
}
