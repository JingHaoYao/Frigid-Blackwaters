using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSentinel : MonoBehaviour
{
    bool dialogueOpened = false;
    bool setBackToFrontier = false;
    public DialogueSet dialogueSet;
    StoryCheckpoint returnFrontier;

    DialogueUI dialogueUI;

    //set for beating the boss initially

    private void Start()
    {
        returnFrontier = GameObject.Find("Return From Boss Frontier").GetComponent<StoryCheckpoint>();
        dialogueUI = FindObjectOfType<DungeonEntryDialogueManager>().dialogueUI;
        StartCoroutine(startUpDialogue());
    }

    IEnumerator startUpDialogue()
    {
        yield return new WaitForSeconds(1f);
        dialogueOpened = true;
        GameObject dialogueBlackOverlay = FindObjectOfType<DungeonEntryDialogueManager>().dialogueBlackOverlay;
        dialogueUI.targetDialogue = dialogueSet;
        dialogueUI.gameObject.SetActive(true);
        dialogueBlackOverlay.SetActive(true);
    }

    IEnumerator dieDown()
    {
        FindObjectOfType<BlackOverlay>().transition();
        yield return new WaitForSeconds(1f);
        Camera.main.orthographicSize = 10;
        Camera.main.GetComponent<MoveCameraNextRoom>().trackPlayer = false;
        Camera.main.transform.position = returnFrontier.cameraPosition;
        FindObjectOfType<PlayerScript>().gameObject.transform.position = returnFrontier.playerShipPosition;
        returnFrontier.startUpDialogue();
        yield return new WaitForSeconds(1f);
    }

    private void Update()
    {
        if(dialogueOpened == true && dialogueUI.gameObject.activeSelf == false)
        {
            if(setBackToFrontier == false)
            {
                setBackToFrontier = true;
                StartCoroutine(dieDown());
            }
        }
    }
}
