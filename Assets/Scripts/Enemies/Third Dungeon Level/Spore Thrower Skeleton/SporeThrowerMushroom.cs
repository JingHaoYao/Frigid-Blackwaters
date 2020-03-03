using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeThrowerMushroom : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D collider;
    bool isEnding = false;

    private void Start()
    {
        StartCoroutine(waitUntilEndProcedure());
    }

    IEnumerator waitUntilEndProcedure()
    {
        yield return new WaitForSeconds(3.5f);
        endProcedure();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            endProcedure();
        }
    }

    void endProcedure()
    {
        if (isEnding == false)
        {
            collider.enabled = false;
            animator.SetTrigger("Sink");
            Destroy(this.gameObject, 7 / 12f);
            isEnding = true;
        }
    }
}
