using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWidgetMenu : MonoBehaviour
{
    [SerializeField] RectTransform highlightBoxTransform;
    [SerializeField] MenuTutorialBox menuTutorialBox;
    [SerializeField] GameObject rayCastBlocker;

    List<TutorialEntry> tutorialEntriesToPlay;
    int currentTutorialEntryIndex;
    Coroutine updateLoop;

    bool tutorialPlaying = false;

    public void Initialize(List<TutorialEntry> tutorialEntries)
    {
        this.tutorialEntriesToPlay = tutorialEntries;
        rayCastBlocker.SetActive(true);
        menuTutorialBox.gameObject.SetActive(true);
        highlightBoxTransform.gameObject.SetActive(true);
        currentTutorialEntryIndex = 0;
        menuTutorialBox.SetTutorialDialogue(tutorialEntriesToPlay[currentTutorialEntryIndex].whichCharacter, tutorialEntriesToPlay[currentTutorialEntryIndex].localPosition, tutorialEntriesToPlay[currentTutorialEntryIndex].dialogueText);
        highlightBoxTransform.sizeDelta = tutorialEntriesToPlay[currentTutorialEntryIndex].highlightBoxDimensions;
        highlightBoxTransform.localPosition = tutorialEntriesToPlay[currentTutorialEntryIndex].highlightBoxPosition;
        tutorialPlaying = true;
        highlightBoxTransform.gameObject.SetActive(!tutorialEntriesToPlay[currentTutorialEntryIndex].hideHighlightBox);
        tutorialEntriesToPlay[currentTutorialEntryIndex].stepAction?.Invoke();
        updateLoop = StartCoroutine(progressLoop());
    }

    IEnumerator progressLoop()
    {
        while(true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                ProgressToNextTutorialPage();
            }
            yield return null;
        }
    }

    public void CloseTutorial()
    {
        if (tutorialPlaying)
        {
            rayCastBlocker.SetActive(false);
            menuTutorialBox.gameObject.SetActive(false);
            highlightBoxTransform.gameObject.SetActive(false);
            tutorialPlaying = false;
            StopCoroutine(updateLoop);
        }
    }

    public void ProgressToNextTutorialPage()
    {
        currentTutorialEntryIndex++;
        if (currentTutorialEntryIndex < tutorialEntriesToPlay.Count)
        {
            menuTutorialBox.SetTutorialDialogue(tutorialEntriesToPlay[currentTutorialEntryIndex].whichCharacter, tutorialEntriesToPlay[currentTutorialEntryIndex].localPosition, tutorialEntriesToPlay[currentTutorialEntryIndex].dialogueText);
            highlightBoxTransform.sizeDelta = tutorialEntriesToPlay[currentTutorialEntryIndex].highlightBoxDimensions;
            highlightBoxTransform.localPosition = tutorialEntriesToPlay[currentTutorialEntryIndex].highlightBoxPosition;
            highlightBoxTransform.gameObject.SetActive(!tutorialEntriesToPlay[currentTutorialEntryIndex].hideHighlightBox);
            tutorialEntriesToPlay[currentTutorialEntryIndex].stepAction?.Invoke();
        }
        else
        {
            CloseTutorial();
        }
        PlayerProperties.audioManager.PlaySound("Generic Button Click");
    }

    private void Awake()
    {
        PlayerProperties.tutorialWidgetMenu = this;
    }

    bool IsTutorialPlaying()
    {
        return tutorialPlaying;
    }
}
