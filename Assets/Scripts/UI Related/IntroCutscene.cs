using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour
{
    Text cutSceneText;
    Image cutSceneImage;
    public Sprite[] cutsceneList;
    public string[] cutsceneText;
    public Animator blackScreenAnimator;
    bool inFadeAnim = false;
    int cutSceneIndex = 0;

    IEnumerator cutSceneWait(float duration)
    {
        inFadeAnim = true;
        yield return new WaitForSeconds(duration);
        inFadeAnim = false;
    }

    IEnumerator transitionCutScene(int index, bool endCutScene)
    {
        StartCoroutine(cutSceneWait(1));
        blackScreenAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
        if (endCutScene == true)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            cutSceneText.text = cutsceneText[index];
            cutSceneImage.sprite = cutsceneList[index];
        }
        blackScreenAnimator.SetTrigger("FadeIn");
    }

    void Start()
    {
        cutSceneImage = GetComponent<Image>();
        cutSceneText = GetComponentInChildren<Text>();
        cutSceneImage.sprite = cutsceneList[0];
        cutSceneText.text = cutsceneText[0];
        blackScreenAnimator.SetTrigger("FadeIn");
        StartCoroutine(cutSceneWait(0.5f));
    }

    void Update()
    {
        if(inFadeAnim == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(cutSceneIndex < cutsceneList.Length)
                {
                    cutSceneIndex++;
                    if(cutSceneIndex == cutsceneList.Length)
                    {
                        StartCoroutine(transitionCutScene(0, true));
                    }
                    else
                    {
                        StartCoroutine(transitionCutScene(cutSceneIndex, false));
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(transitionCutScene(0, true));
            }
        }
    }
}
