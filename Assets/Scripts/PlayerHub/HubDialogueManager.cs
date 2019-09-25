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
            CharacterDialogue tabithaDialogue = openingTabithaCharacter.GetComponent<CharacterDialogue>();
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
                    dialogueText.text = dialogue[dialogueIndex];
                    nameText.text = nameList[dialogueIndex];
                }
            }
        }
    }

    IEnumerator turnOnMapSymbol()
    {
        yield return new WaitForSeconds(1.2f);
        mapSymbol.SetActive(true);
    }

    public void turnOnDialogueText(string[] dialogue, string[] nameList)
    {
        dialogueText.transform.parent.gameObject.SetActive(true);
        dialogueText.text = introductionDialogue[0];
        nameText.text = introductionNames[0];
        playerScript.playerDead = true;
    }
}
