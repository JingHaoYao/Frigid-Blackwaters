using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDungeonFinalBoss : Enemy
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource summonIcePrison, damageAudio, death, entrance, lightning;
    [SerializeField] private SecondDungeonFinalBossSnowShroud shroud;
    [SerializeField] private SecondDungeonFinalBossPortal portal;
    [SerializeField] private GameObject lightningEffect1, lightningEffect2;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private BoxCollider2D damageCollider;
    [SerializeField] private SecondDungeonFinalBossShadow shadow;
    public AudioManager audioManager;
    public GameObject waterSplash;
    public CameraShake cameraShake;
    public PlayerScript playerScript;
    public SecondDungeonFinalBossManager bossManager;
    public BossHealthBar bossHealthBar;
    public GameObject lightningStormAttack;

    private bool idleFlag = true;
    private int whatView = 0;
    private int prevView = 0;
    private float angleToShip;

    public GameObject icePrison;
    public GameObject lightningLance;
    public LayerMask lightningLayerMask;

    public GameObject lightningParticles;

    private int numberLightningLances = 0;
    private float delayBetweenAttacks = 2;
    private float shroudCoolDownTimer = 10;
    private float speed = 0;

    private bool secondPhaseActive = false;

    private Vector3 targetLocation;

    IEnumerator openingAnimation()
    {
        shadow.fadeShadowIn();
        LeanTween.move(this.gameObject, new Vector3(1600, 0.5f), 1f).setEaseInCubic().setOnComplete(() => {
            Instantiate(waterSplash, transform.position - Vector3.up, Quaternion.identity);
            audioManager.FadeOut("Dungeon Ambiance", 0.2f);
            audioManager.PlaySound("Second Dungeon Final Boss Fight");
            audioManager.FadeIn("Second Dungeon Final Boss Fight", 0.2f, 0.8f);
        });
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Entrance");
        entrance.Play();
        yield return new WaitForSeconds(8f/12f);
        lightningEffect1.SetActive(true);
        lightningEffect2.SetActive(true);
        cameraShake.shakeCamFunction(0.5f, 0.1f);
        yield return new WaitForSeconds(7f / 12f);
        shroud.fadeIn();
        animator.SetInteger("WhatView", 1);
        animator.SetTrigger("Idle");
    }

    IEnumerator teleportPortal(float waitDuration)
    {
        delayBetweenAttacks = 3;
        idleFlag = false;
        portal.initialWarp();
        shadow.gameObject.SetActive(false);
        yield return new WaitForSeconds(8f / 12f);
        animator.SetTrigger("Enter Portal");
        yield return new WaitForSeconds(1.2f + 8f/12f + waitDuration);
        damageCollider.enabled = false;
        float angleOrientation = Random.Range(0, Mathf.PI * 2);
        Vector3 spawnPos = playerScript.transform.position + new Vector3(Mathf.Cos(angleOrientation), Mathf.Sin(angleOrientation)) * Random.Range(5f, 7f);
        int iterations = 0;

        while (Physics2D.OverlapCircle(spawnPos, 1) && iterations < 100)
        {
            iterations++;
            angleOrientation = Random.Range(0, Mathf.PI * 2);
            spawnPos = playerScript.transform.position + new Vector3(Mathf.Cos(angleOrientation), Mathf.Sin(angleOrientation)) * Random.Range(5f, 7f);
        }
        transform.position = new Vector3(Mathf.Clamp(spawnPos.x, 1572, 1628), Mathf.Clamp(spawnPos.y, -8, 4));
        portal.endWarp();
        yield return new WaitForSeconds(8 / 12f);
        damageCollider.enabled = true;
        animator.SetTrigger("Exit Portal");
        yield return new WaitForSeconds(1.2f);
        shadow.gameObject.SetActive(true);
        forceIdleAnimation(angleToShip);
        idleFlag = true;
    }

    IEnumerator spawnStorm(Vector3 spawnPosition, Vector3 endPos, float delay)
    {
        Vector3 increment = (endPos - spawnPosition).normalized * 1.75f;
        Vector3 spawnedPos = spawnPosition;
        int iterations_ = 0;
        while (Vector2.Distance(spawnedPos, endPos) > 3 && iterations_ < 100)
        {
            GameObject storm = Instantiate(lightningStormAttack, spawnedPos, Quaternion.identity);
            storm.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            spawnedPos += increment;
            yield return new WaitForSeconds(delay);
            iterations_++;
        }
    }

    IEnumerator fireLightningLance(float direction)
    {
        idleFlag = false;
        forceIdleAnimation(direction);
        animator.SetTrigger("Lightning Lance");
        lightning.Play();
        yield return new WaitForSeconds(11f / 12f);
        cameraShake.shakeCamFunction(0.25f, 0.2f);
        RaycastHit2D hit = Physics2D.CircleCast(origin: transform.position + new Vector3(0, 0.7f), radius: 0.75f, direction: new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad)).normalized, distance: 60, layerMask: lightningLayerMask);
        GameObject lightningLanceInstant = Instantiate(lightningLance, hit.point, Quaternion.Euler(0, 0, 90 + direction));
        Vector3 spawnPos = transform.position + new Vector3(0, 0.7f);
        Vector3 incrementalDirection = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad)) * 0.75f;


        if (secondPhaseActive)
        {
            StartCoroutine(spawnStorm(transform.position + new Vector3(0, 0.7f) + incrementalDirection * 2, hit.point, 0.2f));
        }

        int iterations = 0;
        while (Vector2.Distance(spawnPos, hit.point) > 2 && iterations < 60)
        {
            Instantiate(lightningParticles, spawnPos, Quaternion.identity);
            spawnPos += incrementalDirection;
            iterations++;
        }
        lightningLanceInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        yield return new WaitForSeconds(3 / 12f);
        forceIdleAnimation(angleToShip);
        idleFlag = true;
    }

    IEnumerator icePrisonAttack()
    {
        delayBetweenAttacks = 4;
        idleFlag = false;
        animator.SetTrigger("Ice Prison");
        summonIcePrison.Play();
        yield return new WaitForSeconds(7f / 12f);
        Vector3 target = new Vector3(Mathf.Clamp(playerScript.transform.position.x, 1572, 1628), Mathf.Clamp(playerScript.transform.position.y, -7, 3));
        yield return new WaitForSeconds(4 / 12f);
        cameraShake.shakeCamFunction(0.4f, 0.2f);
        GameObject icePrisonInstant = Instantiate(icePrison, target, Quaternion.identity);
        icePrisonInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        icePrisonInstant.GetComponent<SecondDungeonFinalBossIcePrison>().ylvaCompanion = bossManager.ylva;
        yield return new WaitForSeconds(10f / 12f);
        forceIdleAnimation(angleToShip);
        idleFlag = true;
    }

    private void forceIdleAnimation(float angle)
    {
        whatView = pickView(angle);
        prevView = whatView;
        animator.SetInteger("WhatView", whatView);
        animator.SetTrigger("Idle");
    }

    private void pickIdleAnim()
    {
        whatView = pickView(angleToShip);
        if(prevView != whatView)
        {
            prevView = whatView;
            animator.SetInteger("WhatView", whatView);
            animator.SetTrigger("Idle");
        }
    }

    public void playOpeningAnimation()
    {
        StartCoroutine(openingAnimation());
    }

    private int pickView(float angleOrientation)
    {
        if (angleOrientation > 0 && angleOrientation <= 60)
        {
            return 3;
        }
        else if (angleOrientation > 60 && angleOrientation <= 120)
        {
            return 4;
        }
        else if (angleOrientation > 120 && angleOrientation <= 180)
        {
            return 6;
        }
        else if (angleOrientation > 180 && angleOrientation <= 240)
        {
            return 5;
        }
        else if (angleOrientation > 240 && angleOrientation < 300)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    IEnumerator mainBossLoop()
    {
        while (true)
        {
            angleToShip = (360 + Mathf.Atan2(playerScript.transform.position.y - transform.position.y, playerScript.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;

            if(idleFlag == true)
            {
                pickIdleAnim();

                if((float)health/maxHealth <= 0.5f && secondPhaseActive == false)
                {
                    StartCoroutine(transitionToSecondPhase());
                }

                if (delayBetweenAttacks <= 0 && stopAttacking == false && idleFlag == true)
                {
                    if (numberLightningLances >= 3)
                    {
                        numberLightningLances = 0;
                        if (shroud.IsActive())
                        {
                            StartCoroutine(icePrisonAttack());
                        }
                        else
                        {
                            StartCoroutine(teleportPortal(0.2f));
                        }
                    }
                    else
                    {
                        numberLightningLances++;
                        StartCoroutine(fireLightningLance(angleToShip));
                    }
                }
                else
                {
                    delayBetweenAttacks -= Time.deltaTime;
                }
            }

            if (shroud.IsActive())
            {
                if(Vector2.Distance(targetLocation, transform.position) > 1)
                {
                    rigidBody2D.velocity = (targetLocation - transform.position).normalized * speed;
                }
                else
                {
                    targetLocation = playerScript.transform.position;
                }
            }
            else
            {
                if(shroudCoolDownTimer > 0)
                {
                    shroudCoolDownTimer -= Time.deltaTime;
                }
                else
                {
                    if (idleFlag == true)
                    {
                        shroud.fadeIn();
                        numberLightningLances = 3;
                    }
                }
            }

            yield return null;
        }
    }

    public void InitializeBoss()
    {
        bossHealthBar.targetEnemy = this;
        bossHealthBar.bossStartUp("Ahalfar");
        StartCoroutine(mainBossLoop());
    }

    private void Start()
    {
        targetLocation = playerScript.transform.position;
        rampSpeed();
    }

    private void rampSpeed()
    {
        LeanTween.value(0, 2, 2).setEaseInOutElastic().setOnUpdate((float val) => { speed = val; });
    }

    IEnumerator transitionToSecondPhase()
    {
        idleFlag = false;
        animator.SetTrigger("Entrance");
        entrance.Play();
        yield return new WaitForSeconds(8f / 12f);
        lightningEffect1.SetActive(true);
        lightningEffect2.SetActive(true);
        cameraShake.shakeCamFunction(0.5f, 0.1f);
        yield return new WaitForSeconds(7f / 12f);
        secondPhaseActive = true;
        idleFlag = true;
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    private void dealDamageToBoss(int damage)
    {
        dealDamage(damage);
        damageAudio.Play();
        if (health <= 0)
        {
            finishedSecondLevelProcedure();
            playerScript.enemiesDefeated = true;
            SaveSystem.SaveGame();
            addKills();
        }
        else
        {
            StartCoroutine(hitFrame());
        }
    }

    void finishedSecondLevelProcedure()
    {
        bossHealthBar.bossEnd();
        audioManager.FadeOut("Second Dungeon Final Boss Fight", 0.2f);
        //play defeated music
        animator.SetTrigger("Death");
        death.Play();
        MiscData.dungeonLevelUnlocked = 2;
        // Temporary commented for testing purposes
        // bossManager.bossBeaten("arcane_adventurer", 2f); 
        //Also need to update player weapon unlock level
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            if (!shroud.IsActive())
            {
                dealDamageToBoss(collision.gameObject.GetComponent<DamageAmount>().damage);
            }
            bossManager.ylva.triggerFireball(this.transform.position);
        }
        else if (collision.gameObject.name == "Ylva's Fireball(Clone)")
        {
            if (shroud.IsActive())
            {
                rigidBody2D.velocity = Vector3.zero;
                shroud.fadeOut();
                shroudCoolDownTimer = 15;
            }
            else
            {
                dealDamageToBoss(8);
            }
        }
    }
}
