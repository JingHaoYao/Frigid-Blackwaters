using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiendFlowerBudTurretHead : MonoBehaviour
{
    public GameObject podProjectile;
    [SerializeField] AudioSource fireAudio;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    public SpriteRenderer stemSpriteRenderer;
    public FiendFlowerBudStem stem;

    float attackPeriod = 0;
    Enemy targetEnemy;

    void pickClosestEnemy()
    {
        if(EnemyPool.enemyPool.Count <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(destroyProcedure());
            return;
        }

        float minDistance = float.MaxValue;
        Enemy potentialEnemy = null;
        foreach(Enemy enemy in EnemyPool.enemyPool)
        {
            if(Vector2.Distance(enemy.transform.position, transform.position) < minDistance)
            {
                potentialEnemy = enemy;
            }
        }
        targetEnemy = potentialEnemy;
    }

    IEnumerator destroyProcedure()
    {
        animator.SetTrigger("Shrink");
        yield return new WaitForSeconds(0.333f);
        spriteRenderer.enabled = false;
        stem.destroyStem();
    }

    float angleToEnemy
    {
        get
        {
            return (Mathf.Atan2(targetEnemy.transform.position.y - transform.position.y, targetEnemy.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 360) % 360;
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, angleToEnemy);
        this.spriteRenderer.sortingOrder = stemSpriteRenderer.sortingOrder + 3;
    }

    public void initializeFlowerHead()
    {
        pickClosestEnemy();
        StartCoroutine(spitOutPod());
        spriteRenderer.enabled = true;
    }

    IEnumerator spitOutPod()
    {
        yield return new WaitForSeconds(0.333f);
        for (int i = 0; i < 3; i++)
        {
            animator.SetTrigger("Attack");
            fireAudio.Play();
            yield return new WaitForSeconds(4 / 12f);
            if(targetEnemy == null)
            {
                continue;
            }
            GameObject podInstant = Instantiate(podProjectile, transform.position + new Vector3(Mathf.Cos(angleToEnemy * Mathf.Deg2Rad), Mathf.Sin(angleToEnemy * Mathf.Deg2Rad)) * 1.25f, Quaternion.identity);
            podInstant.GetComponent<BasicProjectile>().angleTravel = angleToEnemy;
            pickClosestEnemy();
            yield return new WaitForSeconds(1f);
        }
        animator.SetTrigger("Shrink");
        yield return new WaitForSeconds(0.333f);
        spriteRenderer.enabled = false;
        stem.destroyStem();
    }
}
