using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunicDrillProjectile : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Collider2D damageCollider;
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(drillProcedure());
    }

    IEnumerator drillProcedure()
    {
        yield return new WaitForSeconds(5 / 12f);
        audioSource.Play();
        for (int i = 0; i < 15; i++)
        {
            damageCollider.enabled = true;
            yield return new WaitForSeconds(0.1f);
            damageCollider.enabled = false;
            yield return new WaitForSeconds(0.1f);
        }
        audioSource.Stop();
        animator.SetTrigger("Sink");
        yield return new WaitForSeconds(5 / 12f);
        Destroy(this.gameObject);
    }
}
