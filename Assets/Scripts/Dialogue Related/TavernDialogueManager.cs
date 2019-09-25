using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernDialogueManager : MonoBehaviour
{
    public string[] tavernDialogueNames;
    public DialogueUI dialogueUI;
    public GameObject tavernIcon;
    public Tavern openScript;
    public NotificationBell notifications;
    string newTavernNotification = "New Tavern Dialogue Unlocked";

    void Awake()
    {
        notifications = GameObject.Find("Dialogue Notifications").GetComponent<NotificationBell>();

        /*tavernDialogueNames = new string[tavernDialogues.Length];

        for(int i = 0; i < tavernDialogueNames.Length; i++)
        {
            tavernDialogueNames[i] = tavernDialogues[i].name;
        }*/

        loadTavernDialogue();
    }

    DialogueSet loadDialogue(string dialogueName)
    {
        return Resources.Load<DialogueSet>("Dialogues/Tavern Dialogues/" + dialogueName);
    }

    void loadTavernDialogue()
    {
        if (MiscData.dungeonLevelUnlocked == 1)
        {
            if (MiscData.completedTavernDialogues.Count == 0)
            {
                openScript.tavernDialogue = loadDialogue(tavernDialogueNames[0]);
            }
            else if (MiscData.completedTavernDialogues.Count == 1 && MiscData.completedCheckPoints.Count >= 1)
            {
                openScript.tavernDialogue = loadDialogue(tavernDialogueNames[1]);
                notifications.dialoguesAvailable.Add("Tavern");
            }
            else if (MiscData.completedTavernDialogues.Count == 2 && MiscData.completedCheckPoints.Count >= 2)
            {
                openScript.tavernDialogue = loadDialogue(tavernDialogueNames[2]);
                notifications.dialoguesAvailable.Add("Tavern");
            }
            else if(MiscData.completedTavernDialogues.Count == 3 && MiscData.completedCheckPoints.Count >= 3)
            {
                openScript.tavernDialogue = loadDialogue(tavernDialogueNames[3]);
                notifications.dialoguesAvailable.Add("Tavern");
            } 
            else
            {
                openScript.enabled = false;
                tavernIcon.SetActive(false);
            }
        }
    }
}
