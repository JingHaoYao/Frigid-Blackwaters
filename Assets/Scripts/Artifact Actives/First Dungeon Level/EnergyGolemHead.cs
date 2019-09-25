﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGolemHead : MonoBehaviour {
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject leftWeapon, frontWeapon, rightWeapon;
    bool leviathanLoaded = false;
    public GameObject leviathanBlast;
    GameObject leftFire, rightFire, frontFire;
    public GameObject energyGolemPulse;
    GameObject instant;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        leftWeapon = GameObject.Find("LeftWeapon");
        rightWeapon = GameObject.Find("RightWeapon");
        frontWeapon = GameObject.Find("FrontWeapon");
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void LateUpdate()
    {
        if (displayItem.isEquipped == true && playerScript.activeEnabled == false && artifacts.numKills >= 4)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    leviathanLoaded = true;
                    artifacts.numKills -= 4;
                    FindObjectOfType<AudioManager>().PlaySound("Golem Head Charge");
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    leviathanLoaded = true;
                    artifacts.numKills -= 4;
                    FindObjectOfType<AudioManager>().PlaySound("Golem Head Charge");
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    leviathanLoaded = true;
                    artifacts.numKills -= 4;
                    FindObjectOfType<AudioManager>().PlaySound("Golem Head Charge");
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
                instant = Instantiate(energyGolemPulse, GameObject.Find("PlayerShip").transform.position, Quaternion.identity);
            }

            /*if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.frontWeapon)))
            {
                frontWeapon.GetComponent<ShipWeaponScript>().musketSmoke = leviathanBlast;
                leviathanLoaded = false;
            }
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.leftWeapon)))
            {
                leftWeapon.GetComponent<ShipWeaponScript>().musketSmoke = leviathanBlast;
                leviathanLoaded = false;
            }
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.rightWeapon)))
            {
                rightWeapon.GetComponent<ShipWeaponScript>().musketSmoke = leviathanBlast;
                leviathanLoaded = false;
            }*/

            leftWeapon.GetComponent<ShipWeaponScript>().musketSmoke = leviathanBlast;
            rightWeapon.GetComponent<ShipWeaponScript>().musketSmoke = leviathanBlast;
            frontWeapon.GetComponent<ShipWeaponScript>().musketSmoke = leviathanBlast;
            if (Input.GetMouseButtonDown(0) && GameObject.Find("Golem Head Laser Circle(Clone)"))
            {
                leviathanLoaded = false;
                playerScript.activeEnabled = false;
                leftWeapon.GetComponent<ShipWeaponScript>().musketSmoke = leftFire;
                rightWeapon.GetComponent<ShipWeaponScript>().musketSmoke = rightFire;
                frontWeapon.GetComponent<ShipWeaponScript>().musketSmoke = frontFire;
                Destroy(instant);
            }
        }
    }
}
