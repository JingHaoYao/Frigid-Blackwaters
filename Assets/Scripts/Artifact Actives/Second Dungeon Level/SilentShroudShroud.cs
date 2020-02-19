using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilentShroudShroud : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource snowStormLoop;

    private void Start()
    {
        fadeIn();
    }

    IEnumerator updateSpriteRenderer()
    {
        while (true)
        {
            transform.position = PlayerProperties.playerShipPosition + new Vector3(0, -1.2f, 0);
            spriteRenderer.sortingOrder = PlayerProperties.spriteRenderer.sortingOrder + 5;
            yield return null;
        }
    }

    public void fadeIn()
    {
        animator.SetTrigger("Appear");
        StartCoroutine(updateSpriteRenderer());
        LeanTween.value(0, 1, 0.5f).setOnUpdate((float val) => { snowStormLoop.volume = val; }).setOnStart(() => snowStormLoop.Play());
    }

    public void fadeOut()
    {
        LeanTween.value(1, 0, 0.5f).setOnUpdate((float val) => { snowStormLoop.volume = val; }).setOnComplete(() => { snowStormLoop.Stop(); });
        LeanTween.alpha(this.gameObject, 0, 1f).setOnComplete(() => { Destroy(this.gameObject); });
    }
}
