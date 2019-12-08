using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public DialogueSet targetDialogue;
    public Image dialogueBackground;
    public Image character1, character2, character3, character4;
    public Text dialogueText, dialogueName;
    int dialogueIndex = 0;
    public Animator blackOverlayAnimator;
    public Image panelImageBack, panelImageFront;
    public float waitReveal = 0;

    PlayerScript playerScript;

    void loadCharacterSprite(Image whichCharacter, Sprite characterArt)
    {
        if(characterArt != null)
        {
            whichCharacter.enabled = true;
            whichCharacter.sprite = characterArt;
        }
        else
        {
            whichCharacter.enabled = false;
        }
    }

    void loadDialoguePanel(Image backPanel, Image frontPanel, Sprite prevPanel, Sprite newPanel)
    {
        if (prevPanel == null && newPanel != null)
        {
            backPanel.enabled = false;
            frontPanel.enabled = true;
            frontPanel.sprite = newPanel;
            frontPanel.GetComponent<Animator>().SetTrigger("FadeOut");
        }
        else if (prevPanel != null && newPanel == null)
        {
            backPanel.enabled = true;
            frontPanel.enabled = false;
            frontPanel.sprite = null;
            backPanel.sprite = prevPanel;
            backPanel.GetComponent<Animator>().SetTrigger("FadeIn");
        }
        else if(prevPanel != null && newPanel != null)
        {
            backPanel.enabled = true;
            frontPanel.enabled = true;
            backPanel.sprite = prevPanel;
            frontPanel.sprite = newPanel;
            frontPanel.GetComponent<Animator>().SetTrigger("FadeOut");
        }
        else
        {
            backPanel.enabled = false;
            frontPanel.enabled = false;
        }
    }
    
    IEnumerator loadDialogue(float waitDuration)
    {
        blackOverlayAnimator.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        blackOverlayAnimator.enabled = false;
        if (targetDialogue.originalMusic == "Dungeon Ambiance")
        {
            // check whether the sound to fade out is the dungeon ambiance
            // since dungeon ambiance is faded in by other sources, just muted it to prevent music overlap
            FindObjectOfType<AudioManager>().MuteSound(targetDialogue.originalMusic);
        }
        else
        {
            FindObjectOfType<AudioManager>().PlaySound(targetDialogue.originalMusic);
            FindObjectOfType<AudioManager>().FadeOut(targetDialogue.originalMusic, 0.2f);
        }
        dialogueIndex = 0;
        if (targetDialogue.background != null)
        {
            dialogueBackground.enabled = true;
            dialogueBackground.sprite = targetDialogue.background;
        }
        else
        {
            dialogueBackground.enabled = false;
        }

        if (dialogueIndex < targetDialogue.character1Sprites.Length)
        {
            loadCharacterSprite(character1, targetDialogue.character1Sprites[0]);
        }
        else
        {
            character1.enabled = false;
        }

        if (dialogueIndex < targetDialogue.character2Sprites.Length)
        {
            loadCharacterSprite(character2, targetDialogue.character2Sprites[0]);
        }
        else
        {
            character2.enabled = false;
        }

        if (dialogueIndex < targetDialogue.character3Sprites.Length)
        {
            loadCharacterSprite(character3, targetDialogue.character3Sprites[0]);
        }
        else
        {
            character3.enabled = false;
        }

        if (dialogueIndex < targetDialogue.character4Sprites.Length)
        {
            loadCharacterSprite(character4, targetDialogue.character4Sprites[0]);
        }
        else
        {
            character4.enabled = false;
        }

        if (dialogueIndex < targetDialogue.panelSprites.Length)
        {
            loadDialoguePanel(panelImageBack, panelImageFront, null, targetDialogue.panelSprites[0]);
        }
        dialogueText.text = targetDialogue.dialogues[0];
        dialogueName.text = targetDialogue.dialogueNames[0];
        playerScript.playerDead = true;
        yield return new WaitForSeconds(waitDuration);
        blackOverlayAnimator.enabled = true;
        blackOverlayAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        FindObjectOfType<AudioManager>().PlaySound(targetDialogue.substituteMusic);
        FindObjectOfType<AudioManager>().FadeIn(targetDialogue.substituteMusic, 0.2f, 0.6f);
        waitReveal = 0;
        blackOverlayAnimator.enabled = false;
    }

    void progressDialogue(int index)
    {
        if (dialogueIndex < targetDialogue.character1Sprites.Length)
        {
            loadCharacterSprite(character1, targetDialogue.character1Sprites[index]);
        }
        else
        {
            character1.enabled = false;
        }

        if (dialogueIndex < targetDialogue.character2Sprites.Length)
        {
            loadCharacterSprite(character2, targetDialogue.character2Sprites[index]);
        }
        else
        {
            character2.enabled = false;
        }

        if (dialogueIndex < targetDialogue.character3Sprites.Length)
        {
            loadCharacterSprite(character3, targetDialogue.character3Sprites[index]);
        }
        else
        {
            character3.enabled = false;
        }

        if (dialogueIndex < targetDialogue.character4Sprites.Length)
        {
            loadCharacterSprite(character4, targetDialogue.character4Sprites[index]);
        }
        else
        {
            character4.enabled = false;
        }

        if (dialogueIndex < targetDialogue.panelSprites.Length)
        {
            loadDialoguePanel(panelImageBack, panelImageFront, targetDialogue.panelSprites[dialogueIndex - 1], targetDialogue.panelSprites[dialogueIndex]);
        }

        FindObjectOfType<AudioManager>().PlaySound("Dialogue Blip");

        dialogueText.text = targetDialogue.dialogues[index];
        dialogueName.text = targetDialogue.dialogueNames[index];
    }

    void Start()
    {
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        StartCoroutine(loadDialogue(waitReveal));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dialogueIndex < targetDialogue.dialogues.Length)
        {
            dialogueIndex++;
            if (dialogueIndex >= targetDialogue.dialogues.Length)
            {
                if (targetDialogue.addedItems != null)
                {
                    foreach (GameObject item in targetDialogue.addedItems)
                    {
                        if (GameObject.Find("PlayerShip").GetComponent<Inventory>().itemList.Count < PlayerItems.maxInventorySize)
                        {
                            if (GameObject.Find("PresentItems"))
                            {
                                GameObject instant = Instantiate(item);
                                instant.transform.SetParent(GameObject.Find("PresentItems").transform);
                                GameObject.Find("PlayerShip").GetComponent<Inventory>().itemList.Add(item);
                                PlayerItems.inventoryItemsIDs.Add(item.name);
                            }
                        }
                    }
                }
                addDialogueID(targetDialogue);
                SaveSystem.SaveGame();
                GameObject.Find("PlayerShip").GetComponent<PlayerScript>().windowAlreadyOpen = false;
                StartCoroutine(turnOffDialoguer());
            }
            else
            {
                progressDialogue(dialogueIndex);
            }
        }

        if(playerScript.playerDead == false && dialogueIndex < targetDialogue.dialogues.Length)
        {
            playerScript.playerDead = true;
        }
    }

    public void addDialogueID(DialogueSet set)
    {
        if (set.whatDialogueType == 1)
        {
            MiscData.completedTavernDialogues.Add(set.gameObject.name);
        }
        else if (set.whatDialogueType == 2)
        {
            MiscData.completedEntryDungeonDialogues.Add(set.gameObject.name);
        }
        else if(set.whatDialogueType == 4)
        {
            MiscData.completedExamineDialogues.Add(set.gameObject.name);
        }
        else if(set.whatDialogueType == 5)
        {
            MiscData.completedShopDialogues.Add(set.gameObject.name);
        }
        else if(set.whatDialogueType == 7)
        {
            MiscData.completedStoryDialogues.Add(set.gameObject.name);
            if (set.checkPointDialogue == true)
            {
                MiscData.completedCheckPoints.Add(set.gameObject.name);
            }
        }
        else if(set.whatDialogueType == 8)
        {
            MiscData.completedUniqueRoomsDialogues.Add(set.gameObject.name);
        }
        else if(set.whatDialogueType == 6)
        {
            MiscData.completedHubReturnDialogues.Add(set.gameObject.name);
        }
    }

    IEnumerator turnOffDialoguer()
    {
        blackOverlayAnimator.enabled = true;
        blackOverlayAnimator.SetTrigger("FadeOut");
        FindObjectOfType<AudioManager>().FadeOut(targetDialogue.substituteMusic, 0.2f);
        yield return new WaitForSeconds(1f);
        blackOverlayAnimator.SetTrigger("FadeIn");
        if (targetDialogue.originalMusic == "Dungeon Ambiance")
        {
            FindObjectOfType<AudioManager>().UnMuteSound(targetDialogue.originalMusic);
        }
        else
        {
            FindObjectOfType<AudioManager>().FadeIn(targetDialogue.originalMusic, 0.2f, .5f);
        }
        this.gameObject.SetActive(false);
        playerScript.playerDead = false;
        targetDialogue = null;
    }

    private void OnEnable()
    {
        if(playerScript == null)
            playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        StartCoroutine(loadDialogue(waitReveal));
    }

    
}
