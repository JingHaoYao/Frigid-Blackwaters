using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryCheckpoint : MonoBehaviour
{
    public Vector3 playerShipPosition;
    public Vector3 cameraPosition;
    public string dialogueName;
    public DialogueUI dialogueUI;
    bool portHub = false;
    public bool moveable = false;

    //how many rooms from the last room should the checkpoint be spawned (typically every 2 rooms equals 1 ish rooms closer to the spawn (by distance))
    public int numberFromLastRoom = 0;

    //checkpoint id is taken in by the dialogue id of the checkpoint dialogue.

    public void startUpDialogue(float waitReveal = 2)
    {
        this.gameObject.SetActive(true);
        dialogueUI.LoadDialogueUI(FindObjectOfType<DungeonEntryDialogueManager>().loadDialogue(dialogueName, true), waitReveal);
    }

    public void Update()
    {
        if (moveable == false && Vector2.Distance(Camera.main.transform.position, cameraPosition) < 0.5f)
        {
            if (dialogueUI.gameObject.activeSelf == false && portHub == false)
            {
                portHub = true;
                PlayerProperties.pauseMenu.loadHub();
            }
            PlayerProperties.playerScript.addRootingObject();
        }
    }
}
