using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrassGolemBossManager : BossManager
{
    public DialogueUI dialogueUI;
    public GameObject dialogueBlackOverlay;
    public MoveCameraNextRoom cameraScript;
    public PlayerScript playerScript;
    public TheBrassGolem brassGolem;
    public DungeonEntryDialogueManager dialogueManager;
    public string dialogueString;
    public BlackOverlay fadeWindow;
    public GameObject brassGolemRooms;
    public GameObject cameraFog;

    void InitializeBossFight()
    {
        brassGolem.InitializeBoss();
        cameraScript.trackPlayer = true;
        cameraScript.freeCam = false;
        playerScript.playerDead = false;
    }

    IEnumerator movePlayerToBossArea()
    {
        fadeWindow.transition();
        playerScript.playerDead = true;
        yield return new WaitForSeconds(1f);
        cameraFog.SetActive(false);
        playerScript.transform.position = new Vector3(1400, -10);
        cameraScript.transform.position = new Vector3(1400, -10);
        cameraScript.GetComponent<Camera>().orthographicSize = 20;
        brassGolem.gameObject.SetActive(true);
        StartCoroutine(openingAnimation());
    }

    public void startMovingPlayer()
    {
        brassGolemRooms.SetActive(true);
        StartCoroutine(movePlayerToBossArea());
    }

    IEnumerator openingAnimation()
    {
        cameraScript.freeCam = true;
        brassGolem.startOpenAnimation();
        yield return new WaitForSeconds(7.1f);
        /*dialogueUI.targetDialogue = dialogueManager.loadDialogue(dialogueString, true);
        dialogueUI.gameObject.SetActive(true);
        dialogueUI.setEndAction(() => { InitializeBossFight(); });
        dialogueBlackOverlay.SetActive(true);*/
        // TODO: Dialogue later

        InitializeBossFight();
    }
}
