using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdLevelIntroductionDialogueManager : MonoBehaviour
{
    [SerializeField] DialogueSet entryDialogue1, entryDialogue2;
    public DialogueUI dialogueUI;
    public GameObject dialogueBlackScreen;

    void StartUpDialogue()
    {
        if (!MiscData.completedHubReturnDialogues.Contains(entryDialogue1.name))
        {
            dialogueUI.targetDialogue = entryDialogue1;
            dialogueUI.setEndAction(() =>
            {
                StartCoroutine(waitUntilEndOfFrameToStartUpDialogue());
            });
            dialogueUI.gameObject.SetActive(true);
            dialogueBlackScreen.SetActive(true);
        }
    }
    
    IEnumerator waitUntilEndOfFrameToStartUpDialogue()
    {
        yield return new WaitForEndOfFrame();
        dialogueUI.gameObject.SetActive(true);
        dialogueUI.targetDialogue = entryDialogue1;
        dialogueBlackScreen.SetActive(true);
    }

    private void Start()
    {
        StartUpDialogue();
    }
}
