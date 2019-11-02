using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShackledWeight : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    float speedDecreasePeriod = 0;
    ArtifactBonus artifactBonus;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }

    void Update()
    {
        if(speedDecreasePeriod > 0)
        {
            speedDecreasePeriod -= Time.deltaTime;
            if(artifactBonus.speedBonus != -2)
            {
                artifactBonus.speedBonus = -2;
                artifacts.UpdateUI();
            }
        }
        else
        {
            if(artifactBonus.speedBonus != 0)
            {
                artifactBonus.speedBonus = 0;
                artifacts.UpdateUI();
            }
        }
    }

    public override void addedKill(string tag, Vector3 deathPos)
    {
    }
    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
    }
    // Whenever the player fires the left weapon, and so on
    public override void firedLeftWeapon(GameObject[] bullet)
    {
        speedDecreasePeriod = 0.5f;
    }
    public override void firedFrontWeapon(GameObject[] bullet)
    {
        speedDecreasePeriod = 0.5f;
    }
    public override void firedRightWeapon(GameObject[] bullet)
    {
        speedDecreasePeriod = 0.5f;
    }
    // Whenever the player enters a previously unentered room
    public override void exploredNewRoom(int whatRoomType) { }
    // Whenever the player picks up an item (updates the inventory)
    public override void updatedInventory()
    {
    }
    // whenever the player dashes
    public override void playerDashed()
    {
    }

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
    }
}
