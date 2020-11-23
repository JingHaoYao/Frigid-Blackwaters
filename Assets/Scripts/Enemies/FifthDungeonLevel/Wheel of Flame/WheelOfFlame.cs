using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelOfFlame : Enemy
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] WhichRoomManager roomManager;
    [SerializeField] AudioSource damageAudio, flameAttackAudio;
    [SerializeField] Collider2D takeDamageHitbox;
    BossHealthBar bossHealthBar;
    private bool dormant = true;
    private float attackPeriod = 0.5f;
    private Camera mainCamera;
    [SerializeField] GameObject treasureChest;
    [SerializeField] GameObject pyrotheumProjectile;
    bool isAttacking = false;
    [SerializeField] GameObject dashDamageHitbox;
    [SerializeField] LayerMask layerMask;


    IEnumerator awakenRoutine()
    {
        EnemyPool.addEnemy(this);
        dormant = false;
        roomManager.antiSpawnSpaceDetailer.spawnDoorSeals();
        PlayerProperties.playerScript.enemiesDefeated = false;
        bossHealthBar.bossStartUp("Wheel of Flame");
        bossHealthBar.targetEnemy = this;
        animator.Play("Wheel of Flame Rise");

        yield return new WaitForSeconds(10 / 12f);

        StartCoroutine(attackLoop());
    }

    IEnumerator attackLoop()
    {
        while (true)
        {
            animator.speed = 1;
            animator.Play("Wheel of Flame Attack Spin");
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(3 / 12f);
                float offSetAngle = Random.Range(0, 45);

                if (stopAttacking == false)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        GameObject pyrotheumProjectileInstant = Instantiate(pyrotheumProjectile, transform.position + Vector3.up * 2, Quaternion.identity);
                        pyrotheumProjectileInstant.GetComponent<PyrotheumProjectile>().angleTravel = k * 45 + offSetAngle;
                        pyrotheumProjectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                        flameAttackAudio.Play();
                    }
                }

                yield return new WaitForSeconds(5 / 12f);
            }

            animator.Play("Wheel of Flame Regular Spin");

            if (stopAttacking == false)
            {
                LeanTween.value(1, 5, 0.75f).setOnUpdate((float val) => { animator.speed = val; });

                yield return new WaitForSeconds(0.75f);

                float angleAttack = angleToShip;

                if (stopAttacking == false)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        spriteRenderer.material.SetFloat("_FlashAmount", 1);
                        yield return new WaitForSeconds(0.1f);
                        spriteRenderer.material.SetFloat("_FlashAmount", 0);
                        yield return new WaitForSeconds(0.1f);
                    }

                    dashDamageHitbox.SetActive(true);

                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(angleAttack * Mathf.Deg2Rad), Mathf.Sin(angleAttack * Mathf.Deg2Rad)), 20, layerMask);

                    Vector3 moveVector = new Vector3(hit.point.x, Mathf.Clamp(hit.point.y, mainCamera.transform.position.y - 8, mainCamera.transform.position.x + 5));

                    float time = Vector2.Distance(moveVector, transform.position) / speed;
                    LeanTween.move(this.gameObject, moveVector, time).setEaseInQuad();

                    yield return new WaitForSeconds(time);
                }
                
                dashDamageHitbox.SetActive(false);
                LeanTween.value(5, 1, 0.75f).setOnUpdate((float val) => { animator.speed = val; });
            }

            yield return new WaitForSeconds(8 / 12f);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        bossHealthBar = FindObjectOfType<BossHealthBar>();
        dashDamageHitbox.SetActive(false);
    }

    private float angleToShip
    {
        get
        {
            return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            if (dormant == true && Vector2.Distance(mainCamera.transform.position, transform.position) < 4)
            {
                StartCoroutine(awakenRoutine());
                return;
            }

            if (health > 0)
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            }
        }
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        roomManager.antiSpawnSpaceDetailer.trialDefeated = true;
        PlayerProperties.playerScript.enemiesDefeated = true;
        animator.speed = 1;
        spriteRenderer.material.color = Color.red;
        animator.Play("Wheel of Flame Death");
        bossHealthBar.bossEnd();
        takeDamageHitbox.enabled = false;
        if (transform.position.y < mainCamera.transform.position.y)
        {
            Instantiate(treasureChest, mainCamera.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(treasureChest, mainCamera.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
        }
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
        SpawnArtifactKillsAndGoOnCooldown(1f);
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.material.color = Color.white;
    }
}
