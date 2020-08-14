using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBrassGolem : Enemy
{
    [SerializeField] BrassGolemHead head;
    [SerializeField] Animator animator;
    [SerializeField] BrassGolemGatlingGunArm gatlingArm;
    [SerializeField] BrassGolemFlameThrowerArm flameThrowerArm;

    [SerializeField] AudioSource emergeAudio, energyChargeAudio, energyReleaseAudio, damageAudio;
    [SerializeField] SpriteRenderer spriteRenderer;
    BossHealthBar bossHealthBar;
    [SerializeField] CameraShake cameraShake;
    [SerializeField] MoveCameraNextRoom cameraScript;
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameObject staticProjectile;
    [SerializeField] AudioClip intro;
    float attackPeriod = 2;
    private int numberStaticAttacks;

    public List<NymphCannon> cannons;

    [SerializeField] GameObject lightningEffect;

    public NymphCannonIndicator cannonIndicator;

    private float armAttackPeriod = 2;
    bool gatlingGunAttack = false;

    bool isAttacking = false;

    int prevCannonIndex = 0;

    public void readyForOpenSequence()
    {
        head.SetToUnactive();
        gatlingArm.setToUnactive();
        flameThrowerArm.setToUnactive();
        spriteRenderer.enabled = false;
    }

    IEnumerator playOpeningSong()
    {
        audioManager.MuteSound("Dungeon Ambiance");
        audioManager.PlaySound("Final Boss Intro Music");
        audioManager.FadeIn("Final Boss Intro Music", 0.2f, 0.8f);
        yield return new WaitForSeconds(intro.length);
        audioManager.PlaySound("Final Boss Music");
    }

    public void StartRemoveArmor()
    {
        StartCoroutine(removeArmor());
    }

    IEnumerator procCannons()
    {
        while (true)
        {
            yield return new WaitForSeconds(16f);
            int whichCannonToProc = Random.Range(0, 5);

            while(whichCannonToProc == prevCannonIndex)
            {
                whichCannonToProc = Random.Range(0, 5);
            }

            prevCannonIndex = whichCannonToProc;

            cannons[whichCannonToProc].StartCannonProcedure();
            cannonIndicator.StartFollowCannon(cannons[whichCannonToProc].transform.position);
        }
    }

    public void StopIndicator()
    {
        cannonIndicator.StopFollowCannon();
    }

    IEnumerator removeArmor()
    {
        armorMitigation = 0;
        // need some visual feedback here
        StartCoroutine(spawnLightningEffects());

        yield return new WaitForSeconds(5f);

        armorMitigation = 4;
    }

    IEnumerator spawnLightningEffects()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(lightningEffect, transform.position + Vector3.up * 8 + new Vector3(Random.Range(-4.0f, 4.0f), Random.Range(-4.0f, 4.0f)), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Instantiate(lightningEffect, transform.position + Vector3.up * 8 + new Vector3(Random.Range(-4.0f, 4.0f), Random.Range(-4.0f, 4.0f)), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator openingAnimation()
    {
        StartCoroutine(playOpeningSong());

        cameraScript.transform.position = new Vector3(1400, 0);

        LeanTween.moveLocalY(cameraScript.gameObject, 10, 2f);

        yield return new WaitForSeconds(1f);

        spriteRenderer.enabled = true;
        animator.SetTrigger("Rise");
        emergeAudio.Play();

        yield return new WaitForSeconds(13 / 12f);

        head.StartLoadOut();
        gatlingArm.StartLoadOut();
        flameThrowerArm.StartLoadOut();

        yield return new WaitForSeconds(3.8f);


        LeanTween.moveLocalY(cameraScript.gameObject, 0, 1.5f);

        yield return new WaitForSeconds(1f);

        // Trigger whatever, bossmanager
    }

    IEnumerator mainLoop()
    {
        while (true)
        {
            if(attackPeriod <= 0)
            {
                if(Random.Range(0, 3) < numberStaticAttacks)
                {
                    numberStaticAttacks = 0;
                    head.SpawnBomb();
                }
                else
                {
                    StartCoroutine(staticAttack());
                    numberStaticAttacks++;
                }

                attackPeriod = 3;
            }
            else
            {
                attackPeriod -= Time.deltaTime;
            }

            if(armAttackPeriod <= 0)
            {
                if (gatlingGunAttack)
                {
                    armAttackPeriod = 6;

                    if ((float)health / maxHealth < 0.5f)
                    {
                        armAttackPeriod -= 2;
                    }

                    float angleToShip = (Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg + 360) % 360;

                    if(angleToShip > 225)
                    {
                        if(angleToShip > 315)
                        {
                            gatlingArm.rightGatlingGunAttack();
                        }
                        else
                        {
                            gatlingArm.centerGatlingGunAttack();
                        }
                    }
                    else
                    {
                        gatlingArm.leftGatlingGunAttack();
                    }
                    gatlingGunAttack = false;
                }
                else
                {
                    gatlingGunAttack = true;
                    armAttackPeriod = 10;

                    if((float)health / maxHealth < 0.5f)
                    {
                        armAttackPeriod -= 2;
                    }

                    float angleToShip = (Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg + 360) % 360;

                    if (angleToShip > 270)
                    {
                        flameThrowerArm.leftSwipe();
                    }
                    else
                    {
                        flameThrowerArm.rightSwipe();
                    }
                }
            }
            else
            {
                armAttackPeriod -= Time.deltaTime;
            }

            yield return null;
        }
    }

    IEnumerator staticAttack()
    {
        isAttacking = true;
        Vector3 spawnPosition = transform.position + Vector3.up * 7.5f;

        animator.SetTrigger("StaticAttack");
        energyChargeAudio.Play();

        yield return new WaitForSeconds(4 / 12f);

        energyReleaseAudio.Play();

        for(int i = 0; i < 3; i++)
        {
            float randomInterval = Random.Range(15, 36);
            float launchingAngle = 0;

            if (i % 2 == 0)
            {
                launchingAngle = Random.Range(240, 300);
            }
            else
            {
                launchingAngle = Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg;
            }

            if((float)health/maxHealth > 0.5f) {
                for (int k = 0; k < 3; k++)
                {
                    float attackingAngle = launchingAngle - randomInterval * 1 + randomInterval * k;

                    GameObject instant = Instantiate(staticProjectile, spawnPosition, Quaternion.identity);
                    instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                    instant.GetComponent<BasicProjectile>().angleTravel = attackingAngle;
                }
            }
            else
            {
                for (int k = 0; k < 5; k++)
                {
                    float attackingAngle = launchingAngle - randomInterval * 2 + randomInterval * k;

                    GameObject instant = Instantiate(staticProjectile, spawnPosition, Quaternion.identity);
                    instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                    instant.GetComponent<BasicProjectile>().angleTravel = attackingAngle;
                }
            }
            yield return new WaitForSeconds(2/12f);
        }

        yield return new WaitForSeconds(8 / 12f);

        isAttacking = false;
        animator.SetTrigger("Idle");
    }

    public void InitializeBoss()
    {
        bossHealthBar.targetEnemy = this;
        bossHealthBar.bossStartUp("The Blue Steel Golem");
        prevCannonIndex = Random.Range(0, cannons.Count);
        StartCoroutine(mainLoop());
        StartCoroutine(procCannons());
    }

    public void startOpenAnimation()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(openingAnimation());
    }

    private void Start()
    {
        readyForOpenSequence();
        bossHealthBar = FindObjectOfType<BossHealthBar>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (health > 0 && PlayerProperties.playerScript.playerDead == false)
        {
            if (collision.gameObject.GetComponent<DamageAmount>())
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            }
        }
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        finishedFourthLevelProcedure();
        StartDeathAnimations();
        spriteRenderer.color = Color.white;
        PlayerProperties.playerScript.enemiesDefeated = true;
        SaveSystem.SaveGame();
        addKills();
    }

    void finishedFourthLevelProcedure()
    {
        bossHealthBar.bossEnd();
        LeanTween.moveLocal(cameraShake.gameObject, new Vector3(1400, 10, 0), 1f).setEaseInOutCubic();
        audioManager.FadeOut("Final Boss Music", 0.2f);
        PlayerProperties.playerScript.playerDead = true;
        if (MiscData.dungeonLevelUnlocked == 4)
        {
            MiscData.dungeonLevelUnlocked = 5;
        }
        cameraScript.freeCam = true;
        cameraScript.trackPlayer = false;
        // Gotta do something after this

        // TODO: Start up ending dialogue, trigger boss manager
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        head.startHitFrame();
        gatlingArm.startHitFrame();
        flameThrowerArm.startHitFrame();
        damageAudio.Play();
    }

    void StartDeathAnimations()
    {
        StartCoroutine(deathAnimation());
        head.StartHeadDeath();
        gatlingArm.armDieDown();
        flameThrowerArm.armDieDown();
    }

    IEnumerator deathAnimation()
    {
        animator.SetTrigger("Death");
        energyChargeAudio.Play();

        yield return new WaitForSeconds(6 / 12f);

        energyReleaseAudio.Play();
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
