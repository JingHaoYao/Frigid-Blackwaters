using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityEnemyController : MonoBehaviour
{
    public SpriteRenderer[] renderersToModify;
    List<Coroutine> showingRendererTweens = new List<Coroutine>();
    List<Coroutine> hidingRendererTweens = new List<Coroutine>();
    bool isInLight = true;
    Coroutine isInLightRoutine;

    public void FogActivated()
    {
        StopCoroutine(isInLightRoutine);
        StartCoroutine(waitUntilEndOfFrame());
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
            Coroutine tween = StartCoroutine(fadeInAlpha(0.75f, spriteRenderer));
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

    IEnumerator fadeOutAlpha(float duration, SpriteRenderer spriteRenderer)
    {
        float increment = 1 / duration;
        float timer = 0;

        spriteRenderer.color = new Color(1, 1, 1, 0);

        while(timer < duration)
        {
            timer += Time.deltaTime;
            spriteRenderer.color = new Color(1, 1, 1, timer * increment);
            yield return null;
        }

        spriteRenderer.color = Color.white;
    }

    IEnumerator fadeInAlpha(float duration, SpriteRenderer spriteRenderer)
    {
        float increment = 1 / duration;
        float timer = duration;

        spriteRenderer.color = new Color(1, 1, 1, 1);

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            spriteRenderer.color = new Color(1, 1, 1, timer * increment);
            yield return null;
        }

        spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    public void showRenderers()
    {
        cancelAllCurrentTweens();
        foreach (SpriteRenderer spriteRenderer in renderersToModify)
        {
            Coroutine tween = StartCoroutine(fadeOutAlpha(0.75f, spriteRenderer));
            showingRendererTweens.Add(tween);
        }
    }

    public void cancelAllCurrentTweens()
    {
        foreach (Coroutine hidingTween in hidingRendererTweens)
        {
            StopCoroutine(hidingTween);
        }

        hidingRendererTweens.Clear();

        foreach (Coroutine showingTween in showingRendererTweens)
        {
            StopCoroutine(showingTween);
        }

        showingRendererTweens.Clear();
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
        yield return new WaitForSeconds(0.1f);
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
