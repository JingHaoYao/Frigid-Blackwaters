using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystallizedSkeletonCrystal : MonoBehaviour
{
    [SerializeField] Collider2D damageCollider;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource shatterAudio;
    public CrystallizedSkeleton crystallizedSkeleton;

    public void shatter()
    {
        crystallizedSkeleton.spawnedCrystals.Remove(this);
        shatterAudio.Play();
        animator.SetTrigger("Shatter");
        Destroy(this.gameObject, 7f / 12f);
        damageCollider.enabled = false;
    }

    private void Start()
    {
        if (crystallizedSkeleton.underFog)
        {
            fadeOut();
        }
    }

    public void fadeOut()
    {
        LeanTween.alpha(this.gameObject, 0, 0.5f);
    }

    public void fadeIn()
    {
        LeanTween.alpha(this.gameObject, 1, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == PlayerProperties.playerShip)
        {
            shatter();
        }
    }
}
