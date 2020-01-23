using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectralHelmsman : Enemy
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource[] audioSources;
    public BossHealthBar bossHealthBar;
    public BossManager bossManager;
    public PlayerScript playerScript;
    public GameObject swordSlashEffect;
    public LayerMask filter;
    public GameObject leftShip, rightShip;

    int mirror = 1;
    int whatView = 1;
    int prevView = 1;

    private float attackDuration = 5;
    float angleToShip;

    bool attacking = false;

    int numberDashes = 0;

    private void Start()
    {
        bossHealthBar.bossStartUp("Spectral Helmsman");
        bossHealthBar.targetEnemy = this;
    }

    IEnumerator summonShips()
    {
        attackDuration = 2;
        attacking = true;
        animator.SetTrigger("Summon");
        audioSources[3].Play();
        yield return new WaitForSeconds(10f / 12f);
        audioSources[2].Play();
        GameObject ship = Instantiate(leftShip, new Vector3(1407.8f - Random.Range(0, 7), 31f, 0), Quaternion.identity);
        ship.GetComponent<SpectralHelmsmanGhostShip>().spectralHelmsman = gameObject;
        ship = Instantiate(rightShip, new Vector3(1392.2f + Random.Range(0, 7), 31f, 0), Quaternion.identity);
        ship.GetComponent<SpectralHelmsmanGhostShip>().spectralHelmsman = gameObject;
        yield return new WaitForSeconds(8f / 12f);
        pickView(angleToShip);
        animator.SetTrigger("Idle");
        transform.localScale = new Vector3(4 * mirror, 4);
        attacking = false;
    }

    IEnumerator swordDash(float direction)
    {
        attacking = true;
        pickView(direction);
        pickIdleAnim();
        attackDuration = 1;
        Vector3 directionVector = Vector3.Normalize(new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad)));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionVector, Mathf.Infinity, filter);
        Vector3 target = hit.point;
        animator.SetTrigger("RaiseSword");
        audioSources[1].Play();
        yield return new WaitForSeconds(6f / 12f);
        GameObject slash = Instantiate(swordSlashEffect, new Vector3((hit.point.x + transform.position.x) / 2f, (hit.point.y + transform.position.y) / 2f, 0), Quaternion.identity);
        slash.GetComponent<FirstBossSwordSlash>().angleAttack = direction;
        slash.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        yield return new WaitForSeconds(.66f);
        transform.position = new Vector3(Mathf.Clamp(target.x, -7.5f + 1400, 7.5f + 1400), Mathf.Clamp(target.y, 13.5f, 26.5f));
        yield return new WaitForSeconds(0.3f);
        animator.SetTrigger("WithDraw");
        yield return new WaitForSeconds(7f/12f);

        pickView(angleToShip);
        animator.SetTrigger("Idle");
        transform.localScale = new Vector3(4 * mirror, 4);
        attacking = false;
    }

    void pickView(float angle)
    {
        if (angle > 255 && angle <= 285)
        {
            whatView = 1;
            mirror = 1;
        }
        else if (angle > 285 && angle <= 360)
        {
            whatView = 2;
            mirror = 1;
        }
        else if (angle > 180 && angle <= 255)
        {
            whatView = 2;
            mirror = -1;
        }
        else if (angle > 75 && angle <= 105)
        {
            whatView = 4;
            mirror = 1;
        }
        else if (angle >= 0 && angle <= 75)
        {
            whatView = 3;
            mirror = 1;
        }
        else
        {
            whatView = 3;
            mirror = -1;
        }
        animator.SetInteger("WhatView", whatView);
    }

    void pickIdleAnim()
    {
        if(whatView != prevView)
        {
            prevView = whatView;
            animator.SetTrigger("Idle");
            transform.localScale = new Vector3(4 * mirror, 4);
        }
    }

    void Update()
    {
        if (health > 0)
        {
            angleToShip = (360 + Mathf.Atan2(playerScript.transform.position.y - transform.position.y, playerScript.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            if (attackDuration > 0)
            {
                if (attacking == false)
                {
                    attackDuration -= Time.deltaTime;
                    pickView(angleToShip);
                    pickIdleAnim();
                }
            }
            else
            {
                if (stopAttacking == false)
                {
                    if (numberDashes < 2)
                    {
                        StartCoroutine(swordDash(angleToShip));
                        numberDashes++;
                    }
                    else
                    {
                        StartCoroutine(summonShips());
                        numberDashes = 0;
                    }
                }
            }
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            audioSources[0].Play();
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            if (health <= 0)
            {
                StopAllCoroutines();
                animator.SetTrigger("Death");
                audioSources[4].Play();
                bossHealthBar.bossEnd();
                bossManager.bossBeaten(nameID, 11f / 12f);
                playerScript.enemiesDefeated = true;
                SaveSystem.SaveGame();
                addKills();
                Destroy(this.gameObject);
            }
            else
            {
                StartCoroutine(hitFrame());
            }
        }
    }
}
