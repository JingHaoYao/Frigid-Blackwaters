using System.Collections;
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

    void GolemAttack()
    {
        float angleToCursor = Mathf.Atan2(PlayerProperties.cursorPosition.y - PlayerProperties.playerShipPosition.y, PlayerProperties.cursorPosition.x - PlayerProperties.playerShipPosition.x) * Mathf.Rad2Deg;
        Instantiate(leviathanBlast, PlayerProperties.playerShipPosition, Quaternion.Euler(0, 0, angleToCursor));
    }

    void LateUpdate()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= 4)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    GolemAttack();
                    artifacts.numKills -= 4;
                    FindObjectOfType<AudioManager>().PlaySound("Golem Head Charge");
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    GolemAttack();
                    artifacts.numKills -= 4;
                    FindObjectOfType<AudioManager>().PlaySound("Golem Head Charge");
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    GolemAttack();
                    artifacts.numKills -= 4;
                    FindObjectOfType<AudioManager>().PlaySound("Golem Head Charge");
                }
            }
        }
    }
}
