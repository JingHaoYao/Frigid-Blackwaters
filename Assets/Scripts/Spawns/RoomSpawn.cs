using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawn : MonoBehaviour
{
    public int openDirection;
    //1 --> top door
    //2 --> bottom door
    //3 --> right door
    //4 --> left door

    private RoomTemplates templates;
    private bool spawned = false;
    private bool dontSpawn = false;
    public bool spawnedDoorBlock = false;
    public float delayInvoke = 0.1f;
    public int collisionNumber = 0;

    private void Awake()
    {
        templates = FindObjectOfType<RoomTemplates>();
        Invoke("Spawn", delayInvoke);
    }

    private void Update()
    {
        if (templates.GetComponent<RoomTemplates>().spawnPeriod >= 6.2f)
        {
            if(collisionNumber > 1)
            {
                spawnDoorBlock();
            }
            Destroy(this.gameObject);
        }
    }

    private void Spawn()
    {
        if (templates.roomCount < templates.maxRoomCount)
        {
            if (spawned == false && dontSpawn == false)
            {
                templates.roomCount++;
                switch (openDirection)
                {
                    case 1:
                        //spawn door with top open
                        spawnRoom(templates.topOpenRooms);
                        break;
                    case 2:
                        //spawn door with bottom open
                        spawnRoom(templates.bottomOpenRooms);
                        break;
                    case 3:
                        //spawn door with right open
                        spawnRoom(templates.rightOpenRooms);
                        break;
                    case 4:
                        //spawn door with left open
                        spawnRoom(templates.leftOpenRooms);
                        break;
                }
                spawned = true;
            }
        }
        else
        {
            if (collisionNumber < 1)
            {
                spawnDoorBlock();
            }
        }
    }

    void spawnRoom(GameObject[] rooms)
    {
        GameObject selectedRoom = rooms[Random.Range(0, rooms.Length)];

        if (templates.antiList.Count < 6)
        {
            while(selectedRoom.GetComponentsInChildren<RoomSpawn>().Length < 3)
            {
                selectedRoom = rooms[Random.Range(0, rooms.Length)];
            }
            Instantiate(selectedRoom, transform.position, Quaternion.identity);
            return;
        }
        else if (templates.antiList.Count < 15)
        {
            while (selectedRoom.GetComponentsInChildren<RoomSpawn>().Length < 2 && selectedRoom.GetComponentsInChildren<RoomSpawn>().Length == 4)
            {
                selectedRoom = rooms[Random.Range(0, rooms.Length)];
            }
            Instantiate(selectedRoom, transform.position, Quaternion.identity);
            return;
        }
        else if (templates.antiList.Count < 30)
        {
            while (selectedRoom.GetComponentsInChildren<RoomSpawn>().Length >= 3 && selectedRoom.GetComponentsInChildren<RoomSpawn>().Length <= 1)
            {
                selectedRoom = rooms[Random.Range(0, rooms.Length)];
            }
            Instantiate(selectedRoom, transform.position, Quaternion.identity);
            return;
        }
        else
        {
            Instantiate(selectedRoom, transform.position, Quaternion.identity);
        }
        
    }

   
    void spawnDoorBlock()
    {
        if (spawnedDoorBlock == false)
        {
            switch (openDirection)
            {
                case 1:
                    //spawn block facing down
                    Instantiate(templates.doorBlock, transform.position + new Vector3(0, 11.1f, 0), Quaternion.Euler(0, 0, 90));
                    transform.parent.gameObject.GetComponent<SetRoomDesign>().memoryDoorsOpen[1] = 1;
                    break;
                case 2:
                    //spawn door facing up
                    Instantiate(templates.doorBlock, transform.position + new Vector3(0, -11.1f, 0), Quaternion.Euler(0, 0, 270));
                    transform.parent.gameObject.GetComponent<SetRoomDesign>().memoryDoorsOpen[0] = 1;
                    break;
                case 3:
                    //spawn door facing right
                    Instantiate(templates.doorBlock, transform.position + new Vector3(11.1f, 0, 0), Quaternion.Euler(0, 0, 0));
                    transform.parent.gameObject.GetComponent<SetRoomDesign>().memoryDoorsOpen[3] = 1;
                    break;
                case 4:
                    //spawn door facing left
                    Instantiate(templates.doorBlock, transform.position + new Vector3(-11.1f, 0, 0), Quaternion.Euler(0, 0, 180));
                    transform.parent.gameObject.GetComponent<SetRoomDesign>().memoryDoorsOpen[2] = 1;
                    break;
            }
            spawnedDoorBlock = true;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "AntiSpawnSpaceSpawner")
        {
            collisionNumber++;
            //if a spawn point collides with a room, it adds a tick to the collider number
            // this means that if it collides twice (from the two rooms overlapping, it'll have a collider number of 2)
            // if it collides none, then that means its a room on the end (reached 40 room limit) - a door block is spawned accordingly
        }
        
        if(collision.gameObject.GetComponent<AntiSpawnSpaceDetailer>() == null)
        {
            return;
        }

        switch (openDirection)
        {
            //Checks if the doorway is open or not
            case 1:
                if (collision.GetComponent<AntiSpawnSpaceDetailer>().topOpening != true)
                {
                    spawnDoorBlock();
                }
                break;
            case 2:
                if (collision.GetComponent<AntiSpawnSpaceDetailer>().bottomOpening != true)
                {
                    spawnDoorBlock();
                }
                break;
            case 3:
                if (collision.GetComponent<AntiSpawnSpaceDetailer>().rightOpening != true)
                {
                    spawnDoorBlock();
                }
                break;
            case 4:
                if (collision.GetComponent<AntiSpawnSpaceDetailer>().leftOpening != true)
                {
                    spawnDoorBlock();
                }
                break;
        }
        dontSpawn = true;

        if (templates.spawnPeriod >= 6.2f)
        {
            if (collision.gameObject.name != "AntiSpawnSpaceSpawner")
            {
                spawnDoorBlock();
            }
        }
    }
}
