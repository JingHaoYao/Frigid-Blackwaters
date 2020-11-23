using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TremorBurnRadius : MonoBehaviour
{
    [SerializeField] Collider2D collider2D;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] DamageAmount damageAmount;
    bool shouldSlow = false;
    List<Enemy> slowedEnemies = new List<Enemy>();
    List<float> slowAddition = new List<float>();

    public void Initialize(int numberTimes, int sortingOrder, int bonusDamage, bool shouldSlow)
    {
        StartCoroutine(burnRoutine(numberTimes));
        this.spriteRenderer.sortingOrder = sortingOrder;
        this.damageAmount.addDamage(bonusDamage);
        this.shouldSlow = shouldSlow;
    }

    IEnumerator burnRoutine(int numberTimes)
    {
        for (int i = 0; i < numberTimes; i++)
        {
            collider2D.enabled = false;
            animator.SetTrigger("Play");
            yield return new WaitForSeconds(4 / 12f);
            collider2D.enabled = true;
            yield return new WaitForSeconds(1 / 12f);
        }
        for(int i = 0; i < slowedEnemies.Count; i++)
        {
            slowedEnemies[i].updateSpeed(slowAddition[i] + slowedEnemies[i].speed);
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "StrongEnemy" || collision.tag == "MeleeEnemy" || collision.tag == "RangedEnemy" && shouldSlow)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if(enemy != null && !slowedEnemies.Contains(enemy))
            {
                slowAddition.Add(Mathf.Clamp(enemy.speed, 0, 1));
                enemy.updateSpeed(enemy.speed - 1);
                slowedEnemies.Add(enemy);
            }
        }
    }
}
