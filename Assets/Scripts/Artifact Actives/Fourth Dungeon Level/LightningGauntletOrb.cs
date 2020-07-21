using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningGauntletOrb : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void attackEnemy(Enemy enemy)
    {
        StartCoroutine(moveTowardsTarget(enemy));
    }

    IEnumerator moveTowardsTarget(Enemy enemy)
    {
        float speed = 6;
        Vector3 targetPosition = enemy.transform.position;
        while (enemy != null && Vector2.Distance(transform.position, enemy.transform.position) > 1)
        {
            speed += Time.deltaTime * 2;
            targetPosition = enemy.transform.position;
            transform.position += (targetPosition - transform.position) * Time.deltaTime * speed;
            yield return null;
        }

        Destroy(this.gameObject, 4 / 12f);
        animator.SetTrigger("Explode");

        if(enemy != null)
        {
            enemy.dealDamage(4);
        }
    }
}
