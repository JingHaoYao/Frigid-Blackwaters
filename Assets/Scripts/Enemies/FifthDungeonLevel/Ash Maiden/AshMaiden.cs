using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AshMaiden : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D takeDamageHitBox;
    public BossManager bossManager;
    private BossHealthBar healthBar;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] AudioSource takeDamageAudio, spearAttackAudio, deathAudio;

    Camera mainCamera;
    bool isAttacking = false;
    private float attackPeriod = 3;

    Vector3 centerPosition;

    List<AshMaidenSpear> allSpears = new List<AshMaidenSpear>();
    [SerializeField] List<AshMaidenPyre> allAshMaidenPyres = new List<AshMaidenPyre>();
    List<AshMaidenPyre> unUsedPyres = new List<AshMaidenPyre>();
    float rotationOffsetSpears = 0;

    private float angleToShip
    {
        get
        {
            return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
    }

    IEnumerator movementLoop()
    {
        while (true)
        {
            LeanTween.move(this.gameObject, transform.position + Vector3.up * 0.5f, 1f).setEaseInOutQuad();
            yield return new WaitForSeconds(1f);
            LeanTween.move(this.gameObject, transform.position - Vector3.up * 0.5f, 1f).setEaseInOutQuad();
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator attackRoutine()
    {
        while(true)
        {
            if(isAttacking == false)
            {
                attackPeriod -= Time.deltaTime;
                if (attackPeriod <= 0)
                {
                    if (allSpears.Count > 6)
                    {
                        if(allSpears.Count > 12)
                        {
                            StartCoroutine(spearAttack());
                            continue;
                        }

                        if(Random.Range(0, 13) < allSpears.Count)
                        {
                            StartCoroutine(spearAttack());
                            continue;
                        }
                    }

                    if(unUsedPyres.Count == 0)
                    {
                        StartCoroutine(reSummonPyres(() => { }));
                        continue;
                    }

                    StartCoroutine(ignitePyres());
                }
            }

            yield return null;
        }
    }

    public void AddSpear(AshMaidenSpear spear)
    {
        allSpears.Add(spear);
        spear.Initialize(this.gameObject);
    }

    IEnumerator updateSpears()
    {
        while (true)
        {
            rotationOffsetSpears += Time.deltaTime * 120;
            if (allSpears.Count > 0)
            {
                float offset = (360 / allSpears.Count);
                foreach (AshMaidenSpear spear in allSpears)
                {
                    Vector3 offsetPosition = transform.position + new Vector3(Mathf.Cos((offset * allSpears.IndexOf(spear) + rotationOffsetSpears) * Mathf.Deg2Rad), Mathf.Sin((offset * allSpears.IndexOf(spear) + rotationOffsetSpears) * Mathf.Deg2Rad)) * 2;
                    if (Vector2.Distance(spear.transform.position, offsetPosition) > 0.1f)
                    {
                        spear.transform.position += (offsetPosition - spear.transform.position).normalized * Time.deltaTime * 8;
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator ignitePyres()
    {
        animator.Play("Ash Maiden Ignite Pyre");
        isAttacking = true;
        yield return new WaitForSeconds(8 / 12f);
        for(int i = 0; i < 2; i++)
        {
            AshMaidenPyre selectedPyre = unUsedPyres[Random.Range(0, unUsedPyres.Count)];
            selectedPyre.IgnitePyre();
            unUsedPyres.Remove(selectedPyre);
        }
        yield return new WaitForSeconds(7 / 12f);
        animator.Play("Ash Maiden Idle");
        isAttacking = false;
        attackPeriod = 2f;
    }

    IEnumerator spearAttack()
    {
        attackPeriod = 2f + Mathf.Clamp(allSpears.Count * 0.1f - 2, 0, float.MaxValue);
        animator.Play("Ash Maiden Spear Attack");
        isAttacking = true;
        yield return new WaitForSeconds(9 / 12f);
        spearAttackAudio.Play();

        StartCoroutine(launchSpears());

        yield return new WaitForSeconds(6 / 12f);
        animator.Play("Ash Maiden Idle");
        isAttacking = false;
    }

    IEnumerator launchSpears()
    {
        foreach (AshMaidenSpear spear in allSpears)
        {
            spear.PrepAndFireSpear(angleToShip * Mathf.Deg2Rad);
            yield return new WaitForSeconds(0.1f);
        }
        allSpears.Clear();
    }

    IEnumerator reSummonPyres(UnityAction onComplete)
    {
        animator.Play("Ash Maiden Summon Pyres");
        isAttacking = true;
        yield return new WaitForSeconds(8 / 12f);
        unUsedPyres.Clear();
        foreach (AshMaidenPyre pyre in allAshMaidenPyres)
        {
            pyre.RisePyre();
            unUsedPyres.Add(pyre);
        }
        yield return new WaitForSeconds(7 / 12f);
        animator.Play("Ash Maiden Idle");
        isAttacking = false;
        onComplete.Invoke();
        attackPeriod = 2f;
    }

    void Initialize()
    {
        foreach(AshMaidenPyre pyre in allAshMaidenPyres)
        {
            pyre.Initialize(this.gameObject);
        }
        StartCoroutine(reSummonPyres(() => StartCoroutine(attackRoutine())));
    }

    private void Start()
    {
        mainCamera = Camera.main;
        Initialize();
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.bossStartUp("Ash Maiden");
        healthBar.targetEnemy = this;
        EnemyPool.addEnemy(this);
        centerPosition = mainCamera.transform.position;
        StartCoroutine(updateSpears());
        StartCoroutine(movementLoop());
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
        takeDamageHitBox.enabled = false;
        //bossManager.bossBeaten(nameID, 1.5f);
        PlayerProperties.playerScript.enemiesDefeated = true;
        healthBar.bossEnd();
        deathAudio.Play();
        animator.enabled = true;
        animator.Play("Ash Maiden Death");
        Destroy(this.gameObject, 10/12f);
        SaveSystem.SaveGame();
        foreach(AshMaidenSpear spear in allSpears)
        {
            spear.FadeOutSpear();
        }
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
