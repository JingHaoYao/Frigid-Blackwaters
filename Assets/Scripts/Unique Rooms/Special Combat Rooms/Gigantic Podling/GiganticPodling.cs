using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiganticPodling : Enemy
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private WhichRoomManager roomManager;
    [SerializeField] private GameObject sleepyZParticleEffects;
    private bool dormant = true;
    [SerializeField] private AudioSource takeDamageAudio;
    [SerializeField] private AudioSource spitAttackAudio;
    [SerializeField] private AudioSource rainAttackAudio;
    [SerializeField] private AudioSource deathAudio;
    [SerializeField] private Collider2D takeDamageHitBox;
    [SerializeField] private Rigidbody2D rigidBody2D;
    public GameObject foamParticles;
    BossHealthBar bossHealthBar;
    Camera mainCamera;
    public GameObject podlingChest;
    public GameObject rainPod;
    public GameObject bouncingPod;
    private bool isAttacking = false;
    private int whatView = 0;
    private int mirror = 1;
    private int pastView = -1;
    private float attackTimer = 3;
    private int numberBouncePodAttacks = 0;

    private void Start()
    {
        bossHealthBar = FindObjectOfType<BossHealthBar>();
        mainCamera = Camera.main;
    }

    IEnumerator awakenRoutine()
    {
        bossHealthBar.bossStartUp("Gigantic Podling");
        bossHealthBar.targetEnemy = this;
        animator.SetTrigger("Awaken");
        roomManager.antiSpawnSpaceDetailer.spawnDoorSeals();
        PlayerProperties.playerScript.enemiesDefeated = false;
        sleepyZParticleEffects.SetActive(false);
        yield return new WaitForSeconds(11 / 12f);
        dormant = false;
        StartCoroutine(attackLoop());
    }

    bool checkIfPositionIsValid(Vector3 pos)
    {
        return Mathf.Abs(pos.x - mainCamera.transform.position.x) < 8.5f && Mathf.Abs(pos.y - mainCamera.transform.position.y) < 8.5f;
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

    Vector3 pickRandomPosition()
    {
        Vector3 randPos = new Vector3(Random.Range(mainCamera.transform.position.x - 8.5f, mainCamera.transform.position.x + 8.5f), Random.Range(mainCamera.transform.position.y - 8.5f, mainCamera.transform.position.y + 8.5f));
        while(Vector2.Distance(randPos, transform.position) < 4)
        {
            randPos = new Vector3(Random.Range(mainCamera.transform.position.x - 8.5f, mainCamera.transform.position.x + 8.5f), Random.Range(mainCamera.transform.position.y - 8.5f, mainCamera.transform.position.y + 8.5f));
        }
        return randPos;
    }


    IEnumerator rainPodAttack(int numberTimesRainAttack)
    {
        isAttacking = true;
        animator.SetTrigger("RainAttackWindUp");
        yield return new WaitForSeconds(9 / 12f);
        for (int i = 0; i < numberTimesRainAttack; i++)
        {
            if (stopAttacking == false)
            {
                animator.SetTrigger("RainAttack");
                rainAttackAudio.Play();
                yield return new WaitForSeconds(3 / 12f);
                if (stopAttacking == false)
                {
                    GameObject rainPodInstant = Instantiate(rainPod, transform.position + Vector3.up * 2.5f, Quaternion.identity);
                    rainPodInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;

                    if (Random.Range(0, 3) == 1)
                    {
                        rainPodInstant.GetComponent<Thornball>().targetLocation = PlayerProperties.playerShipPosition;
                    }
                    else
                    {
                        rainPodInstant.GetComponent<Thornball>().targetLocation = this.pickRandomPosition();
                    }
                    yield return new WaitForSeconds(4 / 12f);
                }
            }
        }
        animator.SetTrigger("RainAttackWindDown");
        yield return new WaitForSeconds(9 / 12f);
        isAttacking = false;
        attackTimer = 3;
        pickIdleAnimation(angleToShip, true);
    }

    void pickView(float angleOrientation)
    {
        if (angleOrientation >= 0 && angleOrientation < 60)
        {
            whatView = 4;
            mirror = 1;
        }
        else if (angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 3;
            mirror = 1;
        }
        else if (angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 4;
            mirror = -1;
        }
        else if (angleOrientation >= 180 && angleOrientation < 240)
        {
            whatView = 2;
            mirror = -1;
        }
        else if (angleOrientation >= 240 && angleOrientation < 300)
        {
            whatView = 1;
            mirror = 1;
        }
        else
        {
            whatView = 2;
            mirror = 1;
        }
        animator.SetInteger("WhatView", whatView);
    }

    void setScale()
    {
        transform.localScale = new Vector3(5 * mirror, 5);
    }

    void pickIdleAnimation(float angle, bool force)
    {
        pickView(angle);
        setScale();
        if (pastView != whatView || force == true)
        {
            animator.SetTrigger("Idle");
            pastView = whatView;
        }
    }

    IEnumerator spitAttack()
    {
        pickView(angleToShip);
        float angleAttack = angleToShip;
        setScale();
        isAttacking = true;
        animator.SetTrigger("SpitAttack");
        yield return new WaitForSeconds(4 / 12f);
        if (stopAttacking == false)
        {
            spitAttackAudio.Play();
            for (int i = 0; i < 3; i++)
            {
                float angleToAttack = angleAttack - 15 + 15 * i;
                GameObject bouncePodInstant = Instantiate(bouncingPod, transform.position + Vector3.up, Quaternion.identity);
                bouncePodInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                bouncePodInstant.GetComponent<GiganticPodlingBouncePod>().targetLocation = transform.position + new Vector3(Mathf.Cos(angleToAttack * Mathf.Deg2Rad), Mathf.Sin(angleToAttack * Mathf.Deg2Rad)) * 3.5f;
            }
        }
        yield return new WaitForSeconds(10 / 12f);
        isAttacking = false;
        attackTimer = 1;
        pickIdleAnimation(angleToShip, true);
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

    IEnumerator attackLoop()
    {
        while (true)
        {
            if (isAttacking == false)
            {
                pickIdleAnimation(angleToShip, false);
                SetVelocity(angleToShip);

                if (attackTimer <= 0 && stopAttacking == false)
                {
                    if (numberBouncePodAttacks < 3)
                    {
                        numberBouncePodAttacks++;
                        StartCoroutine(spitAttack());
                    }
                    else
                    {
                        numberBouncePodAttacks = 0;
                        StartCoroutine(rainPodAttack(Random.Range(6, 10)));
                    }
                }
                else
                {
                    attackTimer -= Time.deltaTime;
                }
            }
            else
            {
                rigidBody2D.velocity = Vector3.zero;
            }
            yield return null;
        }
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

    public override void deathProcedure()
    {
        StopAllCoroutines();
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        roomManager.antiSpawnSpaceDetailer.trialDefeated = true;
        PlayerProperties.playerScript.enemiesDefeated = true;
        animator.SetTrigger("Death");
        bossHealthBar.bossEnd();
        takeDamageHitBox.enabled = false;

        if (transform.position.y < Camera.main.transform.position.y)
        {
            Instantiate(podlingChest, Camera.main.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(podlingChest, Camera.main.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
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
