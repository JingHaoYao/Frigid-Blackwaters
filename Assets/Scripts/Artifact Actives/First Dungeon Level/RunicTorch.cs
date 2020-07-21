using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunicTorch : MonoBehaviour {
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject runicFireCircle;
    GameObject spawnedRunicFire, playerShip;
    private bool spawnedFire = false;

    void Start () {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        playerShip = GameObject.Find("PlayerShip");
    }

    void summonFire()
    {
        spawnedFire = true;
        spawnedRunicFire = Instantiate(runicFireCircle, playerShip.transform.position, Quaternion.Euler(0,0,Random.Range(0,360)));
        if(Random.Range(0,2) == 1)
        {
            float xScale = spawnedRunicFire.transform.localScale.x;
            xScale = -xScale;
            spawnedRunicFire.transform.localScale = new Vector3(xScale, 1, 0);
        }
    }

	void Update () {
        if (displayItem.isEquipped == true && artifacts.numKills >= 3)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    summonFire();
                    artifacts.numKills -= 3;
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    summonFire();
                    artifacts.numKills -= 3;
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    summonFire();
                    artifacts.numKills -= 3;
                }
            }
        }

        if(spawnedFire == true && spawnedRunicFire == null)
        {
            spawnedFire = false;
        }
    }
}
