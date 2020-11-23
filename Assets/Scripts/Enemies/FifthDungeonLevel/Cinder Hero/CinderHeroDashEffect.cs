using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinderHeroDashEffect : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] viewSprites;

    public void Initialize(Vector3 position, int whatView, int mirror)
    {
        transform.position = position;
        this.gameObject.SetActive(true);
        spriteRenderer.sprite = viewSprites[whatView - 1];
        transform.localScale = new Vector3(5 * mirror, 5);
        Color spriteColor = spriteRenderer.color;
        spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0.2941f);
        LeanTween.alpha(this.gameObject, 0, 0.75f).setOnComplete(() => { this.gameObject.SetActive(false); });
    }
}
