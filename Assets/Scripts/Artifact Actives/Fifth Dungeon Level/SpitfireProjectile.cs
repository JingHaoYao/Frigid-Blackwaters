using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitfireProjectile : MonoBehaviour
{
    Enemy targetEnemy;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    private float speed = 4;

    public void Initialize(Enemy targetEnemy)
    {
        this.targetEnemy = targetEnemy;
        StartCoroutine(mainFollowLoop());
    }

    IEnumerator mainFollowLoop()
    {
        while(Vector2.Distance(targetEnemy.transform.position, transform.position) > 0.5f)
        {
            Vector3 travelVector = (targetEnemy.transform.position - transform.position).normalized;
            transform.position += travelVector * Time.deltaTime * speed;
            speed += Time.deltaTime * 4;
            float angle = Mathf.Atan2(travelVector.y, travelVector.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            if (!EnemyPool.enemyPool.Contains(targetEnemy))
            {
                ImpactProcedure();
                StopAllCoroutines();
            }

            yield return null;
        }

        targetEnemy.dealDamage(4);

        ImpactProcedure();
    }
    
    void ImpactProcedure()
    {
        animator.SetTrigger("Impact");
        audioSource.Play();

        Destroy(this.gameObject, 5 / 12f);
    }
}
