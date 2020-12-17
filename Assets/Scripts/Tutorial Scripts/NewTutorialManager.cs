using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NewTutorialManager : MonoBehaviour
{
    [SerializeField] MoveCameraNextRoom moveCameraNextRoom;
    [SerializeField] Animator blackFadeAnimator;
    [SerializeField] BossRoomDoor bossRoomDoor;
    [SerializeField] TutorialSwitch leftTutorialSwitch, rightTutorialSwitch;
    [SerializeField] TutorialGolem tutorialGolem;
    [SerializeField] GameObject roomReveal;
    [SerializeField] GameObject roomBlock;
    [SerializeField] GameObject spaceBarSymbol;
    [SerializeField] AudioManager audioManager;
    [SerializeField] Image whiteFlash;
    [SerializeField] GameObject wasdPrompt;
    [SerializeField] GameObject openInventoryPrompt;
    [SerializeField] GameObject clickOnePrompt;

    [SerializeField] GameObject leviathanCannon;
    GameObject doorBlockInstant;

    bool firstTutorial = true;

    List<string> alreadyPlayedDialogues = new List<string>();

    private int numberDoorTriggers = 0;

    public void StartFirstTutorial()
    {
        blackFadeAnimator.SetTrigger("FadeIn");
        firstTutorial = true;
        moveCameraNextRoom.AddMoveCameraAction((Vector3 vec) => { TriggerDialoguePerPhase(vec); });
        PlayerProperties.playerScript.OverrideDeathAction(StartSecondTutorial);
        // Play dialogue, etc, need to implement those
    }

    IEnumerator flashWhite()
    {
        LeanTween.value(0, 1, 0.5f).setOnUpdate((float val) => { whiteFlash.color = new Color(1, 1, 1, val); });
        yield return new WaitForSeconds(1f);
        tutorialGolem.GuaranteedDeathSlam();
        LeanTween.value(1, 0, 0.5f).setOnUpdate((float val) => { whiteFlash.color = new Color(1, 1, 1, val); });
    }

    public void FlashWhite()
    {
        StartCoroutine(flashWhite());
    }

    public void StartSecondTutorial()
    {
        PlayerProperties.playerScript.SetAnimationShipType(0);
        firstTutorial = false;
        audioManager.FadeOut("Boss Battle Music", 0.2f);
        audioManager.FadeIn("Tutorial Background Music", 0.2f, 1f);
        wasdPrompt.SetActive(false);
        Destroy(doorBlockInstant);

        PlayerProperties.playerArtifacts.numKills = 0;
        PlayerProperties.playerScript.SetShipBackToNormal();
        PlayerProperties.playerShip.transform.position = Vector3.zero;
        tutorialGolem.ResetBoss();

        GameObject itemInstant = Instantiate(leviathanCannon);
        PlayerProperties.playerInventory.itemList.Add(itemInstant);
        // play dialogue with main crew
    }

    public void EndTutorial()
    {
        audioManager.FadeOut("Boss Battle Music", 0.2f);
        audioManager.FadeIn("Tutorial Background Music", 0.2f, 1f);
    }

    public void TriggerDoor()
    {
        numberDoorTriggers++;
        if(numberDoorTriggers >= 2)
        {
            bossRoomDoor.Open(() => { });
            // can be replaced with triggering a dialogue, etc
            leftTutorialSwitch.CloseTrigger();
            rightTutorialSwitch.CloseTrigger();
        }
    }

    public void UnTriggerDoor()
    {
        numberDoorTriggers--;
    }

    void TriggerDialoguePerPhase(Vector3 room)
    {
        int whichRoom = Mathf.RoundToInt(room.y / 20);

        if (firstTutorial)
        {
            switch (whichRoom)
            {
                case 1:
                    // check if dialogue is already in alreadyPlayedDialogues
                    break;
                case 2:
                    break;
                case 3:

                    Instantiate(roomReveal, new Vector3(0, 60), Quaternion.identity);
                    StartBoss(true, null);
                    break;
            }
        }
        else
        {
            // have options to play dialogues involved in the second tutorial
            switch (whichRoom)
            {
                case 1:
                    // check if dialogue is already in alreadyPlayedDialogues
                    break;
                case 2:
                    break;
                case 3:

                    Instantiate(roomReveal, new Vector3(0, 60), Quaternion.identity);
                    StartBoss(false, () => { StartCoroutine(checkPlayerInventoryStatus()); });
                    break;
            }
        }
    }

    IEnumerator checkPlayerInventoryStatus()
    {
        PlayerProperties.playerInventory.EnableInventory();
        while (true)
        {
            if (PlayerProperties.playerArtifacts.numKills >= 4)
            {
                if (PlayerProperties.playerArtifacts.activeArtifacts.Count > 0)
                {
                    clickOnePrompt.SetActive(true);
                    openInventoryPrompt.SetActive(false);
                }
                else
                {
                    openInventoryPrompt.SetActive(true);
                    clickOnePrompt.SetActive(false);
                }
            }
            else
            {
                openInventoryPrompt.SetActive(false);
                clickOnePrompt.SetActive(false);
            }
            yield return null;
        }
    }

    void StartBoss(bool shouldFlash, UnityAction afterSlamAction)
    {
        tutorialGolem.ResetBoss();
        tutorialGolem.StartBoss(
            () => {
                tutorialGolem.TutorialSlamDash(
                    () => {
                        StartCoroutine(PromptAndWaitForUserToDash());
                    }, 
                    3, 
                    () => {
                        tutorialGolem.GolemFlash(
                            shouldFlash, 
                            () => {
                                StartCoroutine(PromptAndWaitForUserToDash());
                            }, 
                            afterSlamAction
                            );
                    });
            });
        // can trigger dialogue at the end of start boss
        doorBlockInstant = Instantiate(roomBlock, new Vector3(0, 50f, 0), Quaternion.Euler(0, 0, 90));
        PlayerProperties.playerShip.transform.position = new Vector3(0, 53.35f, 0);
        audioManager.PlaySound("Boss Battle Music");
        audioManager.FadeIn("Boss Battle Music", 0.2f, 1);
        audioManager.FadeOut("Tutorial Background Music", 0.2f);
    }

    IEnumerator PromptAndWaitForUserToDash()
    {
        Time.timeScale = 0;

        spaceBarSymbol.transform.position = PlayerProperties.playerShipPosition + Vector3.up * 2;
        spaceBarSymbol.SetActive(true);
        PlayerProperties.playerShip.GetComponent<HullUpgradeManager>().SetDashOffCooldown();

        while (!Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.dash)))
        {
            yield return null;
        }
        PlayerProperties.playerScript.windowAlreadyOpen = false;
        spaceBarSymbol.SetActive(false);

        Time.timeScale = 1;
    }

    private void Start()
    {
        PlayerProperties.playerScript.SetAnimationShipType(12);
        StartFirstTutorial();
        PlayerProperties.playerInventory.DisableInventory();
    }
}
