using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossEntrance : MonoBehaviour
{
    GameObject playerShip;
    public GameObject firstBossStatue;
    public DialogueSet firstBossDialogue;
    DialogueUI dialogueUI;
    GameObject dialogueBlackOverlay;

    void adjustRotation()
    {
        if(playerShip.transform.position.x - transform.position.x > 2.5f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if(playerShip.transform.position.x - transform.position.x < 2.5f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(playerShip.transform.position.y - transform.position.y > 2.5f)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        dialogueUI = FindObjectOfType<DungeonEntryDialogueManager>().dialogueUI;
        dialogueBlackOverlay = dialogueUI.blackOverlayAnimator.gameObject;
        Instantiate(firstBossStatue, transform.position + new Vector3(0.4f, -2, 0), Quaternion.identity);
        adjustRotation();
    }
}
