﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoCrab : Enemy
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidBody2D;
    public Sprite[] closedViews;
    public Sprite[] openViews;
    public GameObject deadCrab;
    public GameObject damageSlash;
    GameObject playerShip;
    float travelAngle;

    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    int whatView = 0;
    int mirror = 1;
    float attackPeriod = 0;
    bool isAttacking = false;

    public GameObject invulnerableHitBox;
    BoxCollider2D hitBox;
    bool invulnerable = true;

    public GameObject invulnerabilityIcon;
    float angleToShip = 0;
    public float offset = 30;

    AStarPathfinding aStarPathfinding;

    float cardinalizeDirections(float angle)
    {
        if (angle > 22.5f && angle <= 67.5f)
        {
            return 45;
        }
        else if (angle > 67.5f && angle <= 112.5f)
        {
            return 90;
        }
        else if (angle > 112.5f && angle <= 157.5f)
        {
            return 135;
        }
        else if (angle > 157.5f && angle <= 202.5f)
        {
            return 180;
        }
        else if (angle > 202.5f && angle <= 247.5f)
        {
            return 225;
        }
        else if (angle > 247.5 && angle < 292.5)
        {
            return 270;
        }
        else if (angle > 292.5 && angle < 337.5)
        {
            return 315;
        }
        else
        {
            return 0;
        }
    }

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * rigidBody2D.velocity.magnitude / 3f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    void pickView(float angle)
    {
        if (angle > 255 && angle <= 285)
        {
            whatView = 2;
            mirror = 1;
        }
        else if (angle > 285 && angle <= 360)
        {
            whatView = 1;
            mirror = 1;
        }
        else if (angle > 180 && angle <= 255)
        {
            whatView = 1;
            mirror = -1;
        }
        else if (angle > 75 && angle <= 105)
        {
            whatView = 4;
            mirror = -1;
        }
        else if (angle >= 0 && angle <= 75)
        {
            whatView = 3;
            mirror = -1;
        }
        else
        {
            whatView = 3;
            mirror = 1;
        }
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
    }

    void spawnSlashes()
    {
        for (int i = 0; i < 6; i++)
        {
            float angle = i * 60 + offset;
            GameObject instant = Instantiate(damageSlash, transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 0.7f, Quaternion.Euler(0, 0, angle));
            instant.GetComponent<OctoCrabSlash>().angleTravel = angle;
            instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
    }

    IEnumerator attack()
    {
        animator.enabled = true;
        animator.SetTrigger("WindUp" + whatView.ToString());
        yield return new WaitForSeconds(0.333f);
        animator.enabled = false;
        yield return new WaitForSeconds(2.5f);
        animator.enabled = true;
        animator.SetTrigger("Attack" + whatView.ToString());
        this.GetComponents<AudioSource>()[1].Play();
        if (stopAttacking == false)
        {
            spawnSlashes();
        }
        yield return new WaitForSeconds(0.333f);
        animator.enabled = false;
        isAttacking = false;
    }

    void travelLocation()
    {
        path = aStarPathfinding.seekPath;
        this.aStarPathfinding.target = playerShip.transform.position;

        Vector3 targetPos = PlayerProperties.playerShipPosition;

        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }

        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if (isAttacking == false)
        {
            moveTowards(travelAngle);
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }

        if (Vector2.Distance(transform.position, playerShip.transform.position) < 4f && isAttacking == false)
        {
            isAttacking = true;
            StartCoroutine(attack());
        }

        transform.localScale = new Vector3(0.4f * mirror, 0.4f);
        pickSpritePeriod += Time.deltaTime;

        if (pickSpritePeriod >= 0.2f)
        {
            if (isAttacking == false)
            {
                spriteRenderer.sprite = closedViews[whatView - 1];
            }
            else
            {
                spriteRenderer.sprite = openViews[whatView - 1];
            }
            pickSpritePeriod = 0;
        }

        if (isAttacking == false)
        {
            hitBox.enabled = false;
            invulnerableHitBox.SetActive(true);
            invulnerabilityIcon.SetActive(true);
            invulnerable = true;
        }
        else
        {
            hitBox.enabled = true;
            invulnerableHitBox.SetActive(false);
            invulnerabilityIcon.SetActive(false);
            invulnerable = false;
        }
    }

    void Start()
    {
        hitBox = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator.enabled = false;
        playerShip = FindObjectOfType<PlayerScript>().gameObject;
        attackPeriod = Random.Range(2f, 6f);
        offset = Random.Range(0, 2) * 30;
        aStarPathfinding = GetComponent<AStarPathfinding>();
    }

    void Update()
    {
        angleToShip = (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 360f) % 360f;
        spawnFoam();
        pickView(angleToShip);
        travelLocation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0 && invulnerable == false)
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);

        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    public override void deathProcedure()
    {
        GameObject dead = Instantiate(deadCrab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        this.GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }
}
