using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAuraController : MonoBehaviour
{
    [SerializeField] Vector3 maxScale;
    [SerializeField] Vector3 minScale;
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] float r, g, b, desiredAlpha;

    private void Start()
    {
        flickerUp();
    }

    void flickerUp()
    {
        LeanTween.scale(this.gameObject, maxScale * Random.Range(0.95f, 1f), Random.Range(0.1f, 0.15f)).setOnComplete(flickerDown).setEaseInOutBounce();
    }

    void flickerDown()
    {
        LeanTween.scale(this.gameObject, minScale * Random.Range(0.95f, 1f), Random.Range(0.1f, 0.15f)).setOnComplete(flickerUp).setEaseInOutBounce();
    }

    public void fadeInLights(float duration = 0.1f)
    {
        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = new Color(r, g, b, 0);
            LeanTween.alpha(spriteRenderer.gameObject, desiredAlpha, duration);
        }
    }

    public void fadeOutLights(float duration = 0.1f)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            LeanTween.cancel(spriteRenderer.gameObject);
            LeanTween.alpha(spriteRenderer.gameObject, 0, duration);
        }
    }

    public void SetRenderer(int order)
    {
        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sortingOrder = order;
        }
    }
}
