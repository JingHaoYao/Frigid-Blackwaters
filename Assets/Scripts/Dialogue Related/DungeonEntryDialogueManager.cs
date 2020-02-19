using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntryDialogueManager : MonoBehaviour
{
    public string[] storyEntryDialogues;
    public string[] randomEntryDialogues;
    public DialogueUI dialogueUI;
    public GameObject dialogueBlackOverlay;
    public int whatDungeonLevel = 1;
    bool loadedDialogue = false;
    public GameObject mapSymbol, infoPointer;
    Dictionary<string, string> entryDialogueDict = new Dictionary<string, string>();
    MissionManager missionManager;

    private void Awake()
    {
        missionManager = FindObjectOfType<MissionManager>();
        for(int i = 0; i < missionManager.missionIDs.Count; i++)
        {
            entryDialogueDict.Add(missionManager.missionIDs[i], storyEntryDialogues[i]);
        }
    }

    public DialogueSet loadDialogue(string name, bool storyDialogue = false)
    {
        if (whatDungeonLevel == 1)
        {
            if (storyDialogue == true)
            {
                return Resources.Load<DialogueSet>("Dialogues/First Dungeon Level/Story Dialogues/" + name);
            }
            else
            {
                return Resources.Load<DialogueSet>("Dialogues/First Dungeon Level/Random Entry Dungeon Dialogue/" + name);
            }
        }
        else if (whatDungeonLevel == 2)
        {
            if (storyDialogue == true)
            {
                return Resources.Load<DialogueSet>("Dialogues/Second Dungeon Level/Story Dialogues/" + name);
            }
            else
            {
                return Resources.Load<DialogueSet>("Dialogues/Second Dungeon Level/Random Entry Dungeon Dialogue/" + name);
            }
        }
        return null;
    }

    void Update()
    {
        if(GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>().spawnPeriod >= 5.4f && loadedDialogue == false)
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
            if (!MiscData.completedEntryDungeonDialogues.Contains(entryDialogueDict[MiscData.missionID]))
            {
                dialogueUI.targetDialogue = loadDialogue(entryDialogueDict[MiscData.missionID], true);
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
