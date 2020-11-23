using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SadMachineProjectile : MonoBehaviour
{
    [SerializeField] Animator animator;
    private float speed;

    public void Initialize(Enemy targetEnemy, float initialSpeed)
    {
        speed = initialSpeed;
        StartCoroutine(followAndDealDamage(targetEnemy));
    }

    IEnumerator followAndDealDamage(Enemy targetEnemy)
    {
        while(Vector2.Distance(transform.position, targetEnemy.transform.position) > 0.5f)
        {
            if(!EnemyPool.enemyPool.Contains(targetEnemy))
            {
                break;
            }
            transform.position += (targetEnemy.transform.position - transform.position).normalized * Time.deltaTime * speed;
            speed += Time.deltaTime;
            yield return null;
        }

        if (EnemyPool.enemyPool.Contains(targetEnemy))
        {
            targetEnemy.dealDamage(6);
        }

        animator.SetTrigger("Explode");
        yield return new WaitForSeconds(0.333f);
        Destroy(this.gameObject);
    }
}
