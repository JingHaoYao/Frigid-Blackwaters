using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrosiveEffect : EnemyStatusEffect
{
    public int amountTickDamage;
    public bool isScaling = false;
    public bool isSpeedy = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    SpriteRenderer enemySpriteRenderer;

    IEnumerator tickDamage()
    {
        for (int i = 0; i < amountTickDamage; i++)
        {
            yield return new WaitForSeconds(isSpeedy ? 0.3f : 0.6f);
            if (isScaling == true)
            {
                targetEnemy.dealDamage(i + 1);
            }
            else
            {
                targetEnemy.dealDamage(1);
            }
        }
        durationFinishedProcedure();
    }

    public override void durationFinishedProcedure()
    {
        StopAllCoroutines();
        LeanTween.alpha(this.gameObject, 0, 0.5f).setOnComplete(() => { targetEnemy.removeStatus(this); Destroy(this.gameObject); });
    }

    void Start()
    {
        PlayerScript playerScript = PlayerProperties.playerScript;
        amountTickDamage += playerScript.attackBonus + playerScript.conAttackBonus + (PlayerUpgrades.chemicalSprayerUpgrades.Count >= 1 ? 1 : 0);
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

