using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
    private UnityAction endAction;
    bool loaded = false;

    Animator backPanelAnimator;
    Animator frontPanelAnimator;

    PlayerScript playerScript;

    Coroutine textTypingAnimation;
    bool isTypeAnimating = false;

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

    public void setEndAction(UnityAction action)
    {
        endAction = action;
    }

    void loadDialoguePanel(Image backPanel, Image frontPanel, Sprite prevPanel, Sprite newPanel)
    {
        if (prevPanel != newPanel)
        {
            if (prevPanel == null && newPanel != null)
            {
                backPanel.enabled = false;
                frontPanel.enabled = true;
                frontPanel.sprite = newPanel;
                frontPanelAnimator.SetTrigger("FadeOut");
            }
            else if (prevPanel != null && newPanel == null)
            {
                backPanel.enabled = true;
                frontPanel.enabled = false;
                frontPanel.sprite = null;
                backPanel.sprite = prevPanel;
                backPanelAnimator.enabled = true;
                backPanelAnimator.SetTrigger("FadeIn");
            }
            else if (prevPanel != null && newPanel != null)
            {
                backPanelAnimator.enabled = false;
                backPanel.color = Color.white;
                backPanel.enabled = true;
                frontPanel.enabled = true;
                backPanel.sprite = prevPanel;
                frontPanel.sprite = newPanel;
                frontPanelAnimator.SetTrigger("FadeOut");
            }
            else
            {
                backPanel.enabled = false;
                frontPanel.enabled = false;
            }
        }
    }

    IEnumerator loadDialogue(float waitDuration)
    {
        if (textTypingAnimation != null)
        {
            StopCoroutine(textTypingAnimation);
        }
        playerScript.playerDead = true;
        transform.GetChild(0).gameObject.SetActive(false);
        blackOverlayAnimator.enabled = false;

        if (targetDialogue.originalMusic != "")
        {
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
        }

        panelImageBack.enabled = false;
        panelImageFront.enabled = false;
        dialogueBackground.enabled = false;

        character1.enabled = false;
        character2.enabled = false;
        character3.enabled = false;
        character4.enabled = false;

        dialogueText.transform.parent.gameObject.SetActive(false);

        if (waitDuration != 0)
        {
            blackOverlayAnimator.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            yield return new WaitForSeconds(waitDuration);
        }
        else
        {
            blackOverlayAnimator.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            blackOverlayAnimator.enabled = true;
            // This line of code is weird, I decided to add a very brief delay that allows the animator to enter it's first state before transitioning to
            // the next state (This fixed a bug)
            yield return new WaitForEndOfFrame();
            blackOverlayAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1f);
        }
        transform.GetChild(0).gameObject.SetActive(true);
        dialogueText.transform.parent.gameObject.SetActive(true);

        dialogueIndex = 0;
        textTypingAnimation = StartCoroutine(animateText(0));
        dialogueText.color = targetDialogue.textColor;
        dialogueName.color = targetDialogue.textColor;
        dialogueName.text = targetDialogue.dialogueNames[0];
        if(dialogueName.text == "")
        {
            dialogueName.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            dialogueName.transform.parent.gameObject.SetActive(true);
        }

        if (dialogueIndex < targetDialogue.panelSprites.Length)
        {
            this.panelImageFront.GetComponent<Animator>().SetTrigger("FadeOut");
            this.panelImageBack.GetComponent<Animator>().SetTrigger("FadeOut");
            loadDialoguePanel(panelImageBack, panelImageFront, null, targetDialogue.panelSprites[0]);
        }
        else
        {
            loadDialoguePanel(panelImageBack, panelImageFront, null, null);
        }

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

        blackOverlayAnimator.enabled = true;
        blackOverlayAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        if (targetDialogue.substituteMusic != "")
        {
            FindObjectOfType<AudioManager>().PlaySound(targetDialogue.substituteMusic);
            FindObjectOfType<AudioManager>().FadeIn(targetDialogue.substituteMusic, 0.2f, 0.6f);
        }
        waitReveal = 0;
        blackOverlayAnimator.enabled = false;
        loaded = true;
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
        dialogueName.text = targetDialogue.dialogueNames[index];

        if (dialogueName.text == "")
        {
            dialogueName.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            dialogueName.transform.parent.gameObject.SetActive(true);
        }

        textTypingAnimation = StartCoroutine(animateText(index));
    }

    IEnumerator animateText(int index)
    {
        isTypeAnimating = true;
        int charIndex = 0;
        foreach (char c in targetDialogue.dialogues[index])
        {
            charIndex++;
            dialogueText.text = targetDialogue.dialogues[index].Substring(0, charIndex);
            if (c != ' ')
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
        isTypeAnimating = false;
        textTypingAnimation = null;
    }

    void Update()
    {
        if (dialogueIndex < targetDialogue.dialogues.Length && loaded == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isTypeAnimating)
                {
                    StopCoroutine(textTypingAnimation);
                    isTypeAnimating = false;
                    dialogueText.text = targetDialogue.dialogues[dialogueIndex];
                }
                else
                {
                    dialogueIndex++;
                    if (dialogueIndex >= targetDialogue.dialogues.Length)
                    {
                        endDialogueProcedure();
                    }
                    else
                    {
                        progressDialogue(dialogueIndex);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                dialogueIndex = targetDialogue.dialogues.Length;
                endDialogueProcedure();
            }
        }

        if(playerScript.playerDead == false && dialogueIndex < targetDialogue.dialogues.Length)
        {
            playerScript.playerDead = true;
        }
    }

    void endDialogueProcedure()
    {
        if (targetDialogue.addedItems != null)
        {
            foreach (GameObject item in targetDialogue.addedItems)
            {
                if (PlayerProperties.playerInventory.itemList.Count < PlayerItems.maxInventorySize)
                {
                    if (GameObject.Find("PresentItems"))
                    {
                        GameObject instant = Instantiate(item);
                        instant.transform.SetParent(GameObject.Find("PresentItems").transform);
                        PlayerProperties.playerInventory.itemList.Add(instant);
                        PlayerItems.inventoryItemsIDs.Add(instant.name);
                    }
                }
            }
        }
        addDialogueID(targetDialogue);
        SaveSystem.SaveGame();
        StartCoroutine(turnOffDialoguer());
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
        if (targetDialogue.substituteMusic != "")
        {
            FindObjectOfType<AudioManager>().FadeOut(targetDialogue.substituteMusic, 0.2f);
        }
        yield return new WaitForSeconds(1f);
        blackOverlayAnimator.SetTrigger("FadeIn");
        if (targetDialogue.originalMusic != "")
        {
            if (targetDialogue.originalMusic == "Dungeon Ambiance")
            {
                FindObjectOfType<AudioManager>().UnMuteSound(targetDialogue.originalMusic);
            }
            else
            {
                FindObjectOfType<AudioManager>().FadeIn(targetDialogue.originalMusic, 0.2f, .5f);
            }
        }
        this.gameObject.SetActive(false);
        targetDialogue = null;
        endAction?.Invoke();
        endAction = null;
        playerScript.playerDead = false;
        playerScript.windowAlreadyOpen = false;
        loaded = false;
    }

    private void OnEnable()
    {
        if (playerScript == null)
            playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();

        backPanelAnimator = panelImageBack.GetComponent<Animator>();
        frontPanelAnimator = panelImageFront.GetComponent<Animator>();

        playerScript.windowAlreadyOpen = true;
        StartCoroutine(loadDialogue(waitReveal));
    }
}
