using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : EnemyStatusEffect
{
    [SerializeField] ParticleSystemRenderer rend;
    [SerializeField] int slowAmount;

    public override void durationFinishedProcedure()
    {
        targetEnemy.updateSpeed(targetEnemy.speed + slowAmount);
        StopAllCoroutines();
        targetEnemy.removeStatus(this);
        Destroy(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(waitForSeconds(duration));
        StartCoroutine(followEnemy());
        targetEnemy.updateSpeed(targetEnemy.speed - slowAmount);
    }

    IEnumerator followEnemy()
    {
        SpriteRenderer enemySpriteRenderer;
        enemySpriteRenderer = targetEnemy.GetComponent<SpriteRenderer>();
        while (true)
        {
            transform.position = targetEnemy.transform.position;
            this.rend.sortingOrder = enemySpriteRenderer.sortingOrder + 2;
            yield return null;
        }
    }

    IEnumerator waitForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        durationFinishedProcedure();
    }
}
