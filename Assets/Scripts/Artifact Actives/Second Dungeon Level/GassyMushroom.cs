using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GassyMushroom : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject gasCloud;


    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }


    public override void addedKill(string tag, Vector3 deathPos)
    {
    }
    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
    }
    // Whenever the player fires the left weapon, and so on
    public override void firedLeftWeapon(GameObject[] bullet) { }
    public override void firedFrontWeapon(GameObject[] bullet) { }
    public override void firedRightWeapon(GameObject[] bullet) { }
    // Whenever the player enters a previously unentered room
    public override void exploredNewRoom(int whatRoomType) { }
    // Whenever the player picks up an item (updates the inventory)
    public override void updatedInventory()
    {
    }
    // whenever the player dashes
    public override void playerDashed() {
        GameObject gas = Instantiate(gasCloud, playerScript.transform.position, Quaternion.Euler(0, 0, playerScript.angleEffect + 90));
    }

    public override void dealtDamage(int damageDealt)
    {
    }
}
