using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringOfEtherealFlame : ArtifactBonus
{
    [SerializeField] GameObject flame;
    [SerializeField] DisplayItem displayItem;
    List<GameObject> spawnedFlames = new List<GameObject>();
    float placingPools = 0;

    bool spawnPool()
    {
        foreach (GameObject pool in spawnedFlames)
        {

            if (Vector2.Distance(PlayerProperties.playerShipPosition, pool.transform.position) > 0.5f)
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
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    if (placingPools <= 0)
                    {
                        StartCoroutine(spawnPools());
                    }
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    if (placingPools <= 0)
                    {
                        StartCoroutine(spawnPools());
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    if (placingPools <= 0)
                    {
                        StartCoroutine(spawnPools());
                    }
                }
            }
        }
    }

    public void removeFlame(GameObject gameObject)
    {
        this.spawnedFlames.Remove(gameObject);
    }

    IEnumerator spawnPools()
    {
        placingPools = 5;
        PlayerProperties.durationUI.addTile(displayItem.displayIcon, 5);
        while(placingPools > 0 && displayItem.isEquipped == true)
        {
            placingPools -= Time.deltaTime;
            if (spawnPool() == true)
            {
                GameObject poolInstant = Instantiate(flame, PlayerProperties.playerShipPosition, Quaternion.identity);
                poolInstant.GetComponent<EtherealFlame>().artifact = this;
                spawnedFlames.Add(poolInstant);
            }

            yield return null;
        }

        spawnedFlames.Clear();
    }

}
