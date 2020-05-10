using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeInfectionEffect : EnemyStatusEffect
{
    [SerializeField] ParticleSystemRenderer rend;
    public GameObject sporeExplosion;

    public override void durationFinishedProcedure()
    {
        StopAllCoroutines();
        targetEnemy.removeStatus(this);
        Destroy(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(infectionDamage());
        StartCoroutine(followEnemy());
    }

    IEnumerator infectionDamage()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.5f);
            targetEnemy.dealDamage(1);
        }
        Instantiate(sporeExplosion, transform.position, Quaternion.identity);
        durationFinishedProcedure();
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
