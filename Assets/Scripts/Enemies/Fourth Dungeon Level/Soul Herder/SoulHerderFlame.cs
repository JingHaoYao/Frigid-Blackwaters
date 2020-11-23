using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulHerderFlame : MonoBehaviour
{
    [SerializeField] LightAuraController lightAuraController;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Collider2D col;
    
    public void fadeOut()
    {
        lightAuraController.fadeOutLights(0.5f);
        StartCoroutine(fadeOutThisRenderer());
        col.enabled = false;
    }

    IEnumerator fadeOutThisRenderer()
    {
        float alpha = 1;
        while(spriteRenderer.color.a > 0)
        {
            alpha -= Time.deltaTime * 2;
            spriteRenderer.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        Destroy(this.gameObject);
    }
    
}
