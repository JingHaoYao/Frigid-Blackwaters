using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour
{
    public Animator blackScreenAnimator;
    bool inFadeAnim = false;
    int cutSceneIndex = 0;
    public Text[] texts;
    public Image[] runeImages;

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
        for(int i = 0; i < texts.Length; i++)
        {
            if(i == 0)
            {
                LeanTween.alpha(runeImages[0].GetComponent<RectTransform>(), 1, 1f).setEaseOutCirc();
                yield return new WaitForSeconds(2f);
            }

            LeanTween.alphaText(texts[i].GetComponent<RectTransform>(), 1, 1f).setEaseOutCirc();
            yield return new WaitForSeconds(2f);
            if(i == 4)
            {
                LeanTween.alpha(runeImages[1].GetComponent<RectTransform>(), 1, 1f).setEaseOutCirc();
                LeanTween.alpha(runeImages[2].GetComponent<RectTransform>(), 1, 1f).setEaseOutCirc();
                LeanTween.alpha(runeImages[3].GetComponent<RectTransform>(), 1, 1f).setEaseOutCirc();
                yield return new WaitForSeconds(2f);
            }

            if(i == 8)
            {
                LeanTween.alpha(runeImages[4].GetComponent<RectTransform>(), 1, 1f).setEaseOutCirc();
                yield return new WaitForSeconds(4f);
            }
        }

        if (inFadeAnim == false)
        {
            StartCoroutine(cutSceneEnd());
        }
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
