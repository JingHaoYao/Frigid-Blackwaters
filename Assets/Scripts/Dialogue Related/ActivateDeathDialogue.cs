using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeathDialogue : MonoBehaviour
{
    public DialogueUI dialogueUI;
    public GameObject dialogueBlackOverlay;
    ReturnNotifications returnNotifications;
    GameObject playerShip;
    [SerializeField] string whichDeathDialogue;

    void Start()
    {
        returnNotifications = FindObjectOfType<ReturnNotifications>();
        playerShip = GameObject.Find("PlayerShip");
        loadDeathDialogue();
    }

    void loadDeathDialogue()
    {
        if (MiscData.playerDied)
        {
            returnNotifications.dialoguesToDisplay.Add(Resources.Load<DialogueSet>("Dialogues/" + whichDeathDialogue));
            MiscData.playerDied = false;
            SaveSystem.SaveGame();
        }
    }
}
