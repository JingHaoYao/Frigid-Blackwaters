using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHotCannonBallBurn : EnemyStatusEffect {
    public int amountTickDamage;
    [SerializeField] private SpriteRenderer spriteRenderer;
    SpriteRenderer enemySpriteRenderer;

    IEnumerator tickDamage()
    {
        for(int i = 0; i < amountTickDamage; i++)
        {
            yield return new WaitForSeconds(0.6f);
            targetEnemy.dealDamage(1);
        }
        durationFinishedProcedure();
    }

    public override void durationFinishedProcedure()
    {
        StopAllCoroutines();
        LeanTween.alpha(this.gameObject, 0, 0.5f).setOnComplete(()=> { targetEnemy.removeStatus(this); Destroy(this.gameObject); });
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
            spriteRenderer.sortingOrder = enemySpriteRenderer.sortingOrder;
            transform.position = targetEnemy.transform.position + Vector3.up * 0.4f;
            yield return null;
        }
    }
}
