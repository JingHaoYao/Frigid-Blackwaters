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
    public BossManager bossManager;

    //set for beating the boss initially

    private void Start()
    {
        returnFrontier = bossManager.finishedBossCheckPoint;
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
        GameObject.Find("Sentinel Boss Manager").GetComponent<BossManager>().bossBeaten("sentinel_boss", 0);
        yield return new WaitForSeconds(1f);
        Camera.main.orthographicSize = 10;
        Camera.main.GetComponent<MoveCameraNextRoom>().trackPlayer = false;
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
