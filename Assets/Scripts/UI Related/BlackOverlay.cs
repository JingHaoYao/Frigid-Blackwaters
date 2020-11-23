
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackOverlay : MonoBehaviour
{
    Image image;
    Animator animator;

    private bool transitioning = false;

    void Start()
    {
        image = this.GetComponent<Image>();
        animator = GetComponent<Animator>();
        image.enabled = false;
    }

    IEnumerator fadeOutfadeIn()
    {
        transitioning = true;
        image.enabled = true;
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        image.enabled = false;
        transitioning = false;
    }

    public void transition()
    {
        if (!transitioning)
        {
            StartCoroutine(fadeOutfadeIn());
        }
    }
}
