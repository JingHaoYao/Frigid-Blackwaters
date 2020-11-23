using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SadMachineInstant : MonoBehaviour
{
    [SerializeField] GameObject lightningProjectile;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;

    float speed = 4;

    private void Start()
    {
        StartCoroutine(followLoop());
        StartCoroutine(attackLoop());
    }

    IEnumerator followLoop()
    {
        while(true)
        {
            if(Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 1f)
            {
                transform.position += Time.deltaTime * (PlayerProperties.playerShipPosition - transform.position).normalized * speed;
                speed += Time.deltaTime;
            }
            else
            {
                speed = 4;
            }

            yield return null;
        }
    }

    IEnumerator attackLoop()
    {
        while(true)
        {
            yield return new WaitForSeconds(3f);

            if(EnemyPool.enemyPool.Count <= 0)
            {
                continue;
            }

            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(4 / 12f);
            audioSource.Play();
            float minDistance = float.MaxValue;
            Enemy targetEnemy = null;
            foreach(Enemy enemy in EnemyPool.enemyPool)
            {
                if(Vector2.Distance(enemy.transform.position, transform.position) < minDistance)
                {
                    minDistance = Vector2.Distance(enemy.transform.position, transform.position);
                    targetEnemy = enemy;
                }
            }

            GameObject lightningProjectileInstant = Instantiate(lightningProjectile, transform.position - Vector3.up * 0.5f, Quaternion.identity);
            lightningProjectileInstant.GetComponent<SadMachineProjectile>().Initialize(targetEnemy, 8);

            yield return new WaitForSeconds(3 / 12f);
            animator.SetTrigger("Idle");
        }
    }
}
