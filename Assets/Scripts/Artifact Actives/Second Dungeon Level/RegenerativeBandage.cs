﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerativeBandage : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    float timeToHealPeriod = 0;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void Update()
    {
        if(timeToHealPeriod > 0)
        {
            timeToHealPeriod -= Time.deltaTime;
            if(timeToHealPeriod <= 0)
            {
                playerScript.healPlayer(150);
                timeToHealPeriod = 0;
            }
        }
    }



    public override void addedKill(string tag, Vector3 deathPos)
    {
    }
    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if (timeToHealPeriod <= 0)
        {
            timeToHealPeriod = 3;
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
    public override void playerDashed()
    {
    }

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
    }
}
