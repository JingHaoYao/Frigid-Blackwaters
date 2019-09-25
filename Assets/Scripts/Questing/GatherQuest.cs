using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherQuest : QuestType
{
    PlayerScript playerScript;

    public bool uniqueItems = false;
    public List<string> itemFilter;
    public GameObject toGatherContainer;

    public int numberOfRoomsBeforeSpawn;
    public int percentageSpawnQuestItem;
    public int numberItemsSpawn;
    int currNumberItems = 0;
    int currNumberOfRooms;

    public void progressQuest(List<GameObject> itemList)
    {
        if (uniqueItems)
        {
            List<string> prevItemNames = new List<string>();
            int count = 0;
            foreach (GameObject item in itemList)
            {
                if (itemFilter.Contains(item.name) && !prevItemNames.Contains(item.name))
                {
                    prevItemNames.Add(item.name);
                    count++;
                }
            }
            currentAmount = count;
            Evaluate();
        }
        else
        {
            int count = 0;
            foreach(GameObject item in itemList)
            {
                if (item.name == itemFilter[0])
                {
                    count++;
                }
            }
            currentAmount = count;
            Evaluate();
        }
    }

    private void Start()
    {
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        currNumberOfRooms = playerScript.numRoomsVisited;
    }

    private void Update()
    {
        if(GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>().antiList.Count < playerScript.numRoomsVisited + 3)
        {
            percentageSpawnQuestItem = 110;
        }

        if(playerScript.numRoomsVisited != currNumberOfRooms)
        {
            currNumberOfRooms = playerScript.numRoomsVisited;
            if(playerScript.numRoomsVisited >= numberOfRoomsBeforeSpawn)
            {
                if (Random.Range(1, 101) <= percentageSpawnQuestItem && currNumberItems < numberItemsSpawn)
                {
                    currNumberItems++;
                    Vector3 spawnPos = new Vector3(Random.Range(Camera.main.transform.position.x - 8, Camera.main.transform.position.x + 8), Random.Range(Camera.main.transform.position.y - 8, Camera.main.transform.position.y + 8), 0);
                    while (Physics2D.OverlapCircle(spawnPos, 0.5f) == true)
                    {
                        spawnPos = new Vector3(Random.Range(Camera.main.transform.position.x - 8, Camera.main.transform.position.x + 8), Random.Range(Camera.main.transform.position.y - 8, Camera.main.transform.position.y + 8), 0);
                    }
                    Instantiate(toGatherContainer, spawnPos, Quaternion.identity);
                }
            }
        }
    }
}
