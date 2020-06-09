using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogRuneObstacle : MonoBehaviour
{
    [SerializeField] SpriteRenderer runesSpriteRenderer;
    private const float r = 0, g = 0.9171f, b = 1;

    private void Start()
    {
        runesSpriteRenderer.color = new Color(r, g, b, 0);
        pickRendererLayer();
    }

    void pickRendererLayer()
    {
        runesSpriteRenderer.sortingOrder = (200 - (int)((transform.position.y) * 10)) + 2;
    }


    public void glowRunes()
    {
        LeanTween.value(0, 1, 0.75f).setOnUpdate((float val) => runesSpriteRenderer.color = new Color(r, g, b, val));
    }

    public void unGlowRunes()
    {
        LeanTween.value(1, 0, 0.75f).setOnUpdate((float val) => runesSpriteRenderer.color = new Color(r, g, b, val));
    }
}
