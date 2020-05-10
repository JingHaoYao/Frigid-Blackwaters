using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomStatusEffect : EnemyStatusEffect
{
    public GameObject healParticles;
    [SerializeField] ParticleSystemRenderer particleSystemRenderer;
    [SerializeField] ParticleSystem particleSystem;
    private SpriteRenderer enemySpriteRenderer;

    public override void durationFinishedProcedure()
    {
        StopCoroutine(followEnemy());
        particleSystem.loop = false;
        targetEnemy.removeStatus(this);
        Destroy(this.gameObject, 0.5f);
    }

    private void Start()
    {
        buffEnemy();
        enemySpriteRenderer = targetEnemy.GetComponent<SpriteRenderer>();
        StartCoroutine(followEnemy());
    }

    void buffEnemy()
    {
        targetEnemy.updateSpeed(targetEnemy.speed + 2);
        targetEnemy.heal(targetEnemy.maxHealth / 2);
        GameObject healParticlesInstant = Instantiate(healParticles, targetEnemy.transform.position, Quaternion.identity);
        healParticlesInstant.GetComponent<FollowObject>().objectToFollow = targetEnemy.gameObject;
    }

    IEnumerator followEnemy()
    {
        while (true)
        {
            transform.position = targetEnemy.transform.position;
            particleSystemRenderer.sortingOrder = enemySpriteRenderer.sortingOrder + 2;
            yield return null;
        }
    }
}
