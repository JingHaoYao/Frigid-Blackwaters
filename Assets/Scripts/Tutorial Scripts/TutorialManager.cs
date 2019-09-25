using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    GameObject playerShip;
    PlayerScript playerScript;
    public string[] openingDialogue;
    public Color[] openingColorDialogue;
    public Text openingText;
    public GameObject blackOverlay;
    int openingTextIndex = 0;

    int whichTutorialPhase = 0;
    public GameObject tabitha;
    CharacterDialogue tabithaDialogue;
    public string[] firstTutorialDialogue;

    public string[] secondTutorialDialogue;

    public string[] thirdTutorialDialogue;

    public string[] fourthTutorialDialogue;

    public Text dialogueText;
    int inGameIndex = 0;

    public GameObject wasdSymbols, spaceSymbols, indicatorArrow1, indicatorArrow2;
    float spaceDelay = 0;

    public GameObject jklSymbols, indicatorArrow3;

    public GameObject oneSymbol, inventorySymbol, jklsymbols2, rockRubble;

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        playerScript = playerShip.GetComponent<PlayerScript>();
        playerScript.playerDead = true;
        openingText.text = openingDialogue[0];
        dialogueText.text = firstTutorialDialogue[0];
        dialogueText.transform.parent.gameObject.SetActive(false);
        tabithaDialogue = tabitha.GetComponent<CharacterDialogue>();
    }

    void Update()
    {
        if (spaceDelay < 0.3f)
        {
            spaceDelay += Time.deltaTime;
        }

        playerShip.GetComponent<Artifacts>().numKills = 20;

        if(openingTextIndex < openingDialogue.Length)
        {
            if (Input.GetKeyDown(KeyCode.Space) && spaceDelay >= 0.3f)
            {
                spaceDelay = 0;
                openingTextIndex++;
                if (openingTextIndex == openingDialogue.Length)
                {
                    openingText.text = "";
                    openingText.gameObject.SetActive(false);
                    blackOverlay.GetComponent<Animator>().SetTrigger("FadeIn");
                    tabithaDialogue.toggleLeft = true;
                    tabithaDialogue.toggleRight = false;
                    dialogueText.transform.parent.gameObject.SetActive(true);
                    FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                }
                else
                {
                    openingText.text = openingDialogue[openingTextIndex];
                    openingText.color = openingColorDialogue[openingTextIndex];
                    FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                }
            }
        }
        else
        {
            if(Vector2.Distance(Camera.main.transform.position, new Vector3(0, 40, 0)) < 0.2f && whichTutorialPhase == 0){
                tabithaDialogue.toggleLeft = true;
                tabithaDialogue.toggleRight = false;
                playerScript.playerDead = true;
                whichTutorialPhase = 1;
                inGameIndex = 0;
                dialogueText.transform.parent.gameObject.SetActive(true);
                dialogueText.text = secondTutorialDialogue[0];
                tabithaDialogue.gameObject.GetComponent<SpriteRenderer>().sprite = tabithaDialogue.characterSpriteList[2];
            }

            if (Vector2.Distance(Camera.main.transform.position, new Vector3(0, 60, 0)) < 0.2f && whichTutorialPhase == 1)
            {
                tabithaDialogue.toggleLeft = true;
                tabithaDialogue.toggleRight = false;
                playerScript.playerDead = true;
                whichTutorialPhase = 2;
                inGameIndex = 0;
                dialogueText.transform.parent.gameObject.SetActive(true);
                dialogueText.text = thirdTutorialDialogue[0];
                tabithaDialogue.gameObject.GetComponent<SpriteRenderer>().sprite = tabithaDialogue.characterSpriteList[7];
            }

            if(rockRubble == null && whichTutorialPhase == 2)
            {
                tabithaDialogue.toggleLeft = true;
                tabithaDialogue.toggleRight = false;
                playerScript.playerDead = true;
                whichTutorialPhase = 3;
                inGameIndex = 0;
                dialogueText.transform.parent.gameObject.SetActive(true);
                dialogueText.text = fourthTutorialDialogue[0];
                tabithaDialogue.gameObject.GetComponent<SpriteRenderer>().sprite = tabithaDialogue.characterSpriteList[16];
            }

            if (whichTutorialPhase == 0)
            {
                if (inGameIndex < firstTutorialDialogue.Length)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && spaceDelay >= 0.3f)
                    {
                        spaceDelay = 0;
                        inGameIndex++;
                        if (inGameIndex == firstTutorialDialogue.Length)
                        {
                            playerScript.playerDead = false;
                            tabithaDialogue.toggleLeft = false;
                            tabithaDialogue.toggleRight = true;
                            dialogueText.transform.parent.gameObject.SetActive(false);
                            wasdSymbols.SetActive(true);
                            spaceSymbols.SetActive(true);
                            indicatorArrow1.SetActive(true);
                            indicatorArrow2.SetActive(true);
                            FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                        }
                        else
                        {
                            dialogueText.text = firstTutorialDialogue[inGameIndex];
                            FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                        }
                    }
                }
            }
            else if (whichTutorialPhase == 1)
            {
                if (inGameIndex < secondTutorialDialogue.Length)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && spaceDelay >= 0.3f)
                    {
                        spaceDelay = 0;
                        inGameIndex++;
                        if (inGameIndex == secondTutorialDialogue.Length)
                        {
                            playerScript.playerDead = false;
                            tabithaDialogue.toggleLeft = false;
                            tabithaDialogue.toggleRight = true;
                            dialogueText.transform.parent.gameObject.SetActive(false);
                            jklSymbols.SetActive(true);
                            indicatorArrow3.SetActive(true);
                            FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                        }
                        else
                        {
                            dialogueText.text = secondTutorialDialogue[inGameIndex];
                            FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                        }
                    }
                }
            }
            else if (whichTutorialPhase == 2)
            {
                if (inGameIndex < thirdTutorialDialogue.Length)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && spaceDelay >= 0.3f)
                    {
                        spaceDelay = 0;
                        inGameIndex++;
                        if (inGameIndex == thirdTutorialDialogue.Length)
                        {
                            playerScript.playerDead = false;
                            tabithaDialogue.toggleLeft = false;
                            tabithaDialogue.toggleRight = true;
                            dialogueText.transform.parent.gameObject.SetActive(false);
                            inventorySymbol.SetActive(true);
                            FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                        }
                        else
                        {
                            dialogueText.text = thirdTutorialDialogue[inGameIndex];
                            FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                        }
                    }
                }
                else
                {
                    if(playerShip.GetComponent<Artifacts>().activeArtifacts.Count >= 1)
                    {
                        inventorySymbol.SetActive(false);
                        if (GameObject.Find("Lightning Effect(Clone)"))
                        {
                            oneSymbol.SetActive(false);
                            jklsymbols2.SetActive(true);
                        }
                        else
                        {
                            oneSymbol.SetActive(true);
                            jklsymbols2.SetActive(false);
                        }
                    }
                    else
                    {
                        inventorySymbol.SetActive(true);
                        oneSymbol.SetActive(false);
                        jklsymbols2.SetActive(false);
                    }
                }
            }
            else if(whichTutorialPhase == 3)
            {
                if (inGameIndex < fourthTutorialDialogue.Length)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && spaceDelay >= 0.3f)
                    {
                        spaceDelay = 0;
                        inGameIndex++;
                        if (inGameIndex == fourthTutorialDialogue.Length)
                        {
                            playerScript.playerDead = false;
                            tabithaDialogue.toggleLeft = false;
                            tabithaDialogue.toggleRight = true;
                            dialogueText.transform.parent.gameObject.SetActive(false);
                            FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                        }
                        else
                        {
                            dialogueText.text = fourthTutorialDialogue[inGameIndex];
                            FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                        }
                    }
                }
            }
        }
    }
}
