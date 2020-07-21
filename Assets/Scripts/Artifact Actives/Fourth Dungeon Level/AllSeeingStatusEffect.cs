using System.Collections;
using UnityEngine;

public class AllSeeingStatusEffect : EnemyStatusEffect
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AudioSource audioSource;
    Coroutine followLoopRoutine;

    private void Start()
    {
        StartCoroutine(mainLoop());
        followLoopRoutine = StartCoroutine(mainFollowLoop());
    }

    public override void durationFinishedProcedure()
    {
        audioSource.Play();
        StopCoroutine(followLoopRoutine);
        if(targetEnemy != null)
        {
            targetEnemy.dealDamage(2);
        }
        StartCoroutine(closeEye());
    }

    IEnumerator mainLoop()
    {
        animator.SetTrigger("Open");

        yield return new WaitForSeconds(duration);

        durationFinishedProcedure();
    }

    IEnumerator closeEye()
    {
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(8 / 12f);
        Destroy(this.gameObject);
    }

    IEnumerator mainFollowLoop()
    {
        while (true)
        {
            transform.position = targetEnemy.transform.position + Vector3.up;
            yield return null;
        }
    }
}
