using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenHeroSwordSlash : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] CircleCollider2D circCol;
    [SerializeField] AudioSource slashAudio;
    [SerializeField] SpriteRenderer spriteRenderer;
    public Sprite baseSprite;

    private void Start()
    {
        LeanTween.alpha(this.gameObject, 1, 0.5f).setOnComplete(() => StartCoroutine(slash()));
    }

    IEnumerator slash()
    {
        animator.SetTrigger("Slash");
        yield return new WaitForSeconds(6 / 12f);
        circCol.enabled = true;
        slashAudio.Play();
        yield return new WaitForSeconds(6 / 12f);
        circCol.enabled = false;
        yield return new WaitForSeconds(1 / 12f);
        animator.enabled = false;
        spriteRenderer.sprite = baseSprite;
        LeanTween.alpha(this.gameObject, 0, 0.5f).setOnComplete(() => Destroy(this.gameObject));
    }
}
