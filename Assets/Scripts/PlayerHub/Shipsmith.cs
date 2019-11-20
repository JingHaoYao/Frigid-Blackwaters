using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shipsmith : MonoBehaviour {
    GameObject playerShip, spawnedIndicator;
    public GameObject examineIndicator, shipSmithDisplay;
    public string buildingID;
    public GameObject icon;
    public bool ignoreUnlockLevel = false;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
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
            if (shipSmithDisplay.activeSelf == true)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
                {
                    shipSmithDisplay.SetActive(false);
                    playerShip.GetComponent<PlayerScript>().shipRooted = false;
                    playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = false;
                }
            }
            else if(playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    shipSmithDisplay.SetActive(true);
                    playerShip.GetComponent<PlayerScript>().shipRooted = true;
                    playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = true;
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
