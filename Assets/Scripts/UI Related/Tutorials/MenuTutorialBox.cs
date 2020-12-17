using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTutorialBox : MonoBehaviour
{
    [SerializeField] Image characterIcon;
    [SerializeField] Sprite[] characterSprites;
    [SerializeField] Text characterName;
    [SerializeField] Text tutorialText;

    Coroutine textTypingAnimation;


    public void SetTutorialDialogue(TutorialEntry.Characters whichCharacter, Vector3 localPosition, string dialogue)
    {
        transform.localPosition = localPosition;
        tutorialText.text = dialogue;
        characterName.text = whichCharacter.ToString();
        characterIcon.sprite = characterSprites[(int)whichCharacter];

        if(textTypingAnimation != null)
        {
            StopCoroutine(textTypingAnimation);
        }
        textTypingAnimation = StartCoroutine(animateText(dialogue));
    }

    IEnumerator animateText(string text)
    {
        int charIndex = 0;
        foreach (char c in text)
        {
            charIndex++;
            tutorialText.text = text.Substring(0, charIndex);
            if (c != ' ')
            {
                yield return new WaitForSecondsRealtime(0.05f);
            }
        }
        textTypingAnimation = null;
    }
}
