using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormSpirit : Enemy
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] WhichRoomManager roomManager;
    [SerializeField] AudioSource stormLoop, stormAttack, damageAudio;
    [SerializeField] Collider2D takeDamageHitbox;
    [SerializeField] LayerMask collisionLayerMask;
    [SerializeField] GameObject thunderTornado, windProjectile;
    [SerializeField] GameObject pillar;
    [SerializeField] GameObject damageHitbox;
    BossHealthBar bossHealthBar;
    private bool dormant = true;
    private bool isAttacking = false;
    private float attackPeriod = 0;
    private Camera mainCamera;
    [SerializeField] GameObject treasureChest;
    private Vector3 travelVector;
    private Vector3 lastPositionHit = Vector3.one;

    int numberProjectileAttacks = 0;

    IEnumerator awakenRoutine()
    {
        bossHealthBar.bossStartUp("Storm Elemental");
        bossHealthBar.targetEnemy = this;
        animator.SetTrigger("Awaken");
        EnemyPool.addEnemy(this);
        roomManager.antiSpawnSpaceDetailer.spawnDoorSeals();
        PlayerProperties.playerScript.enemiesDefeated = false;
        stormLoop.Play();
        LeanTween.value(0, 1, 1f).setOnUpdate((float val) => { stormLoop.volume = val; });
        dormant = false;
        yield return new WaitForSeconds(12 / 12f);
        damageHitbox.SetActive(true);
        travelVector = new Vector3(Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(angleToShip * Mathf.Deg2Rad)) * speed;
        SetVelocity();
        StartCoroutine(attackLoop());
    }

    void summonPillars()
    {
        Instantiate(pillar, mainCamera.transform.position + new Vector3(4, 4), Quaternion.identity);
        GameObject pillarInstant = Instantiate(pillar, mainCamera.transform.position + new Vector3(-4, 4), Quaternion.identity);
        pillarInstant.transform.localScale = new Vector3(-6, 6, 0);
        pillarInstant = Instantiate(pillar, mainCamera.transform.position + new Vector3(-4, -4), Quaternion.identity);
        pillarInstant.transform.localScale = new Vector3(-6, 6, 0);
        Instantiate(pillar, mainCamera.transform.position + new Vector3(4,- 4), Quaternion.identity);
    }

    private void Start()
    {
        mainCamera = Camera.main;
        bossHealthBar = FindObjectOfType<BossHealthBar>();
        summonPillars();
    }

    private float angleToShip
    {
        get
        {
            return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
    }

    private void SetVelocity(float angle)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;
    }

    IEnumerator spawnTornados()
    {
        isAttacking = true;
        animator.SetTrigger("SummonThunder");
        stormAttack.Play();
        yield return new WaitForSeconds(3 / 12f);
        for(int i = 0; i < 4; i++)
        {
            GameObject instant = Instantiate(thunderTornado, transform.position, Quaternion.identity);
            instant.GetComponent<StormSpiritThunderTornado>().Initialize((i * 90 + 45) * Mathf.Deg2Rad, this.gameObject);
        }
        yield return new WaitForSeconds(5 / 12f);
        animator.SetTrigger("Idle");
        isAttacking = false;
    }

    IEnumerator spawnWindProjectiles()
    {
        isAttacking = true;
        animator.SetTrigger("SummonThunder");
        stormAttack.Play();
        yield return new WaitForSeconds(3 / 12f);

        for (int i = 0; i < 8; i++)
        {
            GameObject instant = Instantiate(windProjectile, transform.position + Vector3.up * 2, Quaternion.identity);
            instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            instant.GetComponent<BasicProjectile>().angleTravel = i * 45;
        }

        yield return new WaitForSeconds(5 / 12f);
        animator.SetTrigger("Idle");
        isAttacking = false;
    }

    IEnumerator attackLoop()
    {
        while (true)
        {
            SetVelocity();
            if (isAttacking == false)
            {
                attackPeriod += Time.deltaTime;

                if(attackPeriod > 1.5f && stopAttacking == false)
                {
                    if(numberProjectileAttacks < 2)
                    {
                        StartCoroutine(spawnWindProjectiles());
                        attackPeriod = 0;
                        numberProjectileAttacks++;
                    }
                    else
                    {
                        StartCoroutine(spawnTornados());
                        attackPeriod = 0;
                        numberProjectileAttacks = 0;
                    }
                }
            }
            yield return null;
        }
    }

    private void SetVelocity()
    {
        rigidBody2D.velocity = travelVector;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12 && (Vector2.Distance(lastPositionHit, transform.position) > 0.5f || lastPositionHit.z == 1))
        {
            Vector3 normalVector = collision.GetContact(0).normal;
            travelVector = Vector3.Reflect(travelVector, normalVector);
            lastPositionHit = new Vector3(transform.position.x, transform.position.y, 0);
            SetVelocity();
        }
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        roomManager.antiSpawnSpaceDetailer.trialDefeated = true;
        PlayerProperties.playerScript.enemiesDefeated = true;
        LeanTween.value(1, 0, 0.5f).setOnUpdate((float val) => { stormLoop.volume = val; });
        animator.SetTrigger("Death");
        Destroy(this.gameObject, 8 / 12f);
        bossHealthBar.bossEnd();
        takeDamageHitbox.enabled = false;

        // Spawn Chest
        if (transform.position.y < Camera.main.transform.position.y)
        {
            Instantiate(treasureChest, Camera.main.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(treasureChest, Camera.main.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
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
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
