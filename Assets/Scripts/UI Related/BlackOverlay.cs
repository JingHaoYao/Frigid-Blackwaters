using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackOverlay : MonoBehaviour
{
    Image image;
    Animator animator;
    void Start()
    {
        image = this.GetComponent<Image>();
        animator = GetComponent<Animator>();
        image.enabled = false;
    }

    IEnumerator fadeOutfadeIn()
    {
        image.enabled = true;
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        image.enabled = false;
    }

    public void transition()
    {
        StartCoroutine(fadeOutfadeIn());
    }
}
