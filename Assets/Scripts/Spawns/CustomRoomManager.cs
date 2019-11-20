using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRoomManager : MonoBehaviour
{
    bool roomInit = false;
    public int whatRoomType = 0;
    PlayerScript playerScript;
    public GameObject roomReveal;
    public GameObject doorwaySeal;
    GameObject[] doorSeals = new GameObject[4];
    public GameObject[] enemiesToSpawn;
    public bool leftOpening, rightOpening, topOpening, bottomOpening;
    GameObject spawnedGridMap;
    public GameObject AStarGrid;
    bool spawningComplete = false;

    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
    }

    public void spawnDoorSeals()
    {
        if (leftOpening)
        {
            doorSeals[0] = Instantiate(doorwaySeal, transform.parent.transform.position + new Vector3(-10.5f, 0, 0), Quaternion.identity);
        }
        if (rightOpening)
        {
            doorSeals[1] = Instantiate(doorwaySeal, transform.parent.transform.position + new Vector3(10.5f, 0, 0), Quaternion.Euler(0, 0, 180));
        }
        if (topOpening)
        {
            doorSeals[2] = Instantiate(doorwaySeal, transform.parent.transform.position + new Vector3(0, 10.5f, 0), Quaternion.Euler(0, 0, 270));
        }
        if (bottomOpening)
        {
            doorSeals[3] = Instantiate(doorwaySeal, transform.parent.transform.position + new Vector3(0, -10.5f, 0), Quaternion.Euler(0, 0, 90));
        }
    }

    Vector3 pickRandEnemySpawn()
    {
        float xSpawn = Mathf.RoundToInt(Random.Range(Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7)) - 0.5f + Random.Range(0, 2);
        float ySpawn = Mathf.RoundToInt(Random.Range(Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7)) - 0.5f + Random.Range(0, 2);
        Vector3 test = new Vector3(xSpawn, ySpawn, 0);
        while (Physics2D.OverlapCircle(test, 0.5f) == true)
        {
            xSpawn = Mathf.RoundToInt(Random.Range(Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7)) - 0.5f + Random.Range(0, 2);
            ySpawn = Mathf.RoundToInt(Random.Range(Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7)) - 0.5f + Random.Range(0, 2);
            test = new Vector3(xSpawn, ySpawn, 0);
        }
        return test;
    }

    void spawnEnemies()
    {
        foreach(GameObject enemy in enemiesToSpawn)
        {
            Instantiate(enemy, pickRandEnemySpawn(), Quaternion.identity);
        }
        spawningComplete = true;
    }

    void openDoorSeals()
    {
        for (int i = 0; i < 4; i++)
        {
            if (doorSeals[i] != null)
            {
                doorSeals[i].GetComponent<DoorSeal>().open = true;
            }
        }
        if (spawnedGridMap)
        {
            Destroy(spawnedGridMap);
        }
    }

    void Update()
    {
        if (Mathf.Sqrt(Mathf.Pow(Camera.main.transform.position.y - transform.parent.transform.position.y, 2) + Mathf.Pow(Camera.main.transform.position.x - transform.parent.transform.position.x, 2)) < 0.5f) {
            if (roomInit == false)
            {
                playerScript.numRoomsSinceLastArtifact++;
                playerScript.numRoomsVisited++;
                Instantiate(roomReveal, transform.position, Quaternion.identity);

                foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
                {
                    if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    {
                        slot.displayInfo.GetComponent<ArtifactEffect>().exploredNewRoom(2);
                    }
                }

                if (whatRoomType == 2)
                {
                    spawnedGridMap = Instantiate(AStarGrid, transform.parent.position, Quaternion.identity);
                    Invoke("spawnDoorSeals", 0.5f);
                    Invoke("spawnEnemies", 0.18f);
                    playerScript.enemiesDefeated = false;
                }

                roomInit = true;
            }
            else
            {
                if(FindObjectsOfType<Enemy>().Length <= 0 && spawningComplete == true)
                {
                    openDoorSeals();
                    playerScript.enemiesDefeated = true;
                }
            }
        }
        
    }
}
