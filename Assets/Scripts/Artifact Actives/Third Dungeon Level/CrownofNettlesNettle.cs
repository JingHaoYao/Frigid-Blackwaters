using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownofNettlesNettle : MonoBehaviour
{
    [SerializeField] private Collider2D collider;
    [SerializeField] private Animator animator;

    private void Start()
    {
        collider.enabled = false;
        StartCoroutine(nettleCycle());
    }

    IEnumerator nettleCycle()
    {
        yield return new WaitForSeconds(0.75f);
        collider.enabled = true;
        animator.SetTrigger("Sink");
        yield return new WaitForSeconds(0.2f);
        collider.enabled = false;
        yield return new WaitForSeconds(0.75f);
        Destroy(this.gameObject);
    }
}
