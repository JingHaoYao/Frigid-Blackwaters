using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiracleVines : MonoBehaviour
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    bool alreadyHealing = false;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    IEnumerator startHeal()
    {
        alreadyHealing = true;
        for(int i = 0; i < 12; i++)
        {
            playerScript.healPlayer(25);
            yield return new WaitForSeconds(0.5f);
        }
        alreadyHealing = false;
    }

    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= 4)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    artifacts.numKills -= 4;
                    if(alreadyHealing == false)
                    {
                        StartCoroutine(startHeal());
                    }
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    artifacts.numKills -= 4;
                    if (alreadyHealing == false)
                    {
                        StartCoroutine(startHeal());
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    artifacts.numKills -= 4;
                    if (alreadyHealing == false)
                    {
                        StartCoroutine(startHeal());
                    }
                }
            }
        }
    }
}
