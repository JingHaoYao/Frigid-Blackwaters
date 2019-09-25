﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBombSkeleton : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    List<AStarNode> path;
    public GameObject deadSkele;
    public GameObject stickyBomb;
    public Sprite[] viewSprites;

    //used for movement
    Vector3 randomPos;
    float travelAngle;
    public float travelSpeed;

    //attacking
    float attackPeriod = 2;

    //choosing sprites
    int whatView, mirror;

    //etc
    GameObject playerShip;
    public int whatType = 0;

    private float foamTimer = 0;
    public GameObject waterFoam;

    float pickSpritePeriod = 0;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * travelSpeed / 3f)
            {
                foamTimer = 0;
                Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

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

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * travelSpeed;
    }

    void travelLocation()
    {
        path = GetComponent<AStarPathfinding>().seekPath;
        this.GetComponent<AStarPathfinding>().target = randomPos;
        AStarNode pathNode = path[0];
        Vector3 targetPos = pathNode.nodePosition;
        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        moveTowards(travelAngle);

        pickView(travelAngle);
        transform.localScale = new Vector3(0.11f * mirror, 0.11f);
        if (pickSpritePeriod >= 0.2f)
        {
            pickSpritePeriod = 0;
            spriteRenderer.sprite = viewSprites[whatView - 1];
        }

        if (Vector2.Distance(transform.position, randomPos) < 1.5f)
        {
            attackPeriod = 2;
            //attack
            if (stopAttacking == false)
            {
                StartCoroutine(throwBombs());
            }
            randomPos = pickRandPos();
        }
    }

    IEnumerator throwBombs()
    {
        animator.enabled = true;
        animator.SetTrigger("Attack" + whatView.ToString());
        yield return new WaitForSeconds(4f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        if (whatType == 1)
        {
            for(int i = 0; i < 4; i++)
            {
                float angle = i * 90;
                GameObject bombInstant = Instantiate(stickyBomb, transform.position + new Vector3(0, 0.35f, 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
                bombInstant.GetComponent<StickyBombProjectile>().angleTravel = angle * Mathf.Deg2Rad;
                bombInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                float angle = (i * 90) + 45;
                GameObject bombInstant = Instantiate(stickyBomb, transform.position + new Vector3(0, 0.35f, 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
                bombInstant.GetComponent<StickyBombProjectile>().angleTravel = angle * Mathf.Deg2Rad;
                bombInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        yield return new WaitForSeconds(2f / 12f);
        animator.enabled = false;
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
            mirror = -1;
        }
        else if (angle > 180 && angle <= 255)
        {
            whatView = 1;
            mirror = 1;
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
    }

    Vector3 pickRandPos()
    {
        float randX;
        float randY;
        if (Random.Range(0, 2) == 1)
        {
            randX = transform.position.x + Random.Range(5.0f, 7.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(5.0f, 7.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-7.0f, -5.0f);
            }
        }
        else
        {
            randX = transform.position.x + Random.Range(-7.0f, -5.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(5.0f, 7.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-7.0f, -5.0f);
            }
        }

        Vector3 randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        while (Physics2D.OverlapCircle(randPos, .5f))
        {
            if (Random.Range(0, 2) == 1)
            {
                randX = transform.position.x + Random.Range(5.0f, 7.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(5.0f, 7.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-7.0f, -5.0f);
                }
            }
            else
            {
                randX = transform.position.x + Random.Range(-7.0f, -5.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(5.0f, 7.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-7.0f, -5.0f);
                }
            }
            randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        }
        return randPos;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerShip = GameObject.Find("PlayerShip");
        animator.enabled = false;
        randomPos = pickRandPos();
    }

    void Update()
    {
        float angleToShip = (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 360f) % 360f;
        spawnFoam();
        if(attackPeriod <= 0)
        {
            attackPeriod = 0;
            travelLocation();
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
            attackPeriod -= Time.deltaTime;
            pickView(angleToShip);
            transform.localScale = new Vector3(0.11f * mirror, 0.11f);
            if (pickSpritePeriod >= 0.2f)
            {
                pickSpritePeriod = 0;
                spriteRenderer.sprite = viewSprites[whatView - 1];
            }
        }
        pickSpritePeriod += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
            health -= damageDealt;
            this.GetComponents<AudioSource>()[0].Play();
            if (health <= 0)
            {
                GameObject dead = Instantiate(deadSkele, transform.position, Quaternion.identity);
                dead.GetComponent<DeadEnemyScript>().whatView = whatView;
                addKills();
                Destroy(this.gameObject);
            }
            else
            {
                StartCoroutine(hitFrame());
            }
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
