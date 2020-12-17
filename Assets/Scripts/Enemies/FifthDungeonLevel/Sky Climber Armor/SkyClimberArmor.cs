using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyClimberArmor : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D takeDamageHitBox;
    public BossManager bossManager;
    private BossHealthBar healthBar;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] AudioSource roarAudio, 
        phaseAudio, 
        startupAudio, 
        takeDamageAudio, 
        fireDashAudio,
        phaseAttackAudio, 
        armorDashAudio, 
        launchMissileAudio, 
        throwFireball, 
        bossDeathAudio,
        bossDeathExplosionAudio;
    [SerializeField] GameObject fireDash;
    [SerializeField] GameObject introShadow;
    [SerializeField] GameObject pyrotheumProjectile;
    [SerializeField] GameObject pyrotheumMissile;
    [SerializeField] GameObject missileTarget;
    [SerializeField] GameObject giantFireball;
    [SerializeField] AudioClip intro;

    AudioManager audioManager;

    Camera mainCamera;
    bool isAttacking = false;
    private float attackPeriod = 1;

    Vector3 centerPosition;

    private int whatView = 1;
    private int prevView = 0;
    private int mirror = 1;

    int whichAttack = 0;
    // 1 - phase attack
    // 2 - fire dash
    // 3 - throw fireballs
    // 4 - missiles
    int previousAttack = -1;

    void PickView(float angleOrientation)
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
        transform.localScale = new Vector3(mirror * 4, 4);
    }

    void PickIdleAnimation()
    {
        PickView(angleToShip(this.transform));
        if (whatView != prevView)
        {
            prevView = whatView;
            animator.Play("Idle View " + whatView);
        }
    }

    private float angleToShip(Transform sourceTransform)
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - sourceTransform.position.y, PlayerProperties.playerShipPosition.x - sourceTransform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void Initialize()
    {
        audioManager.MuteSound("Dungeon Ambiance");
        StartCoroutine(attackLoop());
    }

    IEnumerator playOpeningSong()
    {
        audioManager.PlaySound("Final Boss Intro Music");
        yield return new WaitForSeconds(intro.length - 0.05f);
        audioManager.PlaySound("Final Boss Music");
    }

    IEnumerator attackLoop()
    {
        takeDamageHitBox.enabled = false;
        this.spriteRenderer.enabled = false;

        yield return new WaitForSeconds(1f);

        GameObject shadowInstant = Instantiate(introShadow, new Vector3(1525, 0), Quaternion.Euler(0, 0, 90));
        LeanTween.move(shadowInstant, new Vector3(1475, 0), 1.5f).setEaseOutQuad().setOnComplete(() => Destroy(shadowInstant));

        yield return new WaitForSeconds(1.5f);

        spriteRenderer.enabled = true;
        animator.Play("Rise");
        phaseAudio.Play();
        startupAudio.Play();

        yield return new WaitForSeconds(8 / 9f);
        healthBar.bossStartUp("Dragon Armor, Rite of Fire");
        StartCoroutine(playOpeningSong());

        roarAudio.Play();

        PlayerProperties.playerScript.playerDead = false;

        yield return new WaitForSeconds(5 / 9f);

        animator.Play("Idle View 1");
        takeDamageHitBox.enabled = true;

        while (true)
        {
            if (isAttacking == false && stopAttacking == false)
            {
                if (attackPeriod > 0)
                {
                    attackPeriod -= Time.deltaTime;
                    PickIdleAnimation();
                }
                else
                {
                    whichAttack = Random.Range(1, 5);
                    while(whichAttack == previousAttack)
                    {
                        whichAttack = Random.Range(1, 5);
                    }
                    previousAttack = whichAttack;

                    switch(whichAttack)
                    {
                        case 1:
                            StartCoroutine(phaseAttack());
                            break;
                        case 2:
                            StartCoroutine(dashAttack());
                            break;
                        case 3:
                            StartCoroutine(launchMissilesAttack());
                            break;
                        case 4:
                            StartCoroutine(throwFireBallAttack());
                            break;
                    }

                    attackPeriod = 0.25f;
                }
            }
            yield return null;
        }
    }

    IEnumerator throwFireBallAttack()
    {
        isAttacking = true;

        float angleAttack = angleToShip(this.transform);

        PickView(angleAttack);

        animator.Play("Throw Fireball View " + whatView);

        yield return new WaitForSeconds(9 / 12f);

        throwFireball.Play();

        if (stopAttacking == false)
        {
            for (int i = 0; i < 9; i++)
            {
                float angleTravel = angleAttack - 60 + 15 * i;
                GameObject giantFireballInstant = Instantiate(giantFireball, transform.position + new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)), Quaternion.identity);
                giantFireballInstant.GetComponent<DragonCannonFireball>().Initialize(angleTravel * Mathf.Deg2Rad);
            }
        }

        yield return new WaitForSeconds(1 / 12f);

        if (stopAttacking == false)
        {
            for (int i = 0; i < 7; i++)
            {
                float angleTravel = angleAttack - 90 + 30 * i;
                GameObject giantFireballInstant = Instantiate(giantFireball, transform.position + new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)), Quaternion.identity);
                giantFireballInstant.GetComponent<DragonCannonFireball>().Initialize(angleTravel * Mathf.Deg2Rad);
            }
        }

        yield return new WaitForSeconds(1 / 12f);

        if (stopAttacking == false)
        {
            for (int i = 0; i < 5; i++)
            {
                float angleTravel = angleAttack - 30 + 15 * i;
                GameObject giantFireballInstant = Instantiate(giantFireball, transform.position + new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)), Quaternion.identity);
                giantFireballInstant.GetComponent<DragonCannonFireball>().Initialize(angleTravel * Mathf.Deg2Rad);
            }
        }

        yield return new WaitForSeconds(3 / 12f);

        isAttacking = false;
        prevView = -1;
    }

    IEnumerator blinkWhite()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.material.SetFloat("_FlashAmount", 1);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.material.SetFloat("_FlashAmount", 0);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator dashAttack()
    {
        isAttacking = true;

        float angleTravel = angleToShip(this.transform);

        PickView(angleTravel);

        animator.Play("Fire Dash View " + whatView);
        armorDashAudio.Play();
        StartCoroutine(blinkWhite());

        yield return new WaitForSeconds(6 / 9f);

        float offset = 0;

        if(whatView == 1)
        {
            offset = angleTravel - 270;
        }
        else if(whatView == 3)
        {
            offset = angleTravel - 90;
        }
        else
        {
            if(mirror == -1)
            {
                offset = angleTravel - 180;
            }
            else
            {
                offset = angleTravel;
            }
        }

        transform.rotation = Quaternion.Euler(0, 0, offset);

        SpawnPyrotheumProjectiles();
        Vector3 targetPosition = transform.position + new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * 45;
        LeanTween.move(this.gameObject, targetPosition, 1.5f);
        GameObject fireDashInstant = Instantiate(fireDash, transform.position, Quaternion.identity);
        fireDashInstant.GetComponent<SkyClimberFireWave>().Initialize(angleTravel, this.gameObject, targetPosition, 1.5f);

        yield return new WaitForSeconds(1.5f);
        transform.rotation = Quaternion.Euler(0, 0, 0);

        transform.position = centerPosition;
        spriteRenderer.enabled = true;
        animator.Play("Rise");
        phaseAudio.Play();

        yield return new WaitForSeconds(4 / 12f);

        prevView = -1;
        isAttacking = false;
    }

    void SpawnPyrotheumProjectiles()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject pyrotheumInstant = Instantiate(pyrotheumProjectile, transform.position + Vector3.up, Quaternion.identity);
            pyrotheumInstant.GetComponent<PyrotheumProjectile>().angleTravel = i * 45;
            pyrotheumInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
    }

    IEnumerator launchMissilesAttack()
    {
        isAttacking = true;
        animator.Play("Launch Missiles");

        phaseAttackAudio.Play();

        yield return new WaitForSeconds(8 / 12f);

        launchMissileAudio.Play();

        if (stopAttacking == false)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject pyrotheumMissileInstant = Instantiate(pyrotheumMissile, transform.position + Vector3.up, Quaternion.identity);
                float randAngle = Random.Range(0, 360);

                Vector3 randPosition = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(randAngle * Mathf.Deg2Rad), Mathf.Sin(randAngle * Mathf.Deg2Rad)) * Random.Range(0, 9);
                GameObject missileTargetInstant = Instantiate(missileTarget, randPosition, Quaternion.identity);
                LeanTween.alpha(missileTargetInstant, 0.75f, 1f).setOnComplete(() => LeanTween.alpha(missileTargetInstant, 0, 1f));

                pyrotheumMissileInstant.GetComponent<SkyClimberRocket>().Initialize(this.gameObject, randPosition);
            }
        }

        yield return new WaitForSeconds(9 / 12f);
        isAttacking = false;
        prevView = -1;
    }

    IEnumerator phaseAttack()
    {
        isAttacking = true;
        animator.Play("Phase Attack");
        phaseAttackAudio.Play();
        yield return new WaitForSeconds(12 / 12f);
        phaseAudio.Play();
        takeDamageHitBox.enabled = false;
        SpawnPyrotheumProjectiles();
        yield return new WaitForSeconds(2 / 12f);
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.enabled = true;
        animator.Play("Phase In");
        takeDamageHitBox.enabled = true;

        int travelDirection = Random.Range(0, 2) * 180;
        Vector3 travelVector = new Vector3(Mathf.Cos(travelDirection * Mathf.Deg2Rad), Mathf.Sin(travelDirection * Mathf.Deg2Rad));

        if (travelDirection == 0)
        {
            transform.localScale = new Vector3(4, 4);
            Vector3 desiredPosition = PlayerProperties.playerShipPosition + Vector3.left * 12;
            transform.position = new Vector3(Mathf.Clamp(desiredPosition.x, centerPosition.x - 18, centerPosition.x + 18), desiredPosition.y);

            yield return new WaitForSeconds(2 / 12f);

            for (int i = 0; i < 3; i++)
            {
                fireDashAudio.Play();
                Vector3 kickPosition = transform.position + travelVector * 8;
                LeanTween.move(this.gameObject, new Vector3(Mathf.Clamp(kickPosition.x, centerPosition.x - 18, centerPosition.x + 18), kickPosition.y), 0.25f);
                GameObject dashInstant = Instantiate(fireDash, transform.position, Quaternion.identity);
                dashInstant.GetComponent<SkyClimberFireWave>().Initialize(0, this.gameObject, kickPosition, 0.25f);

                yield return new WaitForSeconds(0.35f);
            }
        }
        else
        {
            transform.localScale = new Vector3(-4, 4);
            Vector3 desiredPosition = PlayerProperties.playerShipPosition + Vector3.right * 12;
            transform.position = new Vector3(Mathf.Clamp(desiredPosition.x, centerPosition.x - 18, centerPosition.x + 18), desiredPosition.y);

            yield return new WaitForSeconds(2 / 12f);

            for (int i = 0; i < 3; i++)
            {
                fireDashAudio.Play();
                Vector3 kickPosition = transform.position + travelVector * 8;
                LeanTween.move(this.gameObject, new Vector3(Mathf.Clamp(kickPosition.x, centerPosition.x - 18, centerPosition.x + 18), kickPosition.y), 0.25f);
                GameObject dashInstant = Instantiate(fireDash, transform.position, Quaternion.identity);
                dashInstant.GetComponent<SkyClimberFireWave>().Initialize(180, this.gameObject, kickPosition, 0.25f);

                yield return new WaitForSeconds(0.35f);
            }
        }

        animator.Play("Kick Return");

        yield return new WaitForSeconds(5 / 12f);

        prevView = -1;
        isAttacking = false;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.targetEnemy = this;
        EnemyPool.addEnemy(this);
        centerPosition = mainCamera.transform.position;
        audioManager = FindObjectOfType<AudioManager>();
        Initialize();
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

    IEnumerator deathRoutine()
    {
        animator.Play("Death");
        bossDeathAudio.Play();

        yield return new WaitForSeconds(5 / 12f);
        roarAudio.Play();

        yield return new WaitForSeconds(7 / 12f);
        bossDeathExplosionAudio.Play();

        yield return new WaitForSeconds(4 / 12f);
        spriteRenderer.enabled = false;
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        LeanTween.cancel(this.gameObject);
        takeDamageHitBox.enabled = false;
        //bossManager.bossBeaten(nameID, 1.5f);
        PlayerProperties.playerScript.enemiesDefeated = true;
        healthBar.bossEnd();
        spriteRenderer.material.color = Color.white;
        audioManager.FadeOut("Final Boss Music", 0.2f);
        PlayerProperties.playerScript.playerDead = true;
        if (MiscData.dungeonLevelUnlocked == 5)
        {
            MiscData.dungeonLevelUnlocked = 6;
        }

        StartCoroutine(deathRoutine());
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        takeDamageAudio.Play();
        SpawnArtifactKillsAndGoOnCooldown();
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.material.color = Color.white;
    }
}
