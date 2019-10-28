using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyInABottle : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject healParticles;
    int numberUsesLeft = 2;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }


    public override void addedKill(string tag)
    {
    }
    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if ((float)playerScript.shipHealth / playerScript.shipHealthMAX < 0.2f && numberUsesLeft > 0)
        {
            playerScript.healPlayer(500);
            numberUsesLeft--;
            if (playerScript.trueDamage < 0)
            {
                playerScript.trueDamage = 0;
            }
            GameObject particles = Instantiate(healParticles, playerScript.transform.position, Quaternion.identity);
            particles.GetComponent<FollowObject>().objectToFollow = playerScript.gameObject;
            this.GetComponent<AudioSource>().Play();
        }
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
    public override void playerDashed() { }

    public override void dealtDamage(int damageDealt)
    {
    }
}
