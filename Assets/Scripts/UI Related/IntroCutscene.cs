using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour
{
    public Animator blackScreenAnimator;
    [SerializeField] Image backgroundImage;
    [SerializeField] Text cutSceneText;

    [SerializeField] List<Sprite> backgroundSprites;
    [SerializeField] List<string> dialogues;

    bool inFadeAnim = false;
    bool isTypeAnimating = false;
    Coroutine textTypingAnimation;

    IEnumerator cutSceneWait(float duration)
    {
        blackScreenAnimator.SetTrigger("FadeOut");
        inFadeAnim = true;
        yield return new WaitForSeconds(duration);
        inFadeAnim = false;
        blackScreenAnimator.SetTrigger("FadeIn");
    }

    IEnumerator transitionCutScene()
    {
        blackScreenAnimator.SetTrigger("FadeIn");

        int currentDialogueIndex = 0;

        backgroundImage.sprite = backgroundSprites[0];
        cutSceneText.text = dialogues[0];
        textTypingAnimation = StartCoroutine(animateText(dialogues[currentDialogueIndex].Replace("NEWLINE", "\n")));

        while(true)
        {
            if (Input.GetKeyDown(KeyCode.Space) && inFadeAnim == false)
            {
                if (isTypeAnimating)
                {
                    StopCoroutine(textTypingAnimation);
                    cutSceneText.text = dialogues[currentDialogueIndex].Replace("NEWLINE", "\n");
                    isTypeAnimating = false;
                }
                else
                {

                    currentDialogueIndex++;

                    if (currentDialogueIndex >= backgroundSprites.Count)
                    {
                        break;
                    }

                    if (currentDialogueIndex > 0 && backgroundSprites[currentDialogueIndex] == backgroundSprites[currentDialogueIndex - 1])
                    {
                        backgroundImage.sprite = backgroundSprites[currentDialogueIndex];
                        cutSceneText.text = dialogues[currentDialogueIndex];
                        textTypingAnimation = StartCoroutine(animateText(dialogues[currentDialogueIndex].Replace("NEWLINE", "\n")));
                    }
                    else
                    {
                        inFadeAnim = true;
                        blackScreenAnimator.SetTrigger("FadeOut");

                        yield return new WaitForSeconds(1f);

                        backgroundImage.sprite = backgroundSprites[currentDialogueIndex];
                        cutSceneText.text = dialogues[currentDialogueIndex];
                        textTypingAnimation = StartCoroutine(animateText(dialogues[currentDialogueIndex].Replace("NEWLINE", "\n")));
                        inFadeAnim = false;

                        blackScreenAnimator.SetTrigger("FadeIn");
                    }
                }
            }

            yield return null;
        }

        if (inFadeAnim == false)
        {
            StartCoroutine(cutSceneEnd());
        }
    }

    IEnumerator animateText(string dialogue)
    {
        isTypeAnimating = true;
        int charIndex = 0;
        foreach (char c in dialogue)
        {
            charIndex++;
            cutSceneText.text = dialogue.Substring(0, charIndex);
            if (c != ' ')
            {
                yield return new WaitForSecondsRealtime(0.05f);
            }
        }
        isTypeAnimating = false;
    }

    IEnumerator cutSceneEnd()
    {
        StartCoroutine(cutSceneWait(1f));
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(transitionCutScene());
    }

    void Update()
    {
        if(inFadeAnim == false && Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(cutSceneEnd());
        }
    }
}
