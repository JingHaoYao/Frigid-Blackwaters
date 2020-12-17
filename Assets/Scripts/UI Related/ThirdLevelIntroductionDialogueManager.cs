using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdLevelIntroductionDialogueManager : MonoBehaviour
{
    [SerializeField] DialogueSet entryDialogue1, entryDialogue2;
    public DialogueUI dialogueUI;

    void StartUpDialogue()
    {
        if (!MiscData.completedHubReturnDialogues.Contains(entryDialogue1.name))
        {
            dialogueUI.LoadDialogueUI(entryDialogue1, 0f, () =>
            {
                StartCoroutine(waitUntilEndOfFrameToStartUpDialogue());
            });
        }
    }
    
    IEnumerator waitUntilEndOfFrameToStartUpDialogue()
    {
        yield return new WaitForEndOfFrame();

        dialogueUI.LoadDialogueUI(entryDialogue1, 0f);
    }

    private void Start()
    {
        StartUpDialogue();
    }
}
