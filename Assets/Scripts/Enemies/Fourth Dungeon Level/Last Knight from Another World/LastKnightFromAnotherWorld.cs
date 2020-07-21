using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastKnightFromAnotherWorld : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D takeDamageHitBox;
    public BossManager bossManager;
    private BossHealthBar healthBar;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] AudioSource takeDamageAudio, awakenAudio, swipeAudio, deathAudio;
    [SerializeField] int bodyHealth;
    [SerializeField] int bodyMaxHealth;
    [SerializeField] LastKnightHelmet helmet;
    [SerializeField] GameObject swipeHitBox;

    int whatView = 1;
    int prevView = -1;
    int mirror = 1;
    bool isAttacking = false;

    void pickView(float angleOrientation)
    {
        if (angleOrientation >= 0 && angleOrientation < 60)
        {
            whatView = 4;
            mirror = -1;
        }
        else if (angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 3;
            mirror = 1;
        }
        else if (angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 4;
            mirror = 1;
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

    void pickIdleAnimation()
    {
        pickView(angleToShip);
        if (whatView != prevView)
        {
            prevView = whatView;
            animator.SetTrigger("Idle");
        }
        transform.localScale = new Vector3(5 * mirror, 5);
    }

    private float angleToShip
    {
        get
        {
            return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
    }

    IEnumerator attackProcedure()
    {
        yield return new WaitForSeconds(19/12f);
        while (true)
        {
            if(isAttacking == false)
            {
                pickIdleAnimation();
                rigidBody2D.velocity = new Vector3(Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(angleToShip * Mathf.Deg2Rad)) * speed;
                if (Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) < 2.5f)
                {
                    StartCoroutine(swipeAttack());
                }
            }
            else
            {
                rigidBody2D.velocity = Vector3.zero;
            }
            yield return null;
        }
    }

    IEnumerator swipeAttack()
    {
        pickView(angleToShip);
        animator.SetTrigger("Swipe");
        float angleToSwipe = angleToShip;
        isAttacking = true;
        yield return new WaitForSeconds(6 / 12f);
        swipeAudio.Play();
        swipeHitBox.SetActive(true);
        swipeHitBox.transform.rotation = Quaternion.Euler(0, 0, angleToSwipe);
        yield return new WaitForSeconds(3 / 12f);
        swipeHitBox.SetActive(false);
        yield return new WaitForSeconds(3 / 12f);
        isAttacking = false;
        prevView = -1;
    }

    private void Update()
    {
        health = helmet.health + bodyHealth;
    }

    private void Start()
    {
        StartCoroutine(attackProcedure());
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.bossStartUp("Ethereal Archer");
        awakenAudio.Play();
        healthBar.targetEnemy = this;
        EnemyPool.addEnemy(this);
        helmet.Initialize(this);
    }

    public void HelmetDied()
    {
        StartCoroutine(waitForBodyDeath());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())

            if (health > 0)
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
                bodyHealth -= collision.gameObject.GetComponent<DamageAmount>().damage;

                if(bodyHealth <= 0)
                {
                    bodyDown();
                    rigidBody2D.velocity = Vector3.zero;
                    spriteRenderer.color = Color.white;
                    StartCoroutine(waitForHelmetDeath());
                }
            }
    }

    void bodyDown()
    {
        StopAllCoroutines();
        takeDamageHitBox.enabled = false;
        deathAudio.Play();
        animator.SetTrigger("Death");
    }

    IEnumerator waitForBodyDeath()
    {
        float period = 0;
        while (period < 6)
        {
            period += Time.deltaTime;
            if (health <= 0)
            {
                deathAudio.Play(0);
                animator.SetTrigger("Death");
                deathProcedure();
            }

            yield return null;
        }
        helmet.reAwaken();
    }

    IEnumerator waitForHelmetDeath()
    {
        float period = 0;
        while (period < 6)
        {
            period += Time.deltaTime;
            if (health <= 0)
            {
                deathProcedure();
            }

            yield return null;
        }
        isAttacking = false;
        takeDamageHitBox.enabled = true;
        bodyHealth = bodyMaxHealth / 2;
        animator.SetTrigger("Awaken");
        awakenAudio.Play();
        StartCoroutine(attackProcedure());
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        takeDamageHitBox.enabled = false;
        bossManager.bossBeaten(nameID, 1f);
        PlayerProperties.playerScript.enemiesDefeated = true;
        healthBar.bossEnd();
        SaveSystem.SaveGame();
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
