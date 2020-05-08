using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystallinePillar : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D takeDamageHitBox;
    [SerializeField] private GameObject spinAttackBox;
    public BossManager bossManager;
    private BossHealthBar healthBar;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource takeDamageAudio, spinLoopAudio, deathAudio;
    [SerializeField] GameObject circleAttack;
    [SerializeField] Sprite[] idleSprites;
    [SerializeField] Rigidbody2D rigidBody2D;
    Camera mainCamera;
    int whatView = 1;
    int mirror = 1;
    bool isAttacking = false;
    private float attackPeriod = 3;
    bool spinAttacked = false;

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

    IEnumerator circleEffectAttack()
    {
        isAttacking = true;
        pickView(angleToShip);
        attackPeriod = 1;
        animator.enabled = true;
        animator.SetTrigger("Pulse");
        GameObject circleInstant = Instantiate(circleAttack, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        circleInstant.GetComponent<CrystallinePillarEffectCircle>().crystalBoss = this.gameObject;
        yield return new WaitForSeconds(3f);
        animator.enabled = false;
        pickView(angleToShip);
        pickIdleSprite();
        isAttacking = false;
    }

    IEnumerator spinAttack()
    {
        animator.enabled = true;
        isAttacking = true;
        attackPeriod = 1;
        animator.SetTrigger("ChargeUpSpin");
        yield return new WaitForSeconds(5 / 12f);
        rigidBody2D.velocity = new Vector3(Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(angleToShip * Mathf.Deg2Rad)) * speed;
        spinLoopAudio.Play();
        float timer = 0;
        int numberTimesBounced = 0;
        StartCoroutine(spinHitBoxLoop());
        while(numberTimesBounced < 5)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
        
            if((Mathf.Abs(transform.position.x - mainCamera.transform.position.x) > 7f || Mathf.Abs(transform.position.y - mainCamera.transform.position.y) > 7) && timer <= 0)
            {
                rigidBody2D.velocity = new Vector3(Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(angleToShip * Mathf.Deg2Rad)) * speed;
                numberTimesBounced++;
                timer = 1;
            }
            yield return null;
        }
        rigidBody2D.velocity = Vector3.zero;
        animator.SetTrigger("ChargeDownSpin");
        StopCoroutine(spinHitBoxLoop());
        spinAttackBox.SetActive(false);
        yield return new WaitForSeconds(5 / 12f);
        isAttacking = false;
        animator.enabled = false;
    }

    IEnumerator spinHitBoxLoop()
    {
        while (true)
        {
            spinAttackBox.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            spinAttackBox.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private float angleToShip
    {
        get
        {
            return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
    }

    void pickIdleSprite()
    {
        transform.localScale = new Vector3(4 * mirror, 4);
        spriteRenderer.sprite = idleSprites[whatView - 1];
    }

    IEnumerator attackProcedure()
    {
        yield return new WaitForSeconds(14 / 12f);
        animator.enabled = false;
        while (true)
        {
            if(isAttacking == false)
            {
                pickView(angleToShip);
                pickIdleSprite();
                if(attackPeriod > 0)
                {
                    attackPeriod -= Time.deltaTime;
                }
                else
                {
                    if(spinAttacked == false)
                    {
                        spinAttacked = true;
                        StartCoroutine(spinAttack());
                    }
                    else
                    {
                        spinAttacked = false;
                        StartCoroutine(circleEffectAttack());
                    }
                }
            }

            yield return null;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(attackProcedure());
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.bossStartUp("Crystalline Pillar");
        healthBar.targetEnemy = this;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            if (health > 0)
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            }
        }
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        takeDamageHitBox.enabled = false;
        StopAllCoroutines();
        bossManager.bossBeaten(nameID, 1.083f);
        PlayerProperties.playerScript.enemiesDefeated = true;
        healthBar.bossEnd();
        deathAudio.Play();
        animator.SetTrigger("Death");
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
