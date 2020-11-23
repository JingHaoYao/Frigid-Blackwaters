using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionSoul : Enemy
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] viewSprites;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] WhichRoomManager roomManager;
    [SerializeField] AudioSource damageAudio, awakenAudio, attackAudio, summonIllusionAudio;
    [SerializeField] Collider2D takeDamageHitbox;
    [SerializeField] GameObject treasureChest;
    [SerializeField] GameObject illusion;
    [SerializeField] GameObject trace;
    [SerializeField] GameObject projectile;
    BossHealthBar bossHealthBar;
    private bool dormant = true;
    private bool isAttacking = false;
    private float attackPeriod = 0;
    private Camera mainCamera;
    private int whatView = 1;
    private int mirror = 1;
    List<IllusionSoulIllusion> illusions = new List<IllusionSoulIllusion>();
    Coroutine illusionAttackRoutine;
    private int phase = 0;
    int numberBasicAttacks = 0;

    int numberProjectileAttacks = 0;

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
    }

    void setScale()
    {
        transform.localScale = new Vector3(4 * mirror, 4);
    }

    void pickSpriteAndScale()
    {
        pickView(angleToShip);
        spriteRenderer.sprite = viewSprites[whatView - 1];
        setScale();
    }

    IEnumerator awakenRoutine()
    {
        bossHealthBar.bossStartUp("Illusion Conductor");
        bossHealthBar.targetEnemy = this;
        animator.SetTrigger("Awaken");
        EnemyPool.addEnemy(this);
        roomManager.antiSpawnSpaceDetailer.spawnDoorSeals();
        PlayerProperties.playerScript.enemiesDefeated = false;
        dormant = false;
        yield return new WaitForSeconds(10 / 12f);
        awakenAudio.Play();
        yield return new WaitForSeconds(13 / 12f);
        StartCoroutine(attackLoop());
        animator.enabled = false;
    }

    IEnumerator basicAttack()
    {
        animator.enabled = true;
        isAttacking = true;
        pickView(angleToShip);
        setScale();
        float attackingAngle = angleToShip;
        animator.SetTrigger("Attack" + whatView);

        yield return new WaitForSeconds(6 / 12f);

        attackAudio.Play();
        GameObject instant = Instantiate(projectile, transform.position + new Vector3(Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(angleToShip * Mathf.Deg2Rad) + 1), Quaternion.identity);
        instant.GetComponent<BasicProjectile>().angleTravel = attackingAngle;
        instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;

        yield return new WaitForSeconds(6 / 12f);


        if (Vector2.Distance(transform.position, mainCamera.transform.position) > 0.5f)
        {
            StartCoroutine(moveBack());
        }
        else
        {
            animator.enabled = false;
            isAttacking = false;
        }

    }

    IEnumerator illusionAttack()
    {
        animator.enabled = true;
        isAttacking = true;
        animator.SetTrigger("SummonIllusions");
        summonIllusionAudio.Play();
        Vector3 originalPosition = transform.position;

        yield return new WaitForSeconds(18 / 12f);

        int whichPosition = Random.Range(0, 8);
        Vector3 lastPosition = transform.position;


        for (int i = 0; i < 8; i++)
        {
            Vector3 position = originalPosition + new Vector3(Mathf.Cos(i * 45 * Mathf.Deg2Rad), Mathf.Sin(i * 45 * Mathf.Deg2Rad)) * 6;
            if (i == whichPosition)
            {
                LeanTween.move(this.gameObject, position, 1f).setEaseOutCirc().setOnUpdate((float val) =>
                {
                    if (Vector2.Distance(lastPosition, transform.position) > 0.5f)
                    {
                        lastPosition = transform.position;
                        Instantiate(trace, transform.position, Quaternion.identity);
                    }
                });
            }
            else
            {
                GameObject illusionInstant = Instantiate(illusion, originalPosition, Quaternion.identity);
                IllusionSoulIllusion illusionScript = illusionInstant.GetComponent<IllusionSoulIllusion>();
                illusions.Add(illusionScript);
                illusionScript.Initialize(this, position);
            }
        }

        float period = 0;
        animator.enabled = false;

        phase = 1;

        while (period < 3)
        {
            period += Time.deltaTime;
            pickSpriteAndScale();
            yield return null;
        }

        phase = 2;

        illusionHit();
    }

    public void illusionHit(){

        phase = 2;
        foreach(IllusionSoulIllusion illusion in illusions)
        {
            illusion.triggerIllusionAttack();
        }
        illusions.Clear();
        StartCoroutine(basicAttack());
    }

    public void removeIllusions()
    {
        phase = 2;
        foreach(IllusionSoulIllusion illusion in illusions)
        {
            illusion.triggerIllusionDeath();
        }
        illusions.Clear();
    }

    IEnumerator moveBack()
    {
        Vector3 lastPosition = transform.position;
        LeanTween.move(this.gameObject, mainCamera.transform.position, 1f).setEaseOutCirc().setOnUpdate((float val) =>
        {
            if (Vector2.Distance(lastPosition, transform.position) > 0.5f)
            {
                lastPosition = transform.position;
                Instantiate(trace, transform.position, Quaternion.identity);
            }
        });

        yield return new WaitForSeconds(1f);

        phase = 0;
        animator.enabled = false;
        isAttacking = false;
    }

    private void Start()
    {
        mainCamera = Camera.main;
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
                attackPeriod += Time.deltaTime;

                pickSpriteAndScale();

                if (attackPeriod > 1.5f && stopAttacking == false)
                {
                    if(numberBasicAttacks < 2)
                    {
                        numberBasicAttacks++;
                        StartCoroutine(basicAttack());
                        attackPeriod = 0.5f;
                    }
                    else
                    {
                        numberBasicAttacks = 0;
                        illusionAttackRoutine = StartCoroutine(illusionAttack());
                        attackPeriod = 0;
                    }
                }
                
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            if (dormant == true && Vector2.Distance(mainCamera.transform.position, transform.position) < 4)
            {
                StartCoroutine(awakenRoutine());
                return;
            }

            if (health > 0)
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);

                if(Vector2.Distance(transform.position, mainCamera.transform.position) > 0.5f && phase == 1)
                {
                    StopCoroutine(illusionAttackRoutine);
                    StartCoroutine(moveBack());
                    removeIllusions();
                }
            }
        }
    }

    public override void deathProcedure()
    {
        animator.enabled = true;
        StopAllCoroutines();
        roomManager.antiSpawnSpaceDetailer.trialDefeated = true;
        PlayerProperties.playerScript.enemiesDefeated = true;
        animator.SetTrigger("Death");
        Destroy(this.gameObject, 18 / 12f);
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
