using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityEnemyController : MonoBehaviour
{
    public SpriteRenderer[] renderersToModify;
    List<int> showingRendererTweens = new List<int>();
    List<int> hidingRendererTweens = new List<int>();
    bool isInLight = true;
    Coroutine isInLightRoutine;

    public void FogActivated()
    {
        StopCoroutine(isInLightRoutine);
        hideRenderers();
        isInLight = false;
    }

    public void FogDeActivated()
    {
        showRenderers();
        isInLightRoutine = StartCoroutine(isInLightActive());
    }

    private void Start()
    {
        isInLightRoutine = StartCoroutine(isInLightActive());
    }

    IEnumerator isInLightActive()
    {
        while (true)
        {
            isInLight = true;
            yield return null;
        }
    }

    public void hideRenderers()
    {
        cancelAllCurrentTweens();
        foreach (SpriteRenderer spriteRenderer in renderersToModify)
        {
            spriteRenderer.color = Color.white;
            int tween = LeanTween.color(spriteRenderer.gameObject, new Color(1, 1, 1, 0), 0.75f).setOnComplete(removeHidingRendererTweens).id;
            hidingRendererTweens.Add(tween);
        }
    }

    public void hideRendererAfterHit()
    {
        if (!isInLight)
        {
            hideRenderers();
        }
    }

    public void showRenderers()
    {
        cancelAllCurrentTweens();
        foreach (SpriteRenderer spriteRenderer in renderersToModify)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
            int tween = LeanTween.color(spriteRenderer.gameObject, new Color(1, 1, 1, 1), 0.75f).setOnComplete(removeShowRendererTweens).id;
            showingRendererTweens.Add(tween);
        }
    }

    void removeShowRendererTweens()
    {
        int[] rendererIds = showingRendererTweens.ToArray();
        foreach(int showingTween in rendererIds)
        {
            showingRendererTweens.Remove(showingTween);
        }
    }

    void removeHidingRendererTweens()
    {
        int[] rendererIds = hidingRendererTweens.ToArray();
        foreach (int hidingTweens in rendererIds)
        {
            hidingRendererTweens.Remove(hidingTweens);
        }
    }

    public void cancelAllCurrentTweens()
    {
        foreach (int hidingTween in hidingRendererTweens)
        {
            LeanTween.cancel(hidingTween);
        }

        foreach (int showingTween in showingRendererTweens)
        {
            LeanTween.cancel(showingTween);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInLight)
        {
            showRenderers();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isInLight = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(waitUntilEndOfFrame());
    }

    IEnumerator waitUntilEndOfFrame()
    {
        isInLight = false;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (isInLight == false)
        {
            hideRenderers();
        }
    }

    public bool isUnderLight
    {
        get
        {
            return this.isInLight;
        }
    }
}
