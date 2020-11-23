using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambleDice : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    void Start()
    {
        StartCoroutine(fadeInFadeOut());
    }

    IEnumerator fadeInFadeOut()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        LeanTween.alpha(this.gameObject, 1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Play");
        yield return new WaitForSeconds(5 / 12f);
        LeanTween.alpha(this.gameObject, 0, 1f);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
