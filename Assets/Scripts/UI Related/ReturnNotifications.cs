using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnNotifications : MonoBehaviour
{
    PlayerScript playerScript;
    public GameObject notificationBar;
    public List<DialogueSet> dialoguesToDisplay;

    public DialogueUI dialogueUI;
    public GameObject blackOverlay;

    bool notificationsClosed = false;
    bool buildingUnlocked = false;

    public GameObject mapSymbol;

    void Awake()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        if (MiscData.finishedTutorial == false)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            this.GetComponent<Image>().enabled = false;
        }
        else
        {
            playerScript.windowAlreadyOpen = true;
            playerScript.playerDead = true;
        }
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) && this.GetComponent<Image>().enabled == true)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            playerScript.windowAlreadyOpen = false;
            playerScript.playerDead = false;
            notificationsClosed = true;
            this.GetComponent<Image>().enabled = false;
        }

        if(notificationsClosed == true && dialoguesToDisplay.Count > 0)
        {
            if(dialogueUI.gameObject.activeSelf == false)
            {
                dialogueUI.targetDialogue = dialoguesToDisplay[0];
                dialoguesToDisplay.Remove(dialoguesToDisplay[0]);
                dialogueUI.gameObject.SetActive(true);
                blackOverlay.SetActive(true);
                buildingUnlocked = true;
            }
        }

        if(dialoguesToDisplay.Count == 0 && buildingUnlocked)
        {
            if(dialogueUI.gameObject.activeSelf == false)
            {
                mapSymbol.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }
}
