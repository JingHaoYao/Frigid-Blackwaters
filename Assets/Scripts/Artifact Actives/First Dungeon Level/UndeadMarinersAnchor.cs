using System.Collections;
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

    void AnchorAttack()
    {
        float angleToCursor = Mathf.Atan2(PlayerProperties.cursorPosition.y - PlayerProperties.playerShipPosition.y, PlayerProperties.cursorPosition.x - PlayerProperties.playerShipPosition.x) * Mathf.Rad2Deg;
        Instantiate(leviathanBlast, PlayerProperties.playerShipPosition, Quaternion.Euler(0, 0, angleToCursor + 180));
    }
    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= 2)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    AnchorAttack();
                    artifacts.numKills -= 2;
                    FindObjectOfType<AudioManager>().PlaySound("Leviathan Cannon Charge Up");
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    AnchorAttack();
                    artifacts.numKills -= 2;
                    FindObjectOfType<AudioManager>().PlaySound("Leviathan Cannon Charge Up");
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    AnchorAttack();
                    artifacts.numKills -= 2;
                    FindObjectOfType<AudioManager>().PlaySound("Leviathan Cannon Charge Up");
                }
            }
        }
    }
}
