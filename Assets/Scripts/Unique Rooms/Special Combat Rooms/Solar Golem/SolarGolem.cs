using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarGolem : Enemy
{
    public WhichRoomManager roomManager;
    BossHealthBar bossHealthBar;
    [SerializeField] private BoxCollider2D takeDamageHitBox;
    public GameObject solarGolemChest;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AudioSource takeDamageAudio;
    [SerializeField] AudioSource startUpAudio;
    [SerializeField] SolarGolemTop solarGolemTop;
    [SerializeField] SolarGolemMiddle solarGolemMiddle;
    private bool dormant = true;
    int numberOfTimesSprayedPellets = 0;
    private float attackPeriod = 2;
    Camera mainCamera;

    void Start()
    {
        solarGolemMiddle.Initialize(spriteRenderer.sortingOrder);
        solarGolemTop.Initialize(spriteRenderer.sortingOrder);
        mainCamera = Camera.main;
        bossHealthBar = FindObjectOfType<BossHealthBar>();
    }

    IEnumerator attackLoop()
    {
        while (true)
        {
            if (attackPeriod > 0)
            {
                attackPeriod -= Time.deltaTime;
            }
            else
            {
                if (stopAttacking == false)
                {
                    if (numberOfTimesSprayedPellets < 2)
                    {
                        numberOfTimesSprayedPellets++;
                        solarGolemTop.pelletAttack();
                        attackPeriod = 4.5f;
                    }
                    else
                    {
                        numberOfTimesSprayedPellets = 0;
                        solarGolemMiddle.laserBeamAttack();
                        attackPeriod = 4;
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator awakenRoutine()
    {
        bossHealthBar.bossStartUp("Solar Golem");
        bossHealthBar.targetEnemy = this;
        solarGolemMiddle.startUp();
        solarGolemTop.startUp();
        startUpAudio.Play();
        EnemyPool.addEnemy(this);
        roomManager.antiSpawnSpaceDetailer.spawnDoorSeals();
        PlayerProperties.playerScript.enemiesDefeated = false;
        yield return new WaitForSeconds(7 / 12f);
        dormant = false;
        StartCoroutine(attackLoop());
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
            }
        }
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        roomManager.antiSpawnSpaceDetailer.trialDefeated = true;
        PlayerProperties.playerScript.enemiesDefeated = true;
        solarGolemTop.dieDown();
        solarGolemMiddle.dieDown();
        bossHealthBar.bossEnd();
        takeDamageHitBox.enabled = false;
        Instantiate(solarGolemChest, transform.position + new Vector3(0, -2, 0), Quaternion.identity);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        solarGolemTop.toggleHitFrame();
        solarGolemMiddle.toggleHitFrame();
        takeDamageAudio.Play();
        SpawnArtifactKillsAndGoOnCooldown(2f);
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
