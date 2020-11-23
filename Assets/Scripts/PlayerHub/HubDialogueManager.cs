using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubDialogueManager : MonoBehaviour
{
    public string[] introductionDialogue;
    public string[] introductionNames;
    public int dialogueIndex = 0;
    GameObject playerShip;
    PlayerScript playerScript;
    public Text dialogueText;
    public Text nameText;
    public GameObject openingTabithaCharacter;
    bool runTutorialHubText = false;
    public GameObject mapSymbol;

    Coroutine textTypingAnimation;
    CharacterDialogue tabithaDialogue;

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        playerScript = playerShip.GetComponent<PlayerScript>();
        if(MiscData.finishedTutorial == false)
        {
            MiscData.finishedTutorial = true;
            runTutorialHubText = true;
            playerShip.transform.position = new Vector3(20, -22, 0);
            playerScript.playerDead = true;
            dialogueText.transform.parent.gameObject.SetActive(true);
            dialogueText.text = introductionDialogue[dialogueIndex];
            nameText.text = introductionNames[dialogueIndex];
            tabithaDialogue = openingTabithaCharacter.GetComponent<CharacterDialogue>();
            tabithaDialogue.toggleLeft = true;
            tabithaDialogue.toggleRight = false;
        }
    }

    void Update()
    {
        if (runTutorialHubText == true && dialogueIndex < introductionDialogue.Length)
        {
            CharacterDialogue tabithaDialogue = openingTabithaCharacter.GetComponent<CharacterDialogue>();
            progressDialogue(introductionDialogue, introductionNames);
            if(dialogueIndex == introductionDialogue.Length)
            {
                tabithaDialogue.toggleRight = true;
                tabithaDialogue.toggleLeft = false;
            }

        }
    }

    void progressDialogue(string[] dialogue, string[] nameList)
    {
        if(dialogueIndex < dialogue.Length)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(textTypingAnimation != null)
                {
                    dialogueText.text = dialogue[dialogueIndex];
                    StopCoroutine(textTypingAnimation);
                    textTypingAnimation = null;
                }
                else
                {
                    dialogueIndex++;
                    if (dialogueIndex == dialogue.Length)
                    {
                        FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                        FindObjectOfType<Tavern>().turnOnDialogueUI();
                        StartCoroutine(turnOnMapSymbol());
                        dialogueText.transform.parent.gameObject.SetActive(false);
                    }
                    else
                    {
                        FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                        nameText.text = nameList[dialogueIndex];
                        textTypingAnimation = StartCoroutine(animateText(dialogue[dialogueIndex]));
                        tabithaDialogue.updateSprite();
                    }
                }
               
            }
        }
    }

    IEnumerator animateText(string dialogueToWrite)
    {
        int charIndex = 0;
        foreach (char c in dialogueToWrite)
        {
            charIndex++;
            dialogueText.text= dialogueToWrite.Substring(0, charIndex);
            if (c != ' ')
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
        textTypingAnimation = null;
    }

    IEnumerator turnOnMapSymbol()
    {
        yield return new WaitForSeconds(1.2f);
        mapSymbol.SetActive(true);
    }

    public void turnOnDialogueText(string[] dialogue, string[] nameList)
    {
        dialogueText.transform.parent.gameObject.SetActive(true);
        textTypingAnimation = StartCoroutine(animateText(dialogue[0]));
        nameText.text = introductionNames[0];
        playerScript.playerDead = true;
    }
}
