using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solarity : MonoBehaviour {
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    GameObject playerShip;
    public GameObject sunshineParticles, flare, sunshineExplosion;
    bool solarityActive = false;
    private float solarityTimer = 0, particleTimer = 0;
    int numHitsShip = 0;
    bool set1 = false, set2 = false;

    void Start () {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        playerShip = GameObject.Find("PlayerShip");

    }
	
	void Update () {
        if (displayItem.isEquipped == true && playerScript.activeEnabled == false && artifacts.numKills >= 20)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    solarityActive = true;
                    artifacts.numKills -= 20;
                    solarityTimer = 0;
                    playerScript.activeEnabled = true;
                    playerScript.damageImmunity = true;
                    playerScript.damageAbsorb = true;
                    numHitsShip = playerScript.numberHits;
                    set1 = false;
                    set2 = false;
                    FindObjectOfType<DurationUI>().addTile(this.GetComponent<DisplayItem>().displayIcon, 10);
                    FindObjectOfType<AudioManager>().PlaySound("Solarity Chimes");
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    solarityActive = true;
                    artifacts.numKills -= 20;
                    solarityTimer = 0;
                    playerScript.activeEnabled = true;
                    playerScript.damageImmunity = true;
                    playerScript.damageAbsorb = true;
                    numHitsShip = playerScript.numberHits;
                    set1 = false;
                    set2 = false;
                    FindObjectOfType<DurationUI>().addTile(this.GetComponent<DisplayItem>().displayIcon, 10);
                    FindObjectOfType<AudioManager>().PlaySound("Solarity Chimes");
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    solarityActive = true;
                    artifacts.numKills -= 20;
                    solarityTimer = 0;
                    playerScript.activeEnabled = true;
                    playerScript.damageImmunity = true;
                    playerScript.damageAbsorb = true;
                    numHitsShip = playerScript.numberHits;
                    set1 = false;
                    set2 = false;
                    FindObjectOfType<DurationUI>().addTile(this.GetComponent<DisplayItem>().displayIcon, 10);
                    FindObjectOfType<AudioManager>().PlaySound("Solarity Chimes");
                }
            }
        }

        if (solarityActive == true)
        {
            solarityTimer += Time.deltaTime;
            particleTimer += Time.deltaTime;

            if(playerScript.numberHits != numHitsShip)
            {
                numHitsShip = playerScript.numberHits;
                Instantiate(sunshineExplosion, playerShip.transform.position, Quaternion.identity);
                FindObjectOfType<AudioManager>().PlaySound("Solarity Absorb Chime");
            }

            if(particleTimer >= 0.05 && solarityTimer < 10)
            {
                Instantiate(sunshineParticles, playerShip.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0), Quaternion.identity);
                Instantiate(flare, playerShip.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0), Quaternion.identity);
                particleTimer = 0;
            }

            if(solarityTimer >= 10 && set1 == false)
            {
                playerScript.damageImmunity = false;
                playerScript.damageAbsorb = false;
                playerScript.boatSpeed -= 3;
                FindObjectOfType<AudioManager>().StopSound("Solarity Chimes");
                FindObjectOfType<AudioManager>().PlaySound("Solarity Speed Down");
                set1 = true;
            }

            if (solarityTimer >= 15 && set2 == false)
            {
                playerScript.activeEnabled = false;
                solarityActive = false;
                playerScript.boatSpeed += 3;
                set2 = true;
            }
        }
    }
}
