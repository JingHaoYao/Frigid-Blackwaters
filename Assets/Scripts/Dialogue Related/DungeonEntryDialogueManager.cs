using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntryDialogueManager : MonoBehaviour
{
    public string[] storyEntryDialogues;
    public string[] randomEntryDialogues;
    public DialogueUI dialogueUI;
    public GameObject dialogueBlackOverlay;
    public StoryCheckpoint[] storyCheckPoints;
    public StoryCheckpoint bossCheckpoint;
    public int whatDungeonLevel = 1;
    bool loadedDialogue = false;
    public GameObject mapSymbol, infoPointer;

    /*private void Start()
    {
        storyEntryDialogues = new string[storyDungeonEntryDialogues.Length];
        randomEntryDialogues = new string[randomDungeonEntryDialogues.Length];

        for(int i = 0; i < storyEntryDialogues.Length; i++)
        {
            storyEntryDialogues[i] = storyDungeonEntryDialogues[i].gameObject.name;
        }
        
        for(int i = 0; i < randomEntryDialogues.Length; i++)
        {
            randomEntryDialogues[i] = randomDungeonEntryDialogues[i].gameObject.name;
        }
    }*/

    public DialogueSet loadDialogue(string name, bool storyDialogue = false)
    {
        if(storyDialogue == true)
        {
            return Resources.Load<DialogueSet>("Dialogues/First Dungeon Level/Story Dialogues/" + name);
        }
        else
        {
            return Resources.Load<DialogueSet>("Dialogues/First Dungeon Level/Random Entry Dungeon Dialogue/" + name);
        }
    }

    void Update()
    {
        if(GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>().spawnPeriod >= 6.4f && loadedDialogue == false)
        {
            loadDungeonDialogue();
            loadedDialogue = true;
            SaveSystem.SaveGame();
        }

        if(MiscData.completedEntryDungeonDialogues.Count == 1 && dialogueUI.gameObject.activeSelf == false && loadedDialogue == true && MiscData.dungeonMapSymbolShown == false)
        {
            mapSymbol.SetActive(true);
            infoPointer.SetActive(true);
            MiscData.dungeonMapSymbolShown = true;
            SaveSystem.SaveGame();
        }
    }

    void loadDungeonDialogue()
    {
        if (whatDungeonLevel == 1)
        {
            if ((MiscData.completedEntryDungeonDialogues.Count - (whatDungeonLevel - 1) * 3) == 0)
            {
                dialogueUI.targetDialogue = loadDialogue(storyEntryDialogues[0], true);
                dialogueUI.gameObject.SetActive(true);
                dialogueBlackOverlay.SetActive(true);
            }
            else if ((MiscData.completedEntryDungeonDialogues.Count - (whatDungeonLevel - 1) * 3) == 1 && MiscData.completedCheckPoints.Count == 1)
            {
                dialogueUI.targetDialogue = loadDialogue(storyEntryDialogues[1], true);
                dialogueUI.gameObject.SetActive(true);
                dialogueBlackOverlay.SetActive(true);
            }
            else if ((MiscData.completedEntryDungeonDialogues.Count - (whatDungeonLevel - 1) * 3) == 2 && MiscData.completedCheckPoints.Count == 2)
            {
                dialogueUI.targetDialogue = loadDialogue(storyEntryDialogues[2], true);
                dialogueUI.gameObject.SetActive(true);
                dialogueBlackOverlay.SetActive(true);
            }
            else if ((MiscData.completedEntryDungeonDialogues.Count - (whatDungeonLevel - 1) * 3) == 3 && MiscData.completedCheckPoints.Count == 3)
            {
                dialogueUI.targetDialogue = loadDialogue(storyEntryDialogues[3], true);
                dialogueUI.gameObject.SetActive(true);
                dialogueBlackOverlay.SetActive(true);
            }
            else
            {
                if (Random.Range(1, 5) == 1 && MiscData.enoughRoomsTraversed == true)
                {
                    MiscData.enoughRoomsTraversed = false;
                    dialogueUI.targetDialogue = loadDialogue(randomEntryDialogues[Random.Range(0, randomEntryDialogues.Length)]);
                    dialogueUI.gameObject.SetActive(true);
                    dialogueBlackOverlay.SetActive(true);
                }
            }
        }
    }
}
