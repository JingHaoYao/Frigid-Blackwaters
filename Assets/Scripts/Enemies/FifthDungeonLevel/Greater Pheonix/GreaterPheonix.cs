using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreaterPheonix : Enemy
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] WhichRoomManager roomManager;
    [SerializeField] AudioSource featherAttackAudio, damageAudio, flapAudio;
    [SerializeField] Collider2D takeDamageHitbox;
    BossHealthBar bossHealthBar;
    private bool dormant = true;
    private float attackPeriod = 0.5f;
    private Camera mainCamera;
    [SerializeField] GameObject treasureChest;
    [SerializeField] GameObject pheonixGlideProjectile;
    [SerializeField] GameObject featherProjectile;
    [SerializeField] GameObject pheonixRocks;
    bool isAttacking = false;
    Coroutine flapAudioLoop;
    int numberFeatherAttacks = 0;

    Vector3[] glidePositions = new Vector3[] { new Vector3(-15, -15), new Vector3(-15, 0), new Vector3(-15, 15), new Vector3(0, 15), new Vector3(15, 15), new Vector3(0, -15), new Vector3(15, -15), new Vector3(15, 0) };

    IEnumerator awakenRoutine()
    {
        EnemyPool.addEnemy(this);
        dormant = false;
        roomManager.antiSpawnSpaceDetailer.spawnDoorSeals();
        featherAttackAudio.Play();
        PlayerProperties.playerScript.enemiesDefeated = false;
        bossHealthBar.bossStartUp("Greater Pheonix");
        bossHealthBar.targetEnemy = this;

        animator.Play("Greater Pheonix Rise");

        yield return new WaitForSeconds(5 / 12f);

        animator.Play("Pheonix Flap Idle");

        StartCoroutine(attackLoop());
        flapAudioLoop = StartCoroutine(audioLoop());
    }

    IEnumerator flyFromCenterAttack()
    {
        StopCoroutine(flapAudioLoop);
        isAttacking = true;
        Vector3 returnPosition = transform.position;
        animator.Play("Greater Pheonix Fly From Center");
        yield return new WaitForSeconds(4 / 12f);
        LeanTween.move(this.gameObject, transform.position + Vector3.up * 20, 1f).setEaseInQuad();

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 3; i++)
        {
            Vector3 glideStartPosition = glidePositions[Random.Range(0, glidePositions.Length)];
            Vector3 glideEndPosition = new Vector3(0, 0);

            if (glideStartPosition.x < 0)
            {
                glideEndPosition.x = 15;
            }
            else if (glideStartPosition.x > 0)
            {
                glideEndPosition.x = -15;
            }

            if (glideStartPosition.y < 0)
            {
                glideEndPosition.y = 15;
            }
            else if (glideStartPosition.y > 0)
            {
                glideEndPosition.y = -15;
            }

            GameObject pheonixProjectileInstant = Instantiate(pheonixGlideProjectile, glideStartPosition, Quaternion.identity);
            pheonixProjectileInstant.GetComponent<GreaterPheonixGlideProjectile>().Initialize(glideStartPosition + mainCamera.transform.position, glideEndPosition + mainCamera.transform.position, this.gameObject);

            yield return new WaitForSeconds(3f);
        }


        animator.Play("Pheonix Flap Idle");
        flapAudioLoop = StartCoroutine(audioLoop());

        LeanTween.move(this.gameObject, transform.position - Vector3.up * 20, 1f).setEaseInQuad();

        yield return new WaitForSeconds(1f);

        isAttacking = false;
    }

    IEnumerator featherFlapAttack()
    {
        isAttacking = true;
        StopCoroutine(flapAudioLoop);
        animator.Play("Greater Pheonix Spawn Feathers");
        featherAttackAudio.Play();
        yield return new WaitForSeconds(8 / 12f);
        if (stopAttacking == false)
        {
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45;
                GameObject featherProjectileInstant = Instantiate(featherProjectile, transform.position, Quaternion.identity);
                featherProjectileInstant.GetComponent<GreaterPheonixFeather>().Initialize(angle, this.gameObject, mainCamera.transform.position);
            }
        }

        yield return new WaitForSeconds(4 / 12f);
        animator.Play("Pheonix Flap Idle");
        flapAudioLoop = StartCoroutine(audioLoop());
        isAttacking = false;
    }

    IEnumerator audioLoop()
    {
        while(true)
        {
            flapAudio.Play();
            yield return new WaitForSeconds(11 / 12f);
        }
    }

    IEnumerator attackLoop()
    {
        while (true)
        {
            if (isAttacking == false)
            {
                if (attackPeriod > 0)
                {
                    attackPeriod -= Time.deltaTime;
                }
                else
                {
                    if(numberFeatherAttacks < 2)
                    {
                        numberFeatherAttacks++;
                        if (stopAttacking == false)
                        {
                            StartCoroutine(featherFlapAttack());
                        }
                        attackPeriod = 2;
                    }
                    else
                    {
                        numberFeatherAttacks = 0;
                        if (stopAttacking == false)
                        {
                            StartCoroutine(flyFromCenterAttack());
                        }
                        attackPeriod = 1.5f;
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
        Instantiate(pheonixRocks, transform.position + Vector3.up * -0.85f, Quaternion.identity);
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
        animator.Play("Greater Pheonix Death");
        bossHealthBar.bossEnd();
        takeDamageHitbox.enabled = false;
        Instantiate(treasureChest, Camera.main.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
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
