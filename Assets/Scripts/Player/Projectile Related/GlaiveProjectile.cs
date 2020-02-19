using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlaiveProjectile : PlayerProjectile
{
    [SerializeField] CircleCollider2D damagingCollider;
    [SerializeField] DamageAmount damageAmount;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] Animator animator;
    public float distanceTravel;
    public bool fastGlaive;
    public bool bouncingGlaive;
    public bool spittingGlaive;
    public GameObject splitGlaive;
    public GameObject effectGlaive;

    private float directionTravelling;
    private Vector3 target;
    private Vector3 originalSpawnPosition;

    private bool startedBouncing = false;
    public int maxNumberBounces;
    private int currentNumberBounces = 0;

    public float glaiveSpawnDuration = 0.3f;

    IEnumerator pulseAnim()
    {
        animator.SetTrigger("Pulse");
        yield return new WaitForSeconds(0.583f / 1.5f);
    }

    private void Start()
    {
        directionTravelling = pickDirectionTravel();
        triggerWeaponFireFlag();
        originalSpawnPosition = transform.position;
        target = transform.position + new Vector3(Mathf.Cos(directionTravelling * Mathf.Deg2Rad), Mathf.Sin(directionTravelling * Mathf.Deg2Rad)) * distanceTravel;
        StartCoroutine(spinDamage());
        if (spittingGlaive)
        {
            StartCoroutine(spawnGlaives());
        }
        else
        {
            StartCoroutine(spawnEffectGlaives());
        }
        
        if(PlayerUpgrades.glaiveLauncherUpgrades.Count >= 1)
        {
            damageAmount.originDamage += 1;
            damageAmount.updateDamage();
        }

        StartCoroutine(travelToTarget());
    }

    private float pickDirectionTravel()
    {
        Vector3 cursorPosition = PlayerProperties.cursorPosition;
        return (360 + Mathf.Atan2(cursorPosition.y - transform.position.y, cursorPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    IEnumerator travelToTarget()
    {
        float speed = 8 * (1 - (Vector2.Distance(transform.position, originalSpawnPosition) / distanceTravel));
        while (Vector2.Distance(target, transform.position) > 0.2f)
        {
            rigidBody2D.velocity = (target - transform.position).normalized * speed;
            yield return null;
        }

        if (startedBouncing == false)
        {
            StartCoroutine(returnProcedure());
        }
    }

    IEnumerator bouncingOperation(Enemy enemyToTarget)
    {
        while (EnemyPool.enemyPool.Contains(enemyToTarget) && Vector2.Distance(transform.position, enemyToTarget.transform.position) > 0.1f)
        {
            rigidBody2D.velocity = (enemyToTarget.transform.position - transform.position).normalized * 8;
            yield return null;  
        }
        pickBounceTarget(enemyToTarget);
    }

    void pickBounceTarget(Enemy currentEnemy)
    {
        if (currentNumberBounces < maxNumberBounces)
        {
            StartCoroutine(pulseAnim());
            currentNumberBounces++;
            float smallestDistance = float.MaxValue;
            Enemy enemyToTarget = null;
            foreach (Enemy enemy in EnemyPool.enemyPool)
            {
                if (enemy != currentEnemy && Vector2.Distance(enemy.transform.position, transform.position) < smallestDistance && Vector2.Distance(enemy.transform.position, transform.position) < 4)
                {
                    smallestDistance = Vector2.Distance(enemy.transform.position, transform.position);
                    enemyToTarget = enemy;
                }
            }

            if (enemyToTarget != null)
            {
                StopCoroutine(returnProcedure());
                StartCoroutine(bouncingOperation(enemyToTarget));
            }
            else
            {
                StartCoroutine(returnProcedure());
            }
        }
        else
        {
            StartCoroutine(returnProcedure());
        }
    }

    IEnumerator returnProcedure()
    {
        StopCoroutine(travelToTarget());
        float speed = 0;
        while (Vector2.Distance(PlayerProperties.playerShipPosition, this.transform.position) > 1f)
        {
            rigidBody2D.velocity = speed * (PlayerProperties.playerShipPosition - transform.position).normalized;
            speed += Time.deltaTime * 5;
            yield return null;
        }
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            if (bouncingGlaive == true && startedBouncing == false)
            {
                StopCoroutine(travelToTarget());
                Enemy enemyHit = collision.gameObject.GetComponent<Enemy>();
                pickBounceTarget(enemyHit);
                startedBouncing = true;
            }
        }
    }

    IEnumerator spawnGlaives()
    {
        while (true)
        {
            yield return new WaitForSeconds(glaiveSpawnDuration);
            Instantiate(splitGlaive, transform.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z));
        }
    }

    IEnumerator spawnEffectGlaives()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.15f);
            Instantiate(effectGlaive, transform.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z));
        }
    }

    IEnumerator spinDamage()
    {
        while (true)
        {
            LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 180, fastGlaive ? 0.05f : 0.1f).setOnComplete(() => { LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 180, fastGlaive ? 0.05f : 0.1f); });
            yield return new WaitForSeconds(fastGlaive ? 0.1f : 0.2f);
            LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 180, fastGlaive ? 0.05f : 0.1f).setOnComplete(() => { LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 180, fastGlaive ? 0.05f : 0.1f); }); ;
            damagingCollider.enabled = true;
            yield return new WaitForSeconds(fastGlaive ? 0.1f : 0.2f);
            damagingCollider.enabled = false;
        }
    }
}
