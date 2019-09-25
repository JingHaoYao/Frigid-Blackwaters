using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdoloftheGoddess : MonoBehaviour
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    ArtifactBonus artifactBonus;
    public int origKills = 0;
    bool completedTrial = false;
    bool startedTrial = false;
    public int killRequirement = 1;
    bool setDamagedHealth = false;
    public GameObject indicator;
    GameObject spawnedIndicator;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true && playerScript.activeEnabled == false && completedTrial == false && startedTrial == false)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    startedTrial = true;
                    origKills = artifacts.numKills;
                    spawnedIndicator = Instantiate(indicator, playerScript.gameObject.transform.position + new Vector3(0, 2.4f, 0), Quaternion.identity);
                    spawnedIndicator.GetComponent<GoddessIndicator>().idolScript = this;
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    startedTrial = true;
                    origKills = artifacts.numKills;
                    spawnedIndicator = Instantiate(indicator, playerScript.gameObject.transform.position + new Vector3(0, 2.4f, 0), Quaternion.identity);
                    spawnedIndicator.GetComponent<GoddessIndicator>().idolScript = this;
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    startedTrial = true;
                    origKills = artifacts.numKills;
                    spawnedIndicator = Instantiate(indicator, playerScript.gameObject.transform.position + new Vector3(0, 2.4f, 0), Quaternion.identity);
                    spawnedIndicator.GetComponent<GoddessIndicator>().idolScript = this;

                }
            }
        }

        if(startedTrial == true && artifacts.numKills - origKills < killRequirement)
        {
            if (setDamagedHealth == false)
            {
                artifactBonus.healthBonus = -700;
                artifacts.UpdateUI();
                setDamagedHealth = true;
            }
        }
        else if(startedTrial == true && artifacts.numKills - origKills >= killRequirement)
        {
            completedTrial = true;
            startedTrial = false;
            artifactBonus.healthBonus = 1000;
            artifactBonus.attackBonus = 4;
            artifactBonus.speedBonus = 4;
            artifactBonus.defenseBonus = 0.35f;
            artifacts.UpdateUI();
            setDamagedHealth = false;
        }

        if (displayItem.isEquipped == false && completedTrial == false)
        {
            startedTrial = false;
            origKills = artifacts.numKills;
            artifactBonus.healthBonus = 0;
            setDamagedHealth = false;
            if(spawnedIndicator != null)
            {
                Destroy(spawnedIndicator);
            }
        }
    }
}
