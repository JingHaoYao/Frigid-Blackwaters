using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinderHero : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D takeDamageHitBox;
    public BossManager bossManager;
    private BossHealthBar healthBar;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] AudioSource takeDamageAudio, dashAudio, summonSwordsAudio, swordAttackAudio, deathAudio, riseAudio;
    [SerializeField] GameObject damageHitBox;
    [SerializeField] GameObject pyrotheumProjectile;

    List<CinderHeroDashEffect> allDashEffects = new List<CinderHeroDashEffect>();
    [SerializeField] GameObject dashEffect;
    [SerializeField] GameObject swordProjectile;
    List<CinderHeroSwordProjectile> allSwordProjectiles = new List<CinderHeroSwordProjectile>();
    Vector3 currentSwordCenter;

    Camera mainCamera;
    bool isAttacking = false;
    private float attackPeriod = 2;

    Vector3 centerPosition;

    private int whatView = 1;
    private int prevView = 0;
    private int mirror = 1;

    void PickView(float angleOrientation)
    {
        if (angleOrientation >= 0 && angleOrientation < 60)
        {
            whatView = 3;
            mirror = 1;
        }
        else if (angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 4;
            mirror = 1;
        }
        else if (angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 3;
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
        transform.localScale = new Vector3(mirror * 5, 5);
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

    void SpawnPyrotheumProjectiles()
    {
        for(int i = 0; i < 8; i++)
        {
            GameObject pyrotheumInstant = Instantiate(pyrotheumProjectile, transform.position + Vector3.up, Quaternion.identity);
            pyrotheumInstant.GetComponent<PyrotheumProjectile>().angleTravel = i * 45;
            pyrotheumInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
    }

    void Initialize()
    {
        StartCoroutine(attackLoop());
        damageHitBox.SetActive(false);
    }

    IEnumerator attackLoop()
    {
        riseAudio.Play();
        animator.Play("Cinder Hero Rise");

        yield return new WaitForSeconds(14f / 12f);

        animator.Play("Idle View 1");

        while (true)
        {
            if(isAttacking == false)
            {
                rigidBody2D.velocity = new Vector3(Mathf.Cos(angleToShip(this.transform) * Mathf.Deg2Rad), Mathf.Sin(angleToShip(this.transform) * Mathf.Deg2Rad)) * speed;
                PickIdleAnimation();
                if(attackPeriod > 0)
                {
                    attackPeriod -= Time.deltaTime;
                }
                else
                {
                    if(allSwordProjectiles.Count == 0)
                    {
                        StartCoroutine(summonSwords());
                        attackPeriod = 1;
                    }
                    else
                    {
                        StartCoroutine(dashAttack());
                        attackPeriod = 0.25f;
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

    IEnumerator dashAttack()
    {
        dashAudio.Play();
        isAttacking = true;
        float angleAttack = angleToShip(this.transform);
        PickView(angleAttack);
        animator.Play("Swing View " + whatView);
        yield return new WaitForSeconds(10 / 12f);
        damageHitBox.SetActive(true);
        damageHitBox.transform.rotation = Quaternion.Euler(0, 0, angleAttack + 90);
        Vector3 lastSpawnPosition = transform.position + Vector3.up;
        SpawnPyrotheumProjectiles();
        LeanTween.move(this.gameObject, transform.position + new Vector3(Mathf.Cos(angleAttack * Mathf.Deg2Rad), Mathf.Sin(angleAttack * Mathf.Deg2Rad)) * 6, 4 / 12f)
            .setOnUpdate(
            (float val) =>
            {
                if (Vector2.Distance(lastSpawnPosition, transform.position) > 1)
                {
                    bool enabled = false;
                    foreach (CinderHeroDashEffect effect in allDashEffects)
                    {
                        if (effect.gameObject.activeSelf == false)
                        {
                            effect.Initialize(transform.position, whatView, mirror);
                            lastSpawnPosition = transform.position;
                            enabled = true;
                            break;
                        }
                    }
                    if (enabled == false)
                    {
                        GameObject effectInstant = Instantiate(dashEffect, transform.position, Quaternion.identity);
                        CinderHeroDashEffect dashEffectScript = effectInstant.GetComponent<CinderHeroDashEffect>();
                        allDashEffects.Add(dashEffectScript);
                        dashEffectScript.Initialize(transform.position, whatView, mirror);
                        lastSpawnPosition = transform.position;
                    }
                }
            }
            );
        yield return new WaitForSeconds(4 / 12f);
        damageHitBox.SetActive(false);
        yield return new WaitForSeconds(3 / 12f);
        isAttacking = false;
        prevView = -1;
    }

    IEnumerator summonSwords()
    {
        isAttacking = true;
        animator.Play("Summon Swords");
        summonSwordsAudio.Play();
        yield return new WaitForSeconds(16 / 12f);
        for(int i = 0; i < 6; i++)
        {
            GameObject swordInstant = Instantiate(swordProjectile, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            allSwordProjectiles.Add(swordInstant.GetComponent<CinderHeroSwordProjectile>());
            swordInstant.GetComponent<CinderHeroSwordProjectile>().Initialize(this.gameObject, transform.position + Vector3.up * 0.5f + new Vector3(Mathf.Cos((Mathf.PI / 3) * i), Mathf.Sin((Mathf.PI / 3) * i)), (Mathf.PI / 3) * i);
        }
        currentSwordCenter = transform.position + Vector3.up * 0.5f;
        yield return new WaitForSeconds(4 / 12f);
        isAttacking = false;
        prevView = -1;
    }

    public void RemoveSword(CinderHeroSwordProjectile projectile)
    {
        this.allSwordProjectiles.Remove(projectile);
    }

    private void Start()
    {
        mainCamera = Camera.main;
        Initialize();
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.bossStartUp("The Unnamed War Machine");
        healthBar.targetEnemy = this;
        EnemyPool.addEnemy(this);
        centerPosition = mainCamera.transform.position;
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

    public override void deathProcedure()
    {
        StopAllCoroutines();
        LeanTween.cancel(this.gameObject);
        takeDamageHitBox.enabled = false;
        //bossManager.bossBeaten(nameID, 1.5f);
        PlayerProperties.playerScript.enemiesDefeated = true;
        healthBar.bossEnd();
        deathAudio.Play();
        animator.enabled = true;
        animator.Play("Cinder Hero Death");
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        takeDamageAudio.Play();
        SpawnArtifactKillsAndGoOnCooldown();
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
