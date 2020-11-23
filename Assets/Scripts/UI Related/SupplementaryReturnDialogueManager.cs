using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplementaryReturnDialogueManager : MonoBehaviour
{
    // This class handles every dialogue that doesn't have to do with story, building unlocking, or tutorial related
    // things such as explanations, or examine dialogues

    public string[] weaponUnlockDialogues;
    public ReturnNotifications returnNotifications;
    [SerializeField] DialogueSet[] firstTimeLevelDialogues;

    [Header("Moving Locations")]
    [SerializeField] GameObject initialLocation;
    [SerializeField] int whatDialogueShift;
    // Used to put the ship in a different setting before moving them to the main player hub

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
                DialogueSet set = loadDialogue(weaponUnlockDialogues[1]);
                if (!MiscData.completedHubReturnDialogues.Contains(set.gameObject.name))
                {
                    returnNotifications.dialoguesToDisplay.Add(set);
                }
                break;
        } 
    }

    void movePlayerToHub(int count)
    {
        if(count == whatDialogueShift)
        {
            PlayerProperties.playerShip.transform.position = Vector3.zero;
            initialLocation.SetActive(false);
        }
    }

    private void initializeFirstTimeLevelDialogues()
    {
        if (firstTimeLevelDialogues.Length > 0)
        {

            if (!MiscData.completedHubReturnDialogues.Contains(firstTimeLevelDialogues[0].name))
            {
                if (initialLocation != null)
                {
                    initialLocation.SetActive(true);
                    PlayerProperties.playerShip.transform.position = initialLocation.transform.position;
                    returnNotifications.setMidDialogueAction(movePlayerToHub);
                }
            }

            for (int i = 0; i < firstTimeLevelDialogues.Length; i++)
            {
                if (!MiscData.completedHubReturnDialogues.Contains(firstTimeLevelDialogues[i].name))
                {
                    returnNotifications.dialoguesToDisplay.Add(firstTimeLevelDialogues[i]);
                }
            }
        }
    }

    void Start()
    {
        initializeFirstTimeLevelDialogues();
        initializeUnlockWeaponDialogues();
    }

    DialogueSet loadDialogue(string dialogueName)
    {
        return Resources.Load<DialogueSet>("Dialogues/Hub Return Dialogues/" + dialogueName);
    }
}
