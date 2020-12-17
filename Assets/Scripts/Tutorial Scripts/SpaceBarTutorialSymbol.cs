using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBarTutorialSymbol : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite sprite1, sprite2;

    IEnumerator flipSprite()
    {
        while(true)
        {
            spriteRenderer.sprite = sprite1;
            yield return new WaitForSecondsRealtime(0.25f);
            spriteRenderer.sprite = sprite2;
            yield return new WaitForSecondsRealtime(0.25f);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(flipSprite());
    }
}
