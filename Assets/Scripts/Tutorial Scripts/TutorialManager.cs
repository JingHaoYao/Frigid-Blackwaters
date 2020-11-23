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

    Coroutine textTypingAnimation;

    public List<GameObject> spawnedSkeletons = new List<GameObject>();
    [SerializeField] GameObject tutorialSkeleton;

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        playerScript = playerShip.GetComponent<PlayerScript>();
        playerScript.playerDead = true;
        playerScript.windowAlreadyOpen = true;
        openingText.text = openingDialogue[0];
        dialogueText.text = firstTutorialDialogue[0];
        dialogueText.transform.parent.gameObject.SetActive(false);
        tabithaDialogue = tabitha.GetComponent<CharacterDialogue>();

        foreach(GameObject skeleton in spawnedSkeletons)
        {
            skeleton.GetComponent<TutorialSkeleton>().Initialize(this);
        }
    }

    IEnumerator turnPlayerDeadOffAtEndOfFrame()
    {
        yield return new WaitForSeconds(0.2f);
        playerScript.playerDead = false;
        playerScript.windowAlreadyOpen = false;
    }

    IEnumerator animateText(string dialogueToWrite)
    {
        int charIndex = 0;
        foreach (char c in dialogueToWrite)
        {
            charIndex++;
            dialogueText.text = dialogueToWrite.Substring(0, charIndex);
            if (c != ' ')
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
        textTypingAnimation = null;
    }

    void Update()
    {
        if (spaceDelay < 0.3f)
        {
            spaceDelay += Time.deltaTime;
        }

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
                        if (textTypingAnimation != null)
                        {
                            StopCoroutine(textTypingAnimation);
                            textTypingAnimation = null;
                            dialogueText.text = firstTutorialDialogue[inGameIndex];
                        }
                        else
                        {
                            inGameIndex++;
                            if (inGameIndex == firstTutorialDialogue.Length)
                            {
                                StartCoroutine(turnPlayerDeadOffAtEndOfFrame());
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
                                textTypingAnimation = StartCoroutine(animateText(firstTutorialDialogue[inGameIndex]));
                                tabithaDialogue.updateSprite();
                                FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                            }
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
                        if (textTypingAnimation != null)
                        {
                            StopCoroutine(textTypingAnimation);
                            textTypingAnimation = null;
                            dialogueText.text = secondTutorialDialogue[inGameIndex];
                        }
                        else
                        {
                            inGameIndex++;
                            if (inGameIndex == secondTutorialDialogue.Length)
                            {
                                StartCoroutine(turnPlayerDeadOffAtEndOfFrame());
                                tabithaDialogue.toggleLeft = false;
                                tabithaDialogue.toggleRight = true;
                                dialogueText.transform.parent.gameObject.SetActive(false);
                                jklSymbols.SetActive(true);
                                indicatorArrow3.SetActive(true);
                                FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                            }
                            else
                            {
                                textTypingAnimation = StartCoroutine(animateText(secondTutorialDialogue[inGameIndex]));
                                tabithaDialogue.updateSprite();
                                FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                            }
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
                        if (textTypingAnimation != null)
                        {
                            StopCoroutine(textTypingAnimation);
                            textTypingAnimation = null;
                            dialogueText.text = thirdTutorialDialogue[inGameIndex];
                        }
                        else
                        {
                            inGameIndex++;
                            if (inGameIndex == thirdTutorialDialogue.Length)
                            {
                                StartCoroutine(turnPlayerDeadOffAtEndOfFrame());
                                tabithaDialogue.toggleLeft = false;
                                tabithaDialogue.toggleRight = true;
                                dialogueText.transform.parent.gameObject.SetActive(false);
                                inventorySymbol.SetActive(true);
                                FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                            }
                            else
                            {
                                textTypingAnimation = StartCoroutine(animateText(thirdTutorialDialogue[inGameIndex]));
                                tabithaDialogue.updateSprite();
                                FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                            }
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
                        if (textTypingAnimation != null)
                        {
                            StopCoroutine(textTypingAnimation);
                            textTypingAnimation = null;
                            dialogueText.text = fourthTutorialDialogue[inGameIndex];
                        }
                        else
                        {
                            inGameIndex++;
                            if (inGameIndex == fourthTutorialDialogue.Length)
                            {
                                StartCoroutine(turnPlayerDeadOffAtEndOfFrame());
                                tabithaDialogue.toggleLeft = false;
                                tabithaDialogue.toggleRight = true;
                                dialogueText.transform.parent.gameObject.SetActive(false);
                                FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                            }
                            else
                            {
                                textTypingAnimation = StartCoroutine(animateText(fourthTutorialDialogue[inGameIndex]));
                                tabithaDialogue.updateSprite();
                                FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");
                            }
                        }
                    }
                }
            }
        }

        if(Vector2.Distance(new Vector3(0, 40), PlayerProperties.mainCameraPosition) < 1 && whichTutorialPhase >= 2) {
            if(spawnedSkeletons.Count < 3)
            {
                for(int i = 0; i < 4; i++)
                {
                    float randAngle = Random.Range(0, Mathf.PI * 2);
                    Vector3 positionToSpawn = new Vector3(0, 33) + new Vector3(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * Random.Range(1.0f, 3.0f);
                    GameObject skeletonInstant = Instantiate(tutorialSkeleton, positionToSpawn, Quaternion.identity);
                    skeletonInstant.GetComponent<TutorialSkeleton>().Initialize(this);
                    spawnedSkeletons.Add(skeletonInstant);
                }
            }
        }
    }

}
