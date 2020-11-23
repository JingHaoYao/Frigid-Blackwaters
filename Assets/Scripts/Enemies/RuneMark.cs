using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneMark : MonoBehaviour
{
    [SerializeField] List<Sprite> runeSprites;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color customColor;

    private void Start()
    {
        spriteRenderer.color = new Color(customColor.r, customColor.g, customColor.b, 0);
        LeanTween.alpha(this.gameObject, 1, 0.25f);
        spriteRenderer.sprite = runeSprites[Random.Range(0, runeSprites.Count)];
    }
}
