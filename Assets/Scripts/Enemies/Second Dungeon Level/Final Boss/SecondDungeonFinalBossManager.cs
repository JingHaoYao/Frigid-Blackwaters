using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDungeonFinalBossManager : BossManager
{
    public DialogueUI dialogueUI;
    public GameObject dialogueBlackOverlay;
    public MoveCameraNextRoom cameraScript;
    public PlayerScript playerScript;
    public SecondDungeonFinalBoss boss;
    public YlvaCompanion ylva;
    public BossHealthBar healthBar;
    public DungeonEntryDialogueManager dialogueManager;
    public string dialogueString;
    public BlackOverlay fadeWindow;

    void InitializeBossFight()
    {
        ylva.initializeYlvaLoop();
        boss.InitializeBoss();
        cameraScript.trackPlayer = true;
        cameraScript.freeCam = false;
        playerScript.playerDead = false;
    }

    IEnumerator movePlayerToBossArea()
    {
        fadeWindow.transition();
        playerScript.playerDead = true;
        yield return new WaitForSeconds(1f);
        playerScript.transform.position = new Vector3(1600, -8);
        cameraScript.transform.position = new Vector3(1600, 0);
        boss.gameObject.SetActive(true);
        ylva.gameObject.SetActive(true);
        Invoke("playOpeningAnim", 1f);
    }

    public void startMovingPlayer()
    {
        StartCoroutine(movePlayerToBossArea());
    }

    public void startUpAhalfarDialogue()
    {
        StartCoroutine(delayUntilDialogue());
    }

    IEnumerator delayUntilDialogue()
    {
        yield return new WaitForSeconds(2f);
        dialogueUI.targetDialogue = dialogueManager.loadDialogue("Ahalfar's Speech", true);
        dialogueUI.gameObject.SetActive(true);
        dialogueUI.setEndAction(() => { bossBeaten("ahalfar", 1f); });
        dialogueBlackOverlay.SetActive(true);
    }

    IEnumerator openingAnimation()
    {
        cameraScript.freeCam = true;
        boss.playOpeningAnimation();
        yield return new WaitForSeconds(3f);
        dialogueUI.targetDialogue = dialogueManager.loadDialogue(dialogueString, true);
        dialogueUI.gameObject.SetActive(true);
        dialogueUI.setEndAction(() => { InitializeBossFight(); });
        dialogueBlackOverlay.SetActive(true);
    }

    public void playOpeningAnim()
    {
        StartCoroutine(openingAnimation());
    }


}
