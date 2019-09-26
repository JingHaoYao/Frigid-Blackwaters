using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    void Start()
    {
        DungeonEntryDialogueManager dialogueManager = FindObjectOfType<DungeonEntryDialogueManager>();
        GetComponent<Text>().text = dialogueManager.whatDungeonLevel.ToString() + " - " + (MiscData.completedCheckPoints.Count - ((dialogueManager.whatDungeonLevel - 1) * 3) + 1).ToString() + " | " + FindObjectOfType<RoomTemplates>().maxRoomCount.ToString() + " Rooms";
    }
}
