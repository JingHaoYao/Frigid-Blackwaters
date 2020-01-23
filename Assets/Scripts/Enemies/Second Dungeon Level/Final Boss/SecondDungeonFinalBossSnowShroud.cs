using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDungeonFinalBossSnowShroud : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource snowStormLoop;
    public SpriteRenderer bossRenderer;
    bool middleOfTransition = false;
    bool shroudActive = false;

    public bool IsActive()
    {
        return shroudActive;
    }

    private void Start()
    {
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
    }

    IEnumerator updateSpriteRenderer()
    {
        while (true)
        {
            spriteRenderer.sortingOrder = bossRenderer.sortingOrder + 5;
            yield return null;
        }
    }

    public void fadeIn()
    {
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
        shroudActive = true;
        animator.SetTrigger("Appear");
        StartCoroutine(updateSpriteRenderer());
        LeanTween.value(0, 1, 0.5f).setOnUpdate((float val) => { snowStormLoop.volume = val; }).setOnStart(() => snowStormLoop.Play());
    }

    public void fadeOut()
    {
        if (middleOfTransition == false)
        {
            shroudActive = false;
            middleOfTransition = true;
            animator.SetTrigger("Dissipate");
            LeanTween.value(1, 0, 0.5f).setOnUpdate((float val) => { snowStormLoop.volume = val; })
                .setOnComplete(() =>
                {
                    snowStormLoop.Stop();
                    spriteRenderer.enabled = false;
                    boxCollider2D.enabled = false;
                    StopCoroutine(updateSpriteRenderer());
                    middleOfTransition = false;
                });
        }
    }
}
