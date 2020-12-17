using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSwitch : MonoBehaviour
{
    [SerializeField] NewTutorialManager newTutorialManager;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource triggerAudio, resetAudio;
    bool shouldTrigger = true;
    bool closed = false;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite unactivatedSprite;

    IEnumerator resetTrigger()
    {
        yield return new WaitForSeconds(1f);
        if (!closed)
        {
            newTutorialManager.UnTriggerDoor();
            animator.SetTrigger("Resume");
            shouldTrigger = true;
            resetAudio.Play();
        }
    }

    public void CloseTrigger()
    {
        closed = true;
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shouldTrigger && !closed && collision.gameObject.layer == 16)
        {
            triggerAudio.Play();
            animator.SetTrigger("Trigger");
            newTutorialManager.TriggerDoor();
            StartCoroutine(resetTrigger());
            shouldTrigger = false;
        }
    }
}
