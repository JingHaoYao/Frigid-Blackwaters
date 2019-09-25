using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRoom : MonoBehaviour {

    public GameObject goldChest;
    public GameObject[] obstacleList;

    void spawnRooms()
    {
        GameObject spawnedOb;
        spawnedOb = Instantiate(obstacleList[Random.Range(0, obstacleList.Length)], Camera.main.transform.position + new Vector3(-4f, -5f, 0), Quaternion.identity);
        if(Random.Range(0,2) == 1)
        {
            float x = spawnedOb.transform.localScale.x;
            float y = spawnedOb.transform.localScale.y;
            spawnedOb.transform.localScale = new Vector3(-x, y, 0);
        }
        spawnedOb = Instantiate(obstacleList[Random.Range(0, obstacleList.Length)], Camera.main.transform.position + new Vector3(4f, -5f, 0), Quaternion.identity);
        if (Random.Range(0, 2) == 1)
        {
            float x = spawnedOb.transform.localScale.x;
            float y = spawnedOb.transform.localScale.y;
            spawnedOb.transform.localScale = new Vector3(-x, y, 0);
        }
        spawnedOb = Instantiate(obstacleList[Random.Range(0, obstacleList.Length)], Camera.main.transform.position + new Vector3(4f, 4f, 0), Quaternion.identity);
        if (Random.Range(0, 2) == 1)
        {
            float x = spawnedOb.transform.localScale.x;
            float y = spawnedOb.transform.localScale.y;
            spawnedOb.transform.localScale = new Vector3(-x, y, 0);
        }
        spawnedOb = Instantiate(obstacleList[Random.Range(0, obstacleList.Length)], Camera.main.transform.position + new Vector3(-4f, 4f, 0), Quaternion.identity);
        if (Random.Range(0, 2) == 1)
        {
            float x = spawnedOb.transform.localScale.x;
            float y = spawnedOb.transform.localScale.y;
            spawnedOb.transform.localScale = new Vector3(-x, y, 0);
        }

        for (int i = 0; i < 3; i++)
        {
            Vector3 newPos = new Vector3(Random.Range(transform.position.x - 8, transform.position.x + 8), Random.Range(transform.position.y - 8, transform.position.y + 8), 0);
            while(Physics2D.OverlapCircle(newPos, 1) == true)
            {
                newPos = new Vector3(Random.Range(transform.position.x - 8, transform.position.x + 8), Random.Range(transform.position.y - 8, transform.position.y + 8), 0);
            }
            Instantiate(goldChest, newPos, Quaternion.identity);
        }
    }

	void Start () {
        spawnRooms();
	}

	void Update () {
		
	}
}
