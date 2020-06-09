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
    public GameObject smallCrystal, mediumCrystal, largeCrystal;
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

    [SerializeField] AudioSource riseAudio;

    Camera mainCamera;

    private float speedMagnitude;

    [SerializeField] LayerMask filterLayer;

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
        damageBox.SetActive(false);
        FindObjectOfType<BossHealthBar>().bossStartUp("Crustacea King");
        FindObjectOfType<BossHealthBar>().targetEnemy = this;
        StartCoroutine(MainGameLoop());

        mainCamera = Camera.main;
        float angle = Mathf.RoundToInt((Mathf.Atan2(mainCamera.transform.position.y - PlayerProperties.playerShipPosition.y, mainCamera.transform.position.x - PlayerProperties.playerShipPosition.x) + 2 * Mathf.PI) / (Mathf.PI / 2)) * Mathf.PI / 2;
        transform.position += new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 5;
    }

    IEnumerator MainGameLoop()
    {
        yield return new WaitForSeconds(9 / 12f);
        riseAudio.Play();
        yield return new WaitForSeconds(15 / 12f);
        GetComponents<AudioSource>()[2].Play();
        GetComponents<AudioSource>()[3].Play();
        yield return new WaitForSeconds(6 / 12f);

        animator.enabled = false;
        while (true)
        {
            angleToShip = (360 + Mathf.Atan2(playerScript.transform.position.y - transform.position.y, playerScript.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            spawnFoam();

            if (attacking == false)
            {
                pickView(angleToShip);
                spriteRenderer.sprite = closedViews[whatView - 1];
                transform.localScale = new Vector3(6 * mirror, 6, 0);

                dashPeriod += Time.deltaTime;
                if (dashPeriod > 1)
                {
                    StartCoroutine(waveDash());
                }
            }
            yield return null;
        }
    }

    IEnumerator summonCrystals()
    {
        pickView(angleToShip);
        animator.enabled = true;
        animator.SetInteger("WhatView", whatView);
        animator.SetTrigger("CrystalSummon");
        GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds((5f / 12f) / 0.8f);
        animator.enabled = false;
        pickView(angleToShip);
        spriteRenderer.sprite = closedViews[whatView - 1];
        float angleAttack = angleToShip;

        float offSet = Random.Range(0, 90);

        for(int i = 0; i < 6; i++)
        {
            float angleToConsider = i * 60 + offSet;
            Vector3 newPos = transform.position + new Vector3(Mathf.Cos(angleToConsider * Mathf.Deg2Rad), Mathf.Sin(angleToConsider * Mathf.Deg2Rad)) * 4;
            Vector3 spawningPosition = new Vector3(Mathf.Clamp(newPos.x, mainCamera.transform.position.x - 7.5f, mainCamera.transform.position.x + 7.5f), Mathf.Clamp(newPos.y, mainCamera.transform.position.y - 7.5f, mainCamera.transform.position.y + 7.5f));

            if (!Physics2D.OverlapCircle(spawningPosition, 0.5f, filterLayer))
            {
                GameObject crystalInstant = Instantiate(smallCrystal, spawningPosition, Quaternion.identity);
                crystalInstant.GetComponent<CrustaceaKingCrystal>().initializeCrystal(this);
            }
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 6; i++)
        {
            float angleToConsider = i * 60 + offSet;
            Vector3 newPos = transform.position + new Vector3(Mathf.Cos(angleToConsider * Mathf.Deg2Rad), Mathf.Sin(angleToConsider * Mathf.Deg2Rad)) * 6;
            Vector3 spawningPosition = new Vector3(Mathf.Clamp(newPos.x, mainCamera.transform.position.x - 7.5f, mainCamera.transform.position.x + 7.5f), Mathf.Clamp(newPos.y, mainCamera.transform.position.y - 7.5f, mainCamera.transform.position.y + 7.5f));

            if (!Physics2D.OverlapCircle(spawningPosition, 0.5f, filterLayer))
            {
                GameObject crystalInstant = Instantiate(mediumCrystal, spawningPosition, Quaternion.identity);
                crystalInstant.GetComponent<CrustaceaKingCrystal>().initializeCrystal(this);
            }
        }

        yield return new WaitForSeconds(0.5f);


        for (int i = 0; i < 6; i++)
        {
            float angleToConsider = i * 60 + offSet;
            Vector3 newPos = transform.position + new Vector3(Mathf.Cos(angleToConsider * Mathf.Deg2Rad), Mathf.Sin(angleToConsider * Mathf.Deg2Rad)) * 8;
            Vector3 spawningPosition = new Vector3(Mathf.Clamp(newPos.x, mainCamera.transform.position.x - 7.5f, mainCamera.transform.position.x + 7.5f), Mathf.Clamp(newPos.y, mainCamera.transform.position.y - 7.5f, mainCamera.transform.position.y + 7.5f));

            if (!Physics2D.OverlapCircle(spawningPosition, 0.5f, filterLayer))
            {
                GameObject crystalInstant = Instantiate(largeCrystal, spawningPosition, Quaternion.identity);
                crystalInstant.GetComponent<CrustaceaKingCrystal>().initializeCrystal(this);
            }
        }

        attacking = false;
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
        float angleToAttack = angleToShip;
        while (waitTimer < 1.5f)
        {
            waitTimer += Time.deltaTime;
            pickView(angleToShip);
            spriteRenderer.sprite = openViews[whatView - 1];
            angleToAttack = angleToShip;
            transform.localScale = new Vector3(6 * mirror, 6, 0);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        invulnerableHitBox.SetActive(true);
        invulnerableIcon.SetActive(true);
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
        rigidBody2D.velocity = new Vector2(Mathf.Cos(angleToAttack * Mathf.Deg2Rad), Mathf.Sin(angleToAttack * Mathf.Deg2Rad)) * (speed + 10);

        speedMagnitude = speed + 10;

        while (attackPeriod <= 1f)
        {
            attackPeriod += Time.deltaTime;
            speedMagnitude -= Time.deltaTime * 10;
            rigidBody2D.velocity = new Vector2(Mathf.Cos(angleToAttack * Mathf.Deg2Rad), Mathf.Sin(angleToAttack * Mathf.Deg2Rad)) * Mathf.Clamp(speedMagnitude, 0, float.MaxValue);
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
            dealDamage(4);
            speedMagnitude -= 2;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && invulnerableHitBox.activeSelf == false)
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
    }

    public override void deathProcedure()
    {
        Instantiate(deadCrab, transform.position, Quaternion.identity);
        FindObjectOfType<BossHealthBar>().bossEnd();
        bossManager.bossBeaten("crustacea_king", 0.9f);
        playerScript.enemiesDefeated = true;
        SaveSystem.SaveGame();
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        this.GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }
}
