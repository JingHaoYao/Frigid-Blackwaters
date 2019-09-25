using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpentsFin : MonoBehaviour {
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    GameObject spawnedRunicFire, playerShip;
    public GameObject wave1, wave2, wave3, wave4, wave5;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        playerShip = GameObject.Find("PlayerShip");
    }

    void summonWaves()
    {
        GameObject instant = Instantiate(wave1, playerShip.transform.position, Quaternion.identity);
        instant.GetComponent<SerpentFinWave>().angleTravel = 270;
        instant = Instantiate(wave2, playerShip.transform.position, Quaternion.identity);
        instant.GetComponent<SerpentFinWave>().angleTravel = 225;
        instant = Instantiate(wave2, playerShip.transform.position, Quaternion.identity);
        instant.GetComponent<SerpentFinWave>().angleTravel = 315;
        instant.transform.localScale = new Vector3(-0.3f, 0.3f, 0);
        instant = Instantiate(wave3, playerShip.transform.position, Quaternion.identity);
        instant.GetComponent<SerpentFinWave>().angleTravel = 180;
        instant = Instantiate(wave3, playerShip.transform.position, Quaternion.identity);
        instant.GetComponent<SerpentFinWave>().angleTravel = 0;
        instant.transform.localScale = new Vector3(-0.35f, 0.35f, 0);
        instant = Instantiate(wave4, playerShip.transform.position, Quaternion.identity);
        instant.GetComponent<SerpentFinWave>().angleTravel = 90;
        instant = Instantiate(wave5, playerShip.transform.position, Quaternion.identity);
        instant.GetComponent<SerpentFinWave>().angleTravel = 135;
        instant = Instantiate(wave5, playerShip.transform.position, Quaternion.identity);
        instant.GetComponent<SerpentFinWave>().angleTravel = 45;
        instant.transform.localScale = new Vector3(-0.3f, 0.3f, 0);
    }
    
    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= 6)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    artifacts.numKills -= 6;
                    summonWaves();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    artifacts.numKills -= 6;
                    summonWaves();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    artifacts.numKills -= 6;
                    summonWaves();
                }
            }
        }
    }
}
