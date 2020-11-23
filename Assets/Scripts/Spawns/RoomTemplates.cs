using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour {
    public GameObject[] topOpenRooms;
    public GameObject[] bottomOpenRooms;
    public GameObject[] rightOpenRooms;
    public GameObject[] leftOpenRooms;
    DungeonEntryDialogueManager dialogueManager;
    
    public GameObject doorBlock;
    public List<AntiSpawnSpaceDetailer> antiList = new List<AntiSpawnSpaceDetailer>();

    private Dictionary<(int, int), AntiSpawnSpaceDetailer> roomDictionary = new Dictionary<(int, int), AntiSpawnSpaceDetailer>();
    AntiSpawnSpaceDetailer currentActiveRoom;

    public int maxRoomCount = 40;
    public int roomCount = 0;

    public float waitTime = 6.3f;

    public int numberBranchRooms = 0;

    bool spawned = false;

    public void Awake()
    {
        dialogueManager = FindObjectOfType<DungeonEntryDialogueManager>();
        StartCoroutine(initialProcedure());
    }

    public void setMaxRoomCount()
    {
        maxRoomCount = FindObjectOfType<MissionManager>().currMission.numberDungeonRooms;

        if(dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DungeonEntryDialogueManager>();
        }
    }

    IEnumerator initialProcedure()
    {
        yield return new WaitForSeconds(5.4f);

        dialogueManager.Initialize();

        yield return new WaitForSeconds(1f);

        spawned = true;
        int index = antiList.Count - 1;
        while (antiList[index] == null)
        {
            index = index - 1;
        }
        antiList[index].checkPointRoom = true;
        antiList[index].setRoomType();

        foreach(AntiSpawnSpaceDetailer roomManager in antiList)
        {
            roomDictionary.Add((Mathf.RoundToInt(roomManager.transform.position.x), Mathf.RoundToInt(roomManager.transform.position.y)), roomManager);
            roomManager.Initialize();
            roomManager.EndRoomSpawn();
            roomManager.transform.parent.gameObject.SetActive(false);
        }

        currentActiveRoom = antiList[0];
        currentActiveRoom.transform.parent.gameObject.SetActive(true);
    }

    public bool areRoomsSpawned()
    {
        return spawned && antiList.Count > 0;
    }

    public void UpdateAndInitializeRoom(Vector3 position)
    {
        if(!roomDictionary.ContainsKey((Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y))))
        {
            return;
        }

        currentActiveRoom.transform.parent.gameObject.SetActive(false);
        currentActiveRoom = roomDictionary[(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y))];
        currentActiveRoom.transform.parent.gameObject.SetActive(true);
        if (!currentActiveRoom.spawnRoom)
        {
            currentActiveRoom.InitializeRoomEntry();
        }
    }
}
