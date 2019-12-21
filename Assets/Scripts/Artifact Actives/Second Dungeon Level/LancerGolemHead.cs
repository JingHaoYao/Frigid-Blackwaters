using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerGolemHead : ArtifactBonus
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject golemMinion;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void summonGolemMinion()
    {
        artifacts.numKills -= killRequirement;
        float angle = Random.Range(0, 2 * Mathf.PI);
        Instantiate(golemMinion, playerScript.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 2f, Quaternion.identity);
    }

    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= killRequirement && playerScript.enemiesDefeated == false)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    summonGolemMinion();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    summonGolemMinion();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    summonGolemMinion();
                }
            }
        }
    }
}
