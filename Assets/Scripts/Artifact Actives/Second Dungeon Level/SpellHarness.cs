using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHarness : MonoBehaviour
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    public GameObject pool;
    List<GameObject> spawnedPools = new List<GameObject>();
    float placingPools = 0;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    bool spawnPool()
    {
        foreach(GameObject pool in spawnedPools)
        {
            if(pool == null)
            {
                spawnedPools.Remove(pool);
                continue;
            }

            if(Vector2.Distance(playerScript.transform.position, pool.transform.position) > 0.8f)
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    void Update()
    {
        if (displayItem.isEquipped == true && artifacts.numKills >= 3)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    if (placingPools <= 0)
                    {
                        artifacts.numKills -= 3;
                        placingPools = 4;
                        FindObjectOfType<DurationUI>().addTile(displayItem.displayIcon, 4);
                    }
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    if (placingPools <= 0)
                    {
                        artifacts.numKills -= 3;
                        placingPools = 4;
                        FindObjectOfType<DurationUI>().addTile(displayItem.displayIcon, 4);
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    if (placingPools <= 0)
                    {
                        artifacts.numKills -= 3;
                        placingPools = 4;
                        FindObjectOfType<DurationUI>().addTile(displayItem.displayIcon, 4);
                    }
                }
            }
        }

        if (placingPools > 0 && displayItem.isEquipped == true)
        {
            placingPools -= Time.deltaTime;
            if (spawnPool() == true)
            {
                GameObject poolInstant = Instantiate(pool, playerScript.transform.position, Quaternion.identity);
                spawnedPools.Add(poolInstant);
            }

            if(placingPools <= 0)
            {
                spawnedPools.Clear();
            }
        }
    }
}
