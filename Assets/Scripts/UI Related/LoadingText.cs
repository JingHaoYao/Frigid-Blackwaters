using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    void Start()
    {
        DungeonEntryDialogueManager dialogueManager = FindObjectOfType<DungeonEntryDialogueManager>();
        GetComponent<Text>().text = FindObjectOfType<MissionManager>().currMission.bossName.ToString() + " | " + FindObjectOfType<RoomTemplates>().maxRoomCount.ToString() + " Rooms";
    }
}
