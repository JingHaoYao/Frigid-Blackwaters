using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireyHarness : MonoBehaviour
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject harnessFireball, summonEffect;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void spawnFireBalls()
    {
        GetComponent<AudioSource>().Play();
        GameObject effect = Instantiate(summonEffect, playerScript.transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().sortingOrder = playerScript.GetComponent<SpriteRenderer>().sortingOrder + 10;
        for(int i = 0; i < 2; i++)
        {
            GameObject fireball = Instantiate(harnessFireball, playerScript.transform.position + new Vector3(Mathf.Cos((i * 180) * Mathf.Deg2Rad), 0, 0) * 0.5f, Quaternion.identity);
            fireball.GetComponent<FireyHarnessFireball>().angleTravel = i * 180;
        }
    }

    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= 6)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    spawnFireBalls();
                    artifacts.numKills -= 6;
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    spawnFireBalls();
                    artifacts.numKills -= 6;
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    spawnFireBalls();
                    artifacts.numKills -= 6;
                }
            }
        }
    }
}
