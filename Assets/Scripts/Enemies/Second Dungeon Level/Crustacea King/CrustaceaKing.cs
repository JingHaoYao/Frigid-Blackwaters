using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrustaceaKing : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    PlayerScript playerScript;
    Rigidbody2D rigidBody2D;
    public GameObject invulnerableHitBox;
    BoxCollider2D damageHitBox;
    public BossManager bossManager;

    public GameObject deadCrab;
    public GameObject damageBox;
    public GameObject waterFoamBurst;
    public GameObject invulnerableIcon;
    public GameObject crystalObstacle;
    int whatView = 1;
    int mirror = 1;

    public Sprite[] closedViews;
    public Sprite[] openViews;

    float angleToShip = 0;

    bool attacking = false;
    int numberDashes = 0;

    float dashPeriod = 0;

    private float foamTimer = 0;
    public GameObject waterFoam;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        damageHitBox = GetComponent<BoxCollider2D>();
        playerScript = FindObjectOfType<PlayerScript>();
        animator.enabled = false;
        damageBox.SetActive(false);
        FindObjectOfType<BossHealthBar>().bossStartUp("Crustacea King");
        FindObjectOfType<BossHealthBar>().targetEnemy = this;
    }

    void Update()
    {
        angleToShip = (360 + Mathf.Atan2(playerScript.transform.position.y - transform.position.y, playerScript.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        spawnFoam();

        if(attacking == false)
        {
            pickView(angleToShip);
            spriteRenderer.sprite = closedViews[whatView - 1];
            transform.localScale = new Vector3(6 * mirror, 6, 0);

            dashPeriod += Time.deltaTime;
            if(dashPeriod > 1)
            {
                StartCoroutine(waveDash());
            }
        }
    }

    IEnumerator summonCrystals()
    {
        pickView(angleToShip);
        animator.enabled = true;
        animator.SetInteger("WhatView", whatView);
        animator.SetTrigger("CrystalSummon");
        GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(10f/12f);
        attacking = false;
        animator.enabled = false;
        List<Vector3> crystalPositionList = new List<Vector3>();
        for(int i = 0; i < 5; i++)
        {
            Vector3 posToSpawn = new Vector3(Camera.main.transform.position.x + Random.Range(-7.0f, 7.0f), Camera.main.transform.position.y + Random.Range(-7.0f, 7.0f));
            while(isPositionNear(crystalPositionList, posToSpawn) == true)
            {
                posToSpawn = new Vector3(Camera.main.transform.position.x + Random.Range(-7.0f, 7.0f), Camera.main.transform.position.y + Random.Range(-7.0f, 7.0f));
            }

            GameObject crystal = Instantiate(crystalObstacle, posToSpawn, Quaternion.identity);
            if(Random.Range(0,2) == 1)
            {
                Vector3 scale = crystal.transform.localScale;
                crystal.transform.localScale = new Vector3(scale.x * -1, scale.y);

            }
            crystal.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            
            crystalPositionList.Add(posToSpawn);
        }
    }

    bool isPositionNear(List<Vector3> list, Vector3 pos)
    {
        foreach(Vector3 position in list)
        {
            if(Vector2.Distance(pos, position) < 2.5f)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator waveDash()
    {
        attacking = true;
        rigidBody2D.velocity = Vector2.zero;
        pickView(angleToShip);
        animator.enabled = true;

        //play charge smash animation
        animator.SetInteger("WhatView", whatView);
        animator.SetTrigger("ChargeSmash");
        invulnerableHitBox.SetActive(false);
        invulnerableIcon.SetActive(false);

        //wait for charge smash anim to finish playing
        yield return new WaitForSeconds(0.5f);

        //allow buffer period for player to hit boss
        animator.enabled = false;
        float waitTimer = 0;
        while(waitTimer < 1.5f)
        {
            waitTimer += Time.deltaTime;
            pickView(angleToShip);
            spriteRenderer.sprite = openViews[whatView - 1];
            transform.localScale = new Vector3(6 * mirror, 6, 0);
            yield return null;
        }

        invulnerableHitBox.SetActive(true);
        invulnerableIcon.SetActive(true);
        float angleToAttack = angleToShip;
        pickView(angleToAttack);
        animator.enabled = true;
        animator.SetInteger("WhatView", whatView);
        animator.SetTrigger("Smash");
        GetComponents<AudioSource>()[2].Play();
        GetComponents<AudioSource>()[3].Play();

        if (mirror == 1)
        {
            damageBox.transform.rotation = Quaternion.Euler(0, 0, angleToAttack);
        }
        else
        {
            damageBox.transform.rotation = Quaternion.Euler(0, 0, 180 + angleToAttack);
        }

        damageBox.SetActive(true);

        Instantiate(waterFoamBurst, transform.position, Quaternion.Euler(0, 0, angleToAttack + 90));
        float attackPeriod = 0;
        rigidBody2D.velocity = new Vector2(Mathf.Cos(angleToAttack * Mathf.Deg2Rad), Mathf.Sin(angleToAttack * Mathf.Deg2Rad)) * 15;
        float speedMagnitude = 16;
        while (attackPeriod <= 1f)
        {
            attackPeriod += Time.deltaTime;
            speedMagnitude -= Time.deltaTime * 10;
            rigidBody2D.velocity = new Vector2(Mathf.Cos(angleToAttack * Mathf.Deg2Rad), Mathf.Sin(angleToAttack * Mathf.Deg2Rad)) * speedMagnitude;
            yield return null;
        }
        rigidBody2D.velocity = Vector3.zero;
        damageBox.SetActive(false);
        animator.enabled = false;
        numberDashes++;
        dashPeriod = 0;
        if(numberDashes == 2)
        {
            StartCoroutine(summonCrystals());
            numberDashes = 0;
        }
        else
        {
            attacking = false;
        }
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
        //animator.SetInteger("WhatView", whatView);
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    public void crystalDamage()
    {
        if (playerScript.playerDead == false)
        {
            this.GetComponents<AudioSource>()[0].Play();
            dealDamage(4);
            if (health <= 0)
            {

                Instantiate(deadCrab, transform.position, Quaternion.identity);
                FindObjectOfType<BossHealthBar>().bossEnd();
                bossManager.bossBeaten("crustacea_king", 0.9f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && invulnerableHitBox.activeSelf == false)
        {
            this.GetComponents<AudioSource>()[0].Play();
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            if (health <= 0)
            {
                Instantiate(deadCrab, transform.position, Quaternion.identity);
                FindObjectOfType<BossHealthBar>().bossEnd();
                bossManager.bossBeaten("crustacea_king", 0.9f);
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
