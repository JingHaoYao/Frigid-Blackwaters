﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadMarinersAnchor : MonoBehaviour
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject leftWeapon, frontWeapon, rightWeapon;
    bool leviathanLoaded = false;
    public GameObject leviathanBlast;
    GameObject leftFire, rightFire, frontFire;
    public GameObject lightningEffect;
    GameObject lightningEff;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        leftWeapon = GameObject.Find("LeftWeapon");
        rightWeapon = GameObject.Find("RightWeapon");
        frontWeapon = GameObject.Find("FrontWeapon");
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true && playerScript.activeEnabled == false && artifacts.numKills >= 2)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    leviathanLoaded = true;
                    artifacts.numKills -= 2;
                    FindObjectOfType<AudioManager>().PlaySound("Leviathan Cannon Charge Up");
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    leviathanLoaded = true;
                    artifacts.numKills -= 2;
                    FindObjectOfType<AudioManager>().PlaySound("Leviathan Cannon Charge Up");
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    leviathanLoaded = true;
                    artifacts.numKills -= 2;
                    FindObjectOfType<AudioManager>().PlaySound("Leviathan Cannon Charge Up");
                }
            }
        }

        if (leviathanLoaded == true)
        {
            if (playerScript.activeEnabled == false)
            {
                leftFire = leftWeapon.GetComponent<ShipWeaponScript>().musketSmoke;
                rightFire = rightWeapon.GetComponent<ShipWeaponScript>().musketSmoke;
                frontFire = frontWeapon.GetComponent<ShipWeaponScript>().musketSmoke;
                playerScript.activeEnabled = true;
                lightningEff = Instantiate(lightningEffect, playerScript.gameObject.transform.position, Quaternion.identity);
            }

            leftWeapon.GetComponent<ShipWeaponScript>().musketSmoke = leviathanBlast;
            rightWeapon.GetComponent<ShipWeaponScript>().musketSmoke = leviathanBlast;
            frontWeapon.GetComponent<ShipWeaponScript>().musketSmoke = leviathanBlast;
            if (Input.GetMouseButtonDown(0) && GameObject.Find("Anchor Blast(Clone)"))
            {
                leviathanLoaded = false;
                playerScript.activeEnabled = false;
                Destroy(lightningEff);
                leftWeapon.GetComponent<ShipWeaponScript>().musketSmoke = leftFire;
                rightWeapon.GetComponent<ShipWeaponScript>().musketSmoke = rightFire;
                frontWeapon.GetComponent<ShipWeaponScript>().musketSmoke = frontFire;
            }
        }
    }
}
