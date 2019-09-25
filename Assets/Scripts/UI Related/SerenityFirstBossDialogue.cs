using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SerenityFirstBossDialogue : MonoBehaviour
{
    public Animator blackScreenAnimator;
    public Animator sceneTransitionFadeOut;
    public FirstBossManager manager;
    public string[] dialogue;
    public string[] endDialogue;
    Text text;
    public Text spaceText;
    int index = 0;
    bool dialogueCompleted = false;
    bool endDialogueStart = false;
    bool animDone = false;
    public GameObject musketPhase;
    bool playCinematic = false;

    public DialogueSet firstBossDialogue;
    public DialogueUI dialogueUI;
    public GameObject dialogueBlackOverlay;

    void progressDialogue()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            index++;
            if (index < dialogue.Length)
            {
                text.text = dialogue[index];
            }
            else
            {
                text.text = "";
                text.enabled = false;
                spaceText.enabled = false;
                blackScreenAnimator.SetTrigger("FadeIn");
                manager.startBosses = true;
                GameObject.Find("PlayerShip").GetComponent<PlayerScript>().playerDead = false;
                dialogueCompleted = true;
                MiscData.readFirstBossDialogue = true;
                playIntroMusic();
            }
            FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
        }
    }

    void playIntroMusic()
    {
        FindObjectOfType<AudioManager>().PlaySound("Idle Ship Movement");
        FindObjectOfType<AudioManager>().PlaySound("First Boss Background Music");
    }

    void progressEndDialogue()
    {
        if (Input.GetKeyDown(KeyCode.Space) && animDone == true)
        {
            index++;
            if (index < endDialogue.Length)
            {
                text.text = endDialogue[index];
            }
            else
            {
                if (index == endDialogue.Length)
                {
                    text.enabled = false;
                    spaceText.enabled = false;
                    sceneTransitionFadeOut.SetTrigger("FadeOut");
                    if(MiscData.dungeonLevelUnlocked < 2)
                    {
                        MiscData.dungeonLevelUnlocked = 2;
                        PlayerUpgrades.numberMaxSkillPoints++;
                        MiscData.skillPointsNotification = true;
                    }
                    SaveSystem.SaveGame();
                    StartCoroutine(fadeLoadScene());
                }
            }
            FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
        }
    }

    IEnumerator fadeLoadScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    IEnumerator firstBossDefeated(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        blackScreenAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        text.enabled = true;
        spaceText.enabled = true;
        text.text = endDialogue[0];
        index = 0;
        animDone = true;
    }

    void Start()
    {
        text = GetComponent<Text>();
        text.text = dialogue[0];
        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().playerDead = true;

        if(MiscData.readFirstBossDialogue == true)
        {
            text.text = "";
            text.enabled = false;
            spaceText.enabled = false;
            blackScreenAnimator.SetTrigger("FadeIn");
            manager.startBosses = true;
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().playerDead = false;
            dialogueCompleted = true;
            playIntroMusic();
        }
    }

    void Update()
    {
        if (dialogueCompleted == false)
        {
            progressDialogue();
        }

        if(musketPhase == null)
        {
            FindObjectOfType<PlayerScript>().windowAlreadyOpen = true;
            if (/*MiscData.completedBossDialogues.Contains("first_boss_finished_dialogue") &&*/ playCinematic == false) {
                if (endDialogueStart == false)
                {
                    StartCoroutine(firstBossDefeated(6f));
                    endDialogueStart = true;
                }
                progressEndDialogue();
            }
            else
            {
                if (/*MiscData.completedBossDialogues.Contains("first_boss_finished_dialogue")*/true)
                {
                    if (endDialogueStart == false)
                    {
                        StartCoroutine(firstBossDefeated(1f));
                        endDialogueStart = true;
                    }
                    progressEndDialogue();
                }
                else
                {
                    if(playCinematic == false)
                    {
                        dialogueUI.targetDialogue = firstBossDialogue;
                        dialogueUI.gameObject.SetActive(true);
                        dialogueBlackOverlay.SetActive(true);
                        playCinematic = true;
                    }
                }
            }
        }
    }
}
