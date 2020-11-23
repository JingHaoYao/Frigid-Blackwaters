using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernDialogueManager : MonoBehaviour
{
    public string[] tavernDialogueNames;
    public DialogueUI dialogueUI;
    public GameObject tavernIcon;
    public Tavern openScript;

    void Awake()
    {
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
            else
            {
                openScript.enabled = false;
                tavernIcon.SetActive(false);
            }
        }
    }
}
