using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shipsmith : MonoBehaviour {
    GameObject playerShip, spawnedIndicator;
    public GameObject examineIndicator, shipSmithDisplay;
    public string buildingID;
    public GameObject icon;
    public bool ignoreUnlockLevel = false;

    MenuSlideAnimation menuSlideAnimation = new MenuSlideAnimation();

    void SetAnimation()
    {
        menuSlideAnimation.SetOpenAnimation(new Vector3(0, -585, 0), new Vector3(0, 0, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(0, 0, 0), new Vector3(0, -585, 0), 0.25f);
    }

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        SetAnimation();
    }

    void LateUpdate () {
        if (Vector2.Distance(playerShip.transform.position, transform.position) < 5f && (MiscData.unlockedBuildings.Contains(buildingID) || ignoreUnlockLevel) && playerShip.GetComponent<PlayerScript>().enemiesDefeated == true)
        {
            if (shipSmithDisplay.activeSelf == false)
            {
                if (spawnedIndicator == null)
                {
                    spawnedIndicator = Instantiate(examineIndicator, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                    spawnedIndicator.GetComponent<ExamineIndicator>().parentObject = this.gameObject;
                }
            }
            else
            {
                if (spawnedIndicator != null)
                {
                    Destroy(spawnedIndicator);
                }
            }

            if (menuSlideAnimation.IsAnimating == false)
            {
                if (shipSmithDisplay.activeSelf == true)
                {
                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
                    {
                        menuSlideAnimation.PlayEndingAnimation(shipSmithDisplay, () => { shipSmithDisplay.SetActive(false); });
                        PlayerProperties.tutorialWidgetMenu.CloseTutorial();
                        PlayerProperties.playerScript.removeRootingObject();
                        playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = false;
                    }
                }
                else if (playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        shipSmithDisplay.SetActive(true);
                        if(MiscData.firstTimeTutorialsPlayed.Contains(buildingID))
                        {
                            menuSlideAnimation.PlayOpeningAnimation(shipSmithDisplay);
                        }
                        else
                        {
                            if (buildingID == "weapon_outfitter")
                            {
                                menuSlideAnimation.PlayOpeningAnimation(shipSmithDisplay, () => { shipSmithDisplay.GetComponent<PickWeaponMenu>().ShowTutorial(); });
                            }
                            else if (buildingID == "shipsmith")
                            {
                                menuSlideAnimation.PlayOpeningAnimation(shipSmithDisplay, () => { shipSmithDisplay.GetComponent<ShipSmithMenus>().ShowTutorial(); });
                            }
                            MiscData.firstTimeTutorialsPlayed.Add(buildingID);
                        }
                        PlayerProperties.playerScript.addRootingObject();
                        playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = true;
                    }
                }
            }
        }
        else
        {
            if (spawnedIndicator != null)
            {
                Destroy(spawnedIndicator);
            }
        }

        if ((MiscData.unlockedBuildings.Contains(buildingID) || ignoreUnlockLevel) && playerShip.GetComponent<PlayerScript>().enemiesDefeated == true)
        {
            icon.SetActive(true);
        }
        else
        {
            icon.SetActive(false);
        }
    }
}
