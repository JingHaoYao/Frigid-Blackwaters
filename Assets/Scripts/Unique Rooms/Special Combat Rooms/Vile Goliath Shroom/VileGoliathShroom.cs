using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VileGoliathShroom : Enemy
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private WhichRoomManager roomManager;
    [SerializeField] private GameObject sporeParticles;
    private bool dormant = true;
    public GameObject mushroomChest;
    [SerializeField] private AudioSource takeDamageAudio;
    [SerializeField] private AudioSource sprayAttackAudio;
    [SerializeField] private AudioSource shortHopAttackAudio;
    [SerializeField] private AudioSource startUpAudio;
    [SerializeField] private Collider2D takeDamageHitBox;
    [SerializeField] private Collider2D shortHopDealDamageHitBox;
    public GameObject foamParticles;
    public GameObject sprayProjectile;
    public GameObject smallMushroom;
    public GameObject largeMushroom;
    BossHealthBar bossHealthBar;
    Camera mainCamera;

    private void Start()
    {
        bossHealthBar = FindObjectOfType<BossHealthBar>();
        mainCamera = Camera.main;
    }

    IEnumerator awakenRoutine()
    {
        bossHealthBar.bossStartUp("Vile Goliath Shroom");
        bossHealthBar.targetEnemy = this;
        startUpAudio.Play();
        animator.SetTrigger("Awaken");
        EnemyPool.addEnemy(this);
        roomManager.antiSpawnSpaceDetailer.spawnDoorSeals();
        PlayerProperties.playerScript.enemiesDefeated = false;
        yield return new WaitForSeconds(9 / 12f);
        dormant = false;
        StartCoroutine(sprayAttack());
    }

    IEnumerator stompAttack(float delayUntilAttackAgain)
    {
        animator.SetTrigger("MushroomAttack");
        shortHopAttackAudio.Play();
        yield return new WaitForSeconds(2 / 12f);
        shortHopDealDamageHitBox.enabled = true;
        yield return new WaitForSeconds(1 / 12f);
        shortHopDealDamageHitBox.enabled = false;
        yield return new WaitForSeconds(3 / 12f);
        StartCoroutine(spawnMushrooms());
        yield return new WaitForSeconds(delayUntilAttackAgain);
        StartCoroutine(sprayAttack());
    }

    bool checkIfPositionIsValid(Vector3 pos)
    {
        return Mathf.Abs(pos.x - mainCamera.transform.position.x) < 8.5f && Mathf.Abs(pos.y - mainCamera.transform.position.y) < 8.5f;
    }

    IEnumerator spawnMushrooms()
    {
        for(int i = 0; i < 4; i++)
        {


            float angle = i * 90;
            Vector3 potentialSpawnPos = transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 2;
            if (checkIfPositionIsValid(potentialSpawnPos))
            {
                GameObject mushroomInstant = Instantiate(largeMushroom, potentialSpawnPos, Quaternion.identity);
                mushroomInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 6; i++)
        {
            float angle = i * 60;
            Vector3 potentialSpawnPos = transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 3.5f;
            if (checkIfPositionIsValid(potentialSpawnPos))
            {
                GameObject mushroomInstant = Instantiate(smallMushroom, potentialSpawnPos, Quaternion.identity);
                mushroomInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
    }

    IEnumerator sprayAttack()
    {
        animator.SetTrigger("SprayAttack");
        yield return new WaitForSeconds(5 / 12f);
        sprayAttackAudio.Play();
        if (stopAttacking == false)
        {
            spawnSprayAttacks();
        }
        float angleToPlayer = (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerProperties.playerShipPosition);
        StartCoroutine(spawnFoam(angleToPlayer, Mathf.Clamp(distanceToPlayer, 0, 8) / speed));
        Vector3 direction = new Vector3(Mathf.Cos(angleToPlayer * Mathf.Deg2Rad), Mathf.Sin(angleToPlayer * Mathf.Deg2Rad));
        LeanTween.move(this.gameObject, transform.position + direction * Mathf.Clamp(distanceToPlayer, 0, 8), Mathf.Clamp(distanceToPlayer, 0, 8) / speed)
            .setEaseOutCubic()
            .setOnComplete(() => StartCoroutine(stompAttack(1f)))
            .setOnUpdate((float val) => {
                if (stopAttacking == true)
                {
                    LeanTween.cancel(this.gameObject);
                }
            });
    }


    IEnumerator spawnFoam(float whatAngle, float duration)
    {
        float currDuration = 0;
        while (currDuration < duration)
        {
            Instantiate(foamParticles, transform.position + Vector3.up * 0.75f, Quaternion.Euler(0, 0, whatAngle + 90));
            yield return new WaitForSeconds(0.05f);
            currDuration += 0.05f;
        }
    }

    void spawnSprayAttacks()
    {
        for(int i = 0; i < 8; i++)
        {
            float angle = i * 45;
            GameObject projectileInstant = Instantiate(sprayProjectile, transform.position + Vector3.up * 2, Quaternion.Euler(0, 0, angle));
            projectileInstant.GetComponent<BasicProjectile>().angleTravel = angle;
            projectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            if(dormant == true && Vector2.Distance(mainCamera.transform.position, transform.position) < 4)
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
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        roomManager.antiSpawnSpaceDetailer.trialDefeated = true;
        PlayerProperties.playerScript.enemiesDefeated = true;
        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = true;
        animator.SetTrigger("Death");
        bossHealthBar.bossEnd();
        takeDamageHitBox.enabled = false;

        if (transform.position.y < Camera.main.transform.position.y)
        {
            Instantiate(mushroomChest, Camera.main.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(mushroomChest, Camera.main.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
        }
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        takeDamageAudio.Play();
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
