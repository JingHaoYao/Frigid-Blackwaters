﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaFinGroundfire : MonoBehaviour
{
    [SerializeField] Collider2D damagingCollider;
    Coroutine fireLoopInstant;

    private void Start()
    {
        fireLoopInstant = StartCoroutine(FireLoop());
        StartCoroutine(WaitAndFadeOut());
    }

    IEnumerator FireLoop()
    {
        while (true)
        {
            damagingCollider.enabled = true;
            yield return new WaitForSeconds(0.25f);
            damagingCollider.enabled = false;
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator WaitAndFadeOut()
    {
        yield return new WaitForSeconds(0.75f);
        StopCoroutine(fireLoopInstant);
        damagingCollider.enabled = false;
        LeanTween.alpha(this.gameObject, 0, 0.75f).setOnComplete(() => { Destroy(this.gameObject); });
    }
}
