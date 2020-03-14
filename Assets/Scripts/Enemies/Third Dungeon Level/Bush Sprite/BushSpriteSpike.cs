using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushSpriteSpike : MonoBehaviour
{
    [SerializeField] private CircleCollider2D circCol;
    [SerializeField] private Animator animator;
    public float waitDuration;

    private void Start()
    {
        StartCoroutine(delayUntilSink(this.waitDuration));
    }

    IEnumerator delayUntilSink(float waitTime)
    {
        circCol.enabled = false;
        yield return new WaitForSeconds(3f / 12f);
        circCol.enabled = true;
        yield return new WaitForSeconds(4 / 12f);
        yield return new WaitForSeconds(waitTime);
        circCol.enabled = false;
        animator.SetTrigger("Sink");
        Destroy(this.gameObject, 7f / 12f);
    }
}
