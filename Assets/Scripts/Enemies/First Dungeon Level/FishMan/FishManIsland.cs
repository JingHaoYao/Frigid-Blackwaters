using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManIsland : Enemy {
    public int whatCornerSpawned = 0;
    public GameObject fishMan;
    public int numFishMan = 3;
    List<GameObject> spawnedFishmen = new List<GameObject>();

    void spawnFishMen()
    {
        GameObject spawnedFishMan;
        if(whatCornerSpawned == 1)
        {
            for(int i = 0; i < numFishMan; i++)
            {
                spawnedFishMan = Instantiate(fishMan, transform.position + new Vector3(Mathf.Cos((0 - 45*i)*Mathf.Deg2Rad), Mathf.Sin((0 - 45 * i) * Mathf.Deg2Rad), 0) * 3, Quaternion.identity);
                spawnedFishMan.GetComponent<FishManEnemy>().fishManIsland = this.gameObject;
                spawnedFishmen.Add(spawnedFishMan);
            }
        }
        else if(whatCornerSpawned == 2)
        {
            for(int i = 0; i < numFishMan; i++)
            {
                spawnedFishMan = Instantiate(fishMan, transform.position + new Vector3(Mathf.Cos((0 + 45 * i) * Mathf.Deg2Rad), Mathf.Sin((0 + 45 * i) * Mathf.Deg2Rad), 0) * 3, Quaternion.identity);
                spawnedFishMan.GetComponent<FishManEnemy>().fishManIsland = this.gameObject;
                spawnedFishmen.Add(spawnedFishMan);
            }
        }
        else if(whatCornerSpawned == 3)
        {
            for (int i = 0; i < numFishMan; i++)
            {
                spawnedFishMan = Instantiate(fishMan, transform.position + new Vector3(Mathf.Cos((180 + 45 * i) * Mathf.Deg2Rad), Mathf.Sin((180 + 45 * i) * Mathf.Deg2Rad), 0) * 3, Quaternion.identity);
                spawnedFishMan.GetComponent<FishManEnemy>().fishManIsland = this.gameObject;
                spawnedFishmen.Add(spawnedFishMan);
            }
        }
        else
        {
            for (int i = 0; i < numFishMan; i++)
            {
                spawnedFishMan = Instantiate(fishMan, transform.position + new Vector3(Mathf.Cos((90 + 45 * i) * Mathf.Deg2Rad), Mathf.Sin((90 + 45 * i) * Mathf.Deg2Rad), 0) * 3, Quaternion.identity);
                spawnedFishMan.GetComponent<FishManEnemy>().fishManIsland = this.gameObject;
                spawnedFishmen.Add(spawnedFishMan);
            }
        }
    }

	void Start () {
        int whichCorner = Random.Range(1, 5);
        Vector3 spawnLocation;

        if (whichCorner == 1)
        {
            spawnLocation = new Vector3(-5, 5);
        }
        else if (whichCorner == 2)
        {
            spawnLocation = new Vector3(-5, -5);
        }
        else if (whichCorner == 3)
        {
            spawnLocation = new Vector3(5, 5);
        }
        else
        {
            spawnLocation = new Vector3(5, -5);
        }
        spawnLocation += Camera.main.transform.position;
        transform.position = spawnLocation;
        whatCornerSpawned = whichCorner;

        spawnFishMen();
	}

	void Update () {
	    if(checkfishMan() == false)
        {
            Destroy(this);
        }
	}

    bool checkfishMan()
    {
        foreach(GameObject fishman in spawnedFishmen)
        {
            if(fishman != null)
            {
                return true;
            }
        }
        return false;
    }
}
