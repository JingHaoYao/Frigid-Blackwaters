﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplementaryReturnDialogueManager : MonoBehaviour
{
    // This class handles every dialogue that doesn't have to do with story, building unlocking, or tutorial related
    // things such as explanations, or examine dialogues

    public string[] weaponUnlockDialogues;
    public ReturnNotifications returnNotifications;

    private void initializeUnlockWeaponDialogues()
    {
        switch (MiscData.dungeonLevelUnlocked)
        {
            case 2:
                DialogueSet dialogueSet = loadDialogue(weaponUnlockDialogues[0]);
                if (!MiscData.completedHubReturnDialogues.Contains(dialogueSet.gameObject.name))
                {
                    returnNotifications.dialoguesToDisplay.Add(dialogueSet);
                }
                break;
            case 3:
                break;
            // more dungeon levels later here
        } 
    }

    void Start()
    {
        initializeUnlockWeaponDialogues();
    }

    DialogueSet loadDialogue(string dialogueName)
    {
        return Resources.Load<DialogueSet>("Dialogues/Hub Return Dialogues/" + dialogueName);
    }
}