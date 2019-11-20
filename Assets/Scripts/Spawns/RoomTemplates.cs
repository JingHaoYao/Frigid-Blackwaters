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

    public int maxRoomCount = 40;
    public int roomCount = 0;

    public float waitTime = 6.3f;
    public float spawnPeriod = 0;

    bool spawned = false;

    public void Awake()
    {
        dialogueManager = FindObjectOfType<DungeonEntryDialogueManager>();
        setMaxRoomCount();
    }

    void setMaxRoomCount()
    {
        maxRoomCount = (MiscData.completedCheckPoints.Count - ((dialogueManager.whatDungeonLevel - 1) * 3)) * 10 + 20;

        // ONLY FOR CURRENT WORK ON SECOND LEVEL
        if(dialogueManager.whatDungeonLevel == 2)
        {
            maxRoomCount = 20;
        }
    }

    private void Update()
    {
        if(spawnPeriod < 6.5f)
        {
            spawnPeriod += Time.deltaTime;
        }

        if (spawnPeriod >= 6.4f && MiscData.bossesDefeated.Count < 1 && spawned == false)
        {
            spawned = true;
            int index = antiList.Count - 1;
            while(antiList[index] == null)
            {
                index = index - 1;
            }
            antiList[index - dialogueManager.storyCheckPoints[MiscData.completedCheckPoints.Count - ((dialogueManager.whatDungeonLevel - 1) * 3)].numberFromLastRoom].checkPointRoom = true;
            //antiList[index - dialogueManager.storyCheckPoints[0].numberFromLastRoom].checkPointRoom = true;
            antiList[index].setRoomType();
        }
    }
}
