using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientLavaCannon : Enemy
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] WhichRoomManager roomManager;
    [SerializeField] AudioSource startUpAudio, deathAudio, damageAudio;
    [SerializeField] Collider2D takeDamageHitbox;
    BossHealthBar bossHealthBar;
    private bool dormant = true;
    private float attackPeriod = 0.5f;
    private Camera mainCamera;
    [SerializeField] GameObject treasureChest;
    [SerializeField] AncientLavaCannonHead cannonHeadInstant;

    bool flameThrowerAttack = true;

    int numberProjectileAttacks = 0;

    IEnumerator awakenRoutine()
    {
        EnemyPool.addEnemy(this);
        dormant = false;
        roomManager.antiSpawnSpaceDetailer.spawnDoorSeals();
        startUpAudio.Play();
        PlayerProperties.playerScript.enemiesDefeated = false;
        bossHealthBar.bossStartUp("Ancient Lava Cannon");
        bossHealthBar.targetEnemy = this;

        animator.Play("Base Rise Up");
        cannonHeadInstant.StartUp();

        yield return new WaitForSeconds(8 / 12f);

        StartCoroutine(attackLoop());
    }

    IEnumerator attackLoop()
    {
        while(true)
        {
            
            if(attackPeriod > 0)
            {
                attackPeriod -= Time.deltaTime;
            }
            else
            {
                if(flameThrowerAttack == true)
                {
                    flameThrowerAttack = false;
                    float baseAngle = 90 * Random.Range(0, 4);
                    float toAngle = Random.Range(0, 2) == 1 ? baseAngle - 179 : baseAngle + 179;
                    attackPeriod = 6;
                    if (stopAttacking == false)
                    {
                        cannonHeadInstant.StartFlameThrowerRoutine(baseAngle, toAngle);
                    }
                }
                else
                {
                    flameThrowerAttack = true;
                    attackPeriod = 5;
                    if (stopAttacking == false)
                    {
                        cannonHeadInstant.StartSpitAttack();
                    }
                }
            }
            yield return null;
        }
    }
    
    private void Start()
    {
        mainCamera = Camera.main;
        bossHealthBar = FindObjectOfType<BossHealthBar>();
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
        animator.Play("Base Death");
        cannonHeadInstant.DeathRoutine();
        deathAudio.Play();
        bossHealthBar.bossEnd();
        takeDamageHitbox.enabled = false;
        Instantiate(treasureChest, Camera.main.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        cannonHeadInstant.StartHitFrame();
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
