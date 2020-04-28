using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YlvasOffer : MonoBehaviour
{
    [SerializeField] DialogueSet dialogueToDisplay;
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] GameObject dialogueBlackScreen;
    [SerializeField] GeneralSlots slots;
    [SerializeField] string[] itemsToSpawn;
    [SerializeField] GameObject examineIndicator;
    GameObject examineIndicatorInstant;

    void turnOnDialogue()
    {
        dialogueUI.targetDialogue = dialogueToDisplay;
        dialogueUI.gameObject.SetActive(true);
        dialogueBlackScreen.SetActive(true);
        dialogueUI.setEndAction(() => slots.spawnAllItems(itemsToSpawn));
    }

    private void Start()
    {
        turnOnIfNotClaimed();
    }

    void turnOnIfNotClaimed()
    {
        if (MiscData.completedHubReturnDialogues.Contains(dialogueToDisplay.name))
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            examineIndicatorInstant = Instantiate(examineIndicator, transform.position, Quaternion.identity);
            examineIndicatorInstant.GetComponent<ExamineIndicator>().parentObject = this.gameObject;
            examineIndicatorInstant.SetActive(false);
            StartCoroutine(mainLoop());
        }
    }

    IEnumerator mainLoop()
    {
        while (true)
        {
            if (Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) < 2.5f)
            {
                examineIndicatorInstant.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    turnOnDialogue();
                    examineIndicatorInstant.SetActive(false);
                    break;
                }
            }
            else
            {
                examineIndicatorInstant.SetActive(false);
            }
            yield return null;
        }
    }
}
