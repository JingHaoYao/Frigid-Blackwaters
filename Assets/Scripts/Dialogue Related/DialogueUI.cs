using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueUI : MonoBehaviour
{
    DialogueSet targetDialogue;
    public Image dialogueBackground;
    public Image character1, character2, character3, character4;
    public Text dialogueText, dialogueName;
    int dialogueIndex = 0;
    public Animator blackOverlayAnimator;
    public Image panelImageBack, panelImageFront;
    private UnityAction endAction;
    bool loaded = false;

    Animator backPanelAnimator;
    Animator frontPanelAnimator;

    PlayerScript playerScript;

    Coroutine textTypingAnimation;
    bool isTypeAnimating = false;

    bool slideInAnimation = false;

    public void LoadDialogueUI(DialogueSet dialogueSet, float waitReveal, UnityAction endAction = null, bool slideInAnimation = false)
    {
        this.slideInAnimation = slideInAnimation;
        targetDialogue = dialogueSet;
        this.gameObject.SetActive(true);
        blackOverlayAnimator.gameObject.SetActive(!slideInAnimation);

        playerScript.windowAlreadyOpen = true;
        StartCoroutine(loadDialogue(waitReveal));
        this.endAction = endAction;
    }

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

        if (slideInAnimation == false)
        {
            blackOverlayAnimator.enabled = true;
            dialogueBackground.color = Color.white;
            if (waitDuration != 0)
            {
                blackOverlayAnimator.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
                yield return new WaitForSeconds(waitDuration);
            }
            else
            {
                blackOverlayAnimator.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                // This line of code is weird, I decided to add a very brief delay that allows the animator to enter it's first state before transitioning to
                // the next state (This fixed a bug)
                yield return new WaitForEndOfFrame();
                blackOverlayAnimator.SetTrigger("FadeOut");
                yield return new WaitForSeconds(1f);
            }
        }
        else
        {
            dialogueBackground.color = new Color(1, 1, 1, 0);
            LeanTween.value(0, 1, 0.5f).setOnUpdate((float val) => dialogueBackground.color = dialogueBackground.color = new Color(1, 1, 1, val));
        }

        if (slideInAnimation == false)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        dialogueText.transform.parent.gameObject.SetActive(true);

        dialogueIndex = 0;
        textTypingAnimation = StartCoroutine(animateText(0));

        if (targetDialogue.textColors != null && targetDialogue.textColors.Length > 0)
        {
            dialogueText.color = targetDialogue.textColors[0];
        }
        else
        {
            dialogueText.color = Color.white;
        }

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

        if (slideInAnimation == false)
        {
            blackOverlayAnimator.enabled = true;
            blackOverlayAnimator.SetTrigger("FadeIn");
            yield return new WaitForSeconds(1f);
        }
        else
        {
            character1.transform.position = character1.transform.position + Vector3.left * 600;
            character2.transform.position = character2.transform.position + Vector3.left * 600;
            character3.transform.position = character3.transform.position + Vector3.right * 600;
            character4.transform.position = character4.transform.position + Vector3.right * 600;

            LeanTween.move(character1.gameObject, character1.transform.position + Vector3.right * 600, 0.25f);
            LeanTween.move(character2.gameObject, character2.transform.position + Vector3.right * 600, 0.25f);
            LeanTween.move(character3.gameObject, character3.transform.position + Vector3.left * 600, 0.25f);
            LeanTween.move(character4.gameObject, character4.transform.position + Vector3.left * 600, 0.25f);
            yield return new WaitForSeconds(0.25f);
        }


        if (targetDialogue.substituteMusic != "")
        {
            FindObjectOfType<AudioManager>().PlaySound(targetDialogue.substituteMusic);
            FindObjectOfType<AudioManager>().FadeIn(targetDialogue.substituteMusic, 0.2f, 0.6f);
        }
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

        if (targetDialogue.textColors != null && targetDialogue.textColors.Length > dialogueIndex)
        {
            dialogueText.color = targetDialogue.textColors[dialogueIndex];
        }
        else
        {
            dialogueText.color = Color.white;
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
                yield return new WaitForSecondsRealtime(0.05f);
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
        if (targetDialogue.substituteMusic != "")
        {
            FindObjectOfType<AudioManager>().FadeOut(targetDialogue.substituteMusic, 0.2f);
        }

        if (slideInAnimation == false)
        {
            blackOverlayAnimator.enabled = true;
            blackOverlayAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1f);
        }
        else
        {
            dialogueText.transform.parent.gameObject.SetActive(false);
            LeanTween.move(character1.gameObject, character1.transform.position + Vector3.left * 600, 0.25f);
            LeanTween.move(character2.gameObject, character2.transform.position + Vector3.left * 600, 0.25f);
            LeanTween.move(character3.gameObject, character3.transform.position + Vector3.right * 600, 0.25f);
            LeanTween.move(character4.gameObject, character4.transform.position + Vector3.right * 600, 0.25f);
            LeanTween.value(1, 0, 0.5f).setOnUpdate((float val) => dialogueBackground.color = dialogueBackground.color = new Color(1, 1, 1, val));
            yield return new WaitForSeconds(0.25f);
        }

        if (slideInAnimation == false)
        {
            blackOverlayAnimator.SetTrigger("FadeIn");
        }

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

        if(slideInAnimation)
        {
            character1.transform.position += Vector3.right * 600;
            character2.transform.position += Vector3.right * 600;
            character3.transform.position += Vector3.left * 600;
            character4.transform.position += Vector3.left * 600;
        }

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
    }
}
