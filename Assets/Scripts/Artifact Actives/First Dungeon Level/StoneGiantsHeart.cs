using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGiantsHeart : MonoBehaviour {
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    GameObject playerShip;
    public GameObject fist;

    IEnumerator spawnFist()
    {
        playerScript.activeEnabled = true;
        Instantiate(fist, playerShip.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.4f + 2f/12f);
        playerScript.activeEnabled = false;
    }

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        playerShip = GameObject.Find("PlayerShip");
    }

    void Update()
    {
        if (displayItem.isEquipped == true && playerScript.activeEnabled == false && artifacts.numKills >= 6)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    StartCoroutine(spawnFist());
                    artifacts.numKills -= 6;
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    StartCoroutine(spawnFist());
                    artifacts.numKills -= 6;
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    StartCoroutine(spawnFist());
                    artifacts.numKills -= 6;
                }
            }
        }
    }
}
