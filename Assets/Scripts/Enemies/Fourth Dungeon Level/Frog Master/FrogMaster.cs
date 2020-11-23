using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMaster : Enemy
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource spinAudio, damageAudio, chainAudio, throwAudio;
    [SerializeField] AudioSource chainSnap;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] WhichRoomManager roomManager;
    [SerializeField] GameObject foamParticles;
    [SerializeField] Sprite[] viewSprites;
    [SerializeField] Collider2D takeDamageHitbox;
    [SerializeField] GameObject deflectionHitBox, dealDamageHitbox;
    [SerializeField] GameObject hook;
    private int whatView;
    BossHealthBar bossHealthBar;
    private bool dormant = true;
    private bool isAttacking = false;
    private float attackPeriod = 0;
    int mirror = 1;
    private bool spinStaff = false;
    private Camera mainCamera;
    int numberSpinAttacks = 0;
    Coroutine returnRoutine;
    [SerializeField] GameObject treasureChest;
    [SerializeField] AudioSource roarAudio;

    List<Vector3> handPositions = new List<Vector3> { new Vector3(0.98f, 0.08f), new Vector3( 1.13f, 0.6f), new Vector3(-0.92f, 0.23f), new Vector3(-1.36f, 0.42f) };

    public void TriggerThrowback()
    {
        returnRoutine = StartCoroutine(returnHook());
    }

    IEnumerator returnHook()
    {
        chainAudio.Play();
        animator.SetTrigger("ReturnChain");
        yield return new WaitForSeconds(10 / 12f);
        isAttacking = false;
        animator.enabled = false;
    }

    IEnumerator awakenRoutine()
    {
        animator.enabled = true;
        bossHealthBar.bossStartUp("Champion Goliath");
        bossHealthBar.targetEnemy = this;
        animator.SetTrigger("Rise");
        EnemyPool.addEnemy(this);
        roomManager.antiSpawnSpaceDetailer.spawnDoorSeals();
        PlayerProperties.playerScript.enemiesDefeated = false;
        chainSnap.Play();
        roarAudio.Play();
        dormant = false;
        yield return new WaitForSeconds(21 / 12f);
        animator.enabled = false;
        StartCoroutine(attackLoop());
        StartCoroutine(spawnFoam());
    }

    IEnumerator spawnFoam()
    {;
        while (true)
        {
            if (isAttacking == false)
            {
                Instantiate(foamParticles, transform.position + Vector3.up * 0.75f, Quaternion.Euler(0, 0, angleToShip + 90));
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

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

    void setScale()
    {
        transform.localScale = new Vector3(5 * mirror, 5);
    }

    void pickSprite()
    {
        pickView(angleToShip);
        spriteRenderer.sprite = viewSprites[whatView - 1];
        setScale();
    }

    IEnumerator battleSpin()
    {
        while (true)
        {
            dealDamageHitbox.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            dealDamageHitbox.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator spinStaffAttack(float timeToSpin)
    {
        spinStaff = true;

        pickView(angleToShip);
        animator.enabled = true;
        animator.SetTrigger("RaiseStaff");

        yield return new WaitForSeconds(0.5f);

        float period = 0;
        int currView = whatView;
        takeDamageHitbox.enabled = false;
        deflectionHitBox.SetActive(true);
        spinAudio.Play();

        Coroutine battleSpinRoutine = StartCoroutine(battleSpin());

        while(period < timeToSpin)
        {
            period += Time.deltaTime;
            pickView(angleToShip);

            if(currView != whatView)
            {
                animator.SetTrigger("SpinStaff");
                setScale();
                currView = whatView;
            }
            yield return null;
        }

        StopCoroutine(battleSpinRoutine);

        dealDamageHitbox.SetActive(false);
        takeDamageHitbox.enabled = true;
        deflectionHitBox.SetActive(false);
        spinAudio.Stop();

        animator.SetTrigger("LowerStaff");

        yield return new WaitForSeconds(0.483f);
        animator.enabled = false;
        spinStaff = false;
    }

    IEnumerator ThrowHook()
    {
        pickView(angleToShip);
        int view = whatView;
        float attackAngle = angleToShip;

        animator.enabled = true;
        isAttacking = true;

        animator.SetTrigger("ThrowChain");
        yield return new WaitForSeconds(8f / 12f);
        throwAudio.Play();
        chainAudio.Play();

        float angleToTravel = attackAngle * Mathf.Deg2Rad;

        Vector3 hookPosition = new Vector3(handPositions[view - 1].x * mirror, handPositions[view - 1].y);
        GameObject hookInstant = Instantiate(hook, transform.position + hookPosition, Quaternion.identity);
        hookInstant.GetComponent<FrogMasterHook>().Initialize(angleToTravel, this, transform.position + hookPosition);
        
        yield return new WaitForSeconds(3f / 12f);
    }

    private void Start()
    {
        mainCamera = Camera.main;
        animator.enabled = false;
        bossHealthBar = FindObjectOfType<BossHealthBar>();
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
                SetVelocity(angleToShip);

                if(spinStaff == false)
                {
                    pickSprite();
                }

                if (stopAttacking == false)
                {
                    attackPeriod -= Time.deltaTime;
                }

                if(attackPeriod <= 0)
                {
                    if (numberSpinAttacks < 1)
                    {
                        attackPeriod = 8;
                        StartCoroutine(spinStaffAttack(6));
                        numberSpinAttacks++;
                    }
                    else
                    {
                        if (stopAttacking == false)
                        {
                            attackPeriod = 0;
                            numberSpinAttacks = 0;
                            // use hookAttack
                            StartCoroutine(ThrowHook());
                        }
                    }
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
        if (spinStaff == false && collision.gameObject.GetComponent<DamageAmount>())
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
        animator.SetTrigger("Death");
        bossHealthBar.bossEnd();
        takeDamageHitbox.enabled = false;

        // Spawn Chest
        if (transform.position.y < Camera.main.transform.position.y)
        {
            Instantiate(treasureChest, Camera.main.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(treasureChest, Camera.main.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
        }
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
