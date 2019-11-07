﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeviathanCannon : MonoBehaviour {
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject leftWeapon, frontWeapon, rightWeapon;
    bool leviathanLoaded = false;
    public GameObject leviathanBlast;
    GameObject leftFire, rightFire, frontFire;
    public GameObject lightningEffect;
    GameObject lightningEff;

    void Start () {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        leftWeapon = GameObject.Find("LeftWeapon");
        rightWeapon = GameObject.Find("RightWeapon");
        frontWeapon = GameObject.Find("FrontWeapon");
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }
	
	void LateUpdate () {
        if (displayItem.isEquipped == true && playerScript.activeEnabled == false && artifacts.numKills >= 4)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    leviathanLoaded = true;
                    FindObjectOfType<AudioManager>().PlaySound("Leviathan Cannon Charge Up");
                    artifacts.numKills -= 4;
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    leviathanLoaded = true;
                    FindObjectOfType<AudioManager>().PlaySound("Leviathan Cannon Charge Up");
                    artifacts.numKills -= 4;
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    leviathanLoaded = true;
                    FindObjectOfType<AudioManager>().PlaySound("Leviathan Cannon Charge Up");
                    artifacts.numKills -= 4;
                }
            }
        }

        if(leviathanLoaded == true)
        {
            if (playerScript.activeEnabled == false)
            {
                leftFire = leftWeapon.GetComponent<ShipWeaponScript>().weaponPlume;
                rightFire = rightWeapon.GetComponent<ShipWeaponScript>().weaponPlume;
                frontFire = frontWeapon.GetComponent<ShipWeaponScript>().weaponPlume;
                playerScript.activeEnabled = true;
                lightningEff = Instantiate(lightningEffect, playerScript.gameObject.transform.position, Quaternion.identity);
            }

            /*if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.frontWeapon)))
            {
                frontWeapon.GetComponent<ShipWeaponScript>().weaponPlume = leviathanBlast;
                leviathanLoaded = false;
            }
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.leftWeapon)))
            {
                leftWeapon.GetComponent<ShipWeaponScript>().weaponPlume = leviathanBlast;
                leviathanLoaded = false;
            }
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.rightWeapon)))
            {
                rightWeapon.GetComponent<ShipWeaponScript>().weaponPlume = leviathanBlast;
                leviathanLoaded = false;
            }*/

            leftWeapon.GetComponent<ShipWeaponScript>().weaponPlume = leviathanBlast;
            rightWeapon.GetComponent<ShipWeaponScript>().weaponPlume = leviathanBlast;
            frontWeapon.GetComponent<ShipWeaponScript>().weaponPlume = leviathanBlast;
            if(Input.GetMouseButtonDown(0) && GameObject.Find("Leviathan Blast(Clone)"))
            {
                leviathanLoaded = false;
                playerScript.activeEnabled = false;
                Destroy(lightningEff);
                leftWeapon.GetComponent<ShipWeaponScript>().weaponPlume = leftFire;
                rightWeapon.GetComponent<ShipWeaponScript>().weaponPlume = rightFire;
                frontWeapon.GetComponent<ShipWeaponScript>().weaponPlume = frontFire;
            }
        }
    }
}
