using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallRunicFire : EnemyStatusEffect {
    [SerializeField] private SpriteRenderer spriteRenderer;
    SpriteRenderer enemySpriteRenderer;

    IEnumerator tickDamage()
    {
        yield return new WaitForSeconds(1f);
        targetEnemy.dealDamage(1);
        durationFinishedProcedure();
    }

    public override void durationFinishedProcedure()
    {
        StopAllCoroutines();
        LeanTween.alpha(this.gameObject, 0, 0.5f).setOnComplete(() => { Destroy(this.gameObject); });
    }

    void Start()
    {
        StartCoroutine(tickDamage());
        enemySpriteRenderer = targetEnemy.GetComponent<SpriteRenderer>();
        StartCoroutine(spriteRenderAdjustment());
    }

    IEnumerator spriteRenderAdjustment()
    {
        while (true)
        {
            transform.position = targetEnemy.transform.position + Vector3.up * 0.4f;
            spriteRenderer.sortingOrder = enemySpriteRenderer.sortingOrder;
            yield return null;
        }
    }
}
