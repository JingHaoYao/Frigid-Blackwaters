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
        notifications = FindObjectOfType<ReturnNotifications>().dialogueNotification;
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
            else if (MiscData.completedTavernDialogues.Count == 1 && MiscData.completedMissions.Count >= 1)
            {
                openScript.tavernDialogue = loadDialogue(tavernDialogueNames[1]);
                notifications.dialoguesAvailable.Add("Tavern");
            }
            else if (MiscData.completedTavernDialogues.Count == 2 && MiscData.completedMissions.Count >= 2)
            {
                openScript.tavernDialogue = loadDialogue(tavernDialogueNames[2]);
                notifications.dialoguesAvailable.Add("Tavern");
            }
            else if(MiscData.completedTavernDialogues.Count == 3 && MiscData.completedMissions.Count >= 3)
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
        else if(MiscData.dungeonLevelUnlocked == 2)
        {
            if(MiscData.completedTavernDialogues.Count == 4 && MiscData.completedMissions.Count >= 5)
            {
                openScript.tavernDialogue = loadDialogue(tavernDialogueNames[4]);
                notifications.dialoguesAvailable.Add("Tavern");
            }
            else if(MiscData.completedTavernDialogues.Count == 5 && MiscData.completedMissions.Count >= 6)
            {
                openScript.tavernDialogue = loadDialogue(tavernDialogueNames[5]);
                notifications.dialoguesAvailable.Add("Tavern");
            }
            else if(MiscData.completedTavernDialogues.Count == 6 && MiscData.completedMissions.Count >= 7)
            {
                openScript.tavernDialogue = loadDialogue(tavernDialogueNames[6]);
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
