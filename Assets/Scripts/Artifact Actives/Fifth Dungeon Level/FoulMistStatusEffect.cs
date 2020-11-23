using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoulMistStatusEffect : EnemyStatusEffect
{
    [SerializeField] ParticleSystemRenderer rend;

    public override void durationFinishedProcedure()
    {
        StopAllCoroutines();
        targetEnemy.removeStatus(this);
        Destroy(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(followEnemy());
        targetEnemy.dealDamage(Mathf.RoundToInt(targetEnemy.maxHealth * 0.25f));
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
}
