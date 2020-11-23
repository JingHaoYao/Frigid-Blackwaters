using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellFishSpike : MonoBehaviour
{
    [SerializeField] Collider2D damageCollider;
    [SerializeField] Animator animator;

    private void Start()
    {
        damageCollider.enabled = false;
        StartCoroutine(colliderEnabledLoop());
    }

    IEnumerator colliderEnabledLoop()
    {
        yield return new WaitForSeconds(0.5f);
        damageCollider.enabled = true;
    }

    public void Sink()
    {
        animator.SetTrigger("Sink");
        damageCollider.enabled = false;
        Destroy(this.gameObject, 0.5f);
    }
}
