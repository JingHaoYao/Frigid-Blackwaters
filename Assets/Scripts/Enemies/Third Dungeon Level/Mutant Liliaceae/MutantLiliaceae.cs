using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantLiliaceae : Enemy {
    [SerializeField] Animator animator;
    [SerializeField] AudioSource takeDamageAudio, waveSplashAudio, flowerOpenAttackAudio, flowerAwakenAudio, flowerDeathAudio;
    [SerializeField] SpriteRenderer spriteRenderer;
    public CameraShake cameraShake;
    public MoveCameraNextRoom cameraScript;
    public GameObject invisibleWall;
    [SerializeField] GameObject willowNotification;
    [SerializeField] GameObject[] mushroomCovers;
    [SerializeField] GameObject dropPodProjectile;
    [SerializeField] LayerMask solidObstacleLayerMask;
    List<GameObject> spawnedPodJumpers = new List<GameObject>();
    [SerializeField] GameObject summoningFlower, summoningFlowerLatePhase, eatingVines;
    [SerializeField] GameObject podProjectile;
    [SerializeField] GameObject waveWall;
    BossHealthBar bossHealthBar;
    [SerializeField] DialogueSet mutantLiliaceaeInterstitialDialogue;
    public DialogueUI dialogueUI;
    public GameObject dialogueBlackOverlay;
    public MutantLiliaceaeBossManager bossManager;
    public AudioManager audioManager;

    private int bossPhase = 0;
    // 0 - player moving back to boss
    // 1 - player near boss
    // 2 - player being moved back to the start of the room

    private float attackTimer = 0;
    private int numberConsumed = 0;
    private int numberPodJumpersSpawned = 0;

    void Start()
    {
        bossHealthBar = FindObjectOfType<BossHealthBar>();
        EnemyPool.addEnemy(this);
        InitializeBoss();
    }

    void InitializeBoss()
    {
        StartCoroutine(openingAnimationSequence());
        PlayerProperties.playerScript.enemiesDefeated = false;
    }

    IEnumerator openingAnimationSequence()
    {
        cameraScript.freeCam = true;
        PlayerProperties.playerScript.playerDead = true;
        LeanTween.move(cameraScript.gameObject, new Vector3(1600, 60), 2f).setEaseOutQuad();
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("Awaken");
        flowerAwakenAudio.Play();
        audioManager.PlaySound("Final Boss Music");
        audioManager.FadeIn("Final Boss Music", 0.2f, 1f);
        yield return new WaitForSeconds(1.333f);
        LeanTween.move(cameraScript.gameObject, new Vector3(1600, 0), 1f).setEaseOutQuad();
        yield return new WaitForSeconds(1f);
        cameraScript.freeCam = false;
        cameraScript.trackPlayer = true;
        PlayerProperties.playerScript.playerDead = false;

        dialogueUI.LoadDialogueUI(mutantLiliaceaeInterstitialDialogue, 0f, () => StartCoroutine(mainEnemyLoop()));
    }

    IEnumerator mainEnemyLoop()
    {
        bossHealthBar.bossStartUp("Mutant Liliaceae");
        bossHealthBar.targetEnemy = this;
        while (true)
        {
            if (bossPhase == 0)
            {
                dropPodProjectileAttack();
            }
            else if (bossPhase == 1)
            {
                summonPodJumpers();
            }
            else if(bossPhase == 2)
            {
                if(Vector2.Distance(cameraScript.transform.position, new Vector3(1600, 0)) < 0.5f)
                {
                    bossPhase = 0;
                }
            }
            yield return null;
        }
    }

    public void addJumper(GameObject jumper)
    {
        spawnedPodJumpers.Add(jumper);
    }

    public void addConsumedJumper()
    {
        numberConsumed++;
    }

    void summonPodJumpers()
    {
        if (attackTimer < 3)
        {
            attackTimer += Time.deltaTime;
        }
        else
        {
            if (numberPodJumpersSpawned < 7)
            {
                attackTimer = 0;
                GameObject summoningFlowerInstant = Instantiate(((float)health/maxHealth) > 0.5f ? summoningFlower : summoningFlowerLatePhase, pickRandomPositionLastRoom(), Quaternion.identity);
                summoningFlowerInstant.GetComponent<MutantLiliaceaeSummoningFlower>().boss = this;
                numberPodJumpersSpawned++;
            }
            else
            {
                bossPhase = 2;
                StartCoroutine(eatPodJumpersAndAttack());
            }
        }
    }

    IEnumerator eatPodJumpersAndAttack()
    {
        animator.SetTrigger("StopSummoning");
        yield return new WaitForSeconds(4 / 12f);
        animator.SetTrigger("CloseFlower");
        yield return new WaitForSeconds(0.5f);
        foreach(GameObject jumper in spawnedPodJumpers)
        {
            if(jumper != null)
            {
                GameObject eatingVinesInstant = Instantiate(eatingVines, jumper.transform.position, Quaternion.identity);
                MutantLiliaceaeEatingVines vines = eatingVinesInstant.GetComponent<MutantLiliaceaeEatingVines>();
                vines.targetEnemy = jumper;
                vines.boss = this;
            }
        }
        yield return new WaitForSeconds(3f);
        animator.SetTrigger("OpenAttack");
        flowerOpenAttackAudio.Play();

        int totalNumberOfProjectiles = numberConsumed * 2;
        float angleIncrement = 180f / totalNumberOfProjectiles;
        for(int i = 0; i < totalNumberOfProjectiles; i++)
        {
            GameObject podProjectileInstant = Instantiate(podProjectile, transform.position, Quaternion.identity);
            podProjectileInstant.GetComponent<BasicProjectile>().angleTravel = (-180) + i * angleIncrement;
            podProjectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }

        yield return new WaitForSeconds(2);
        cameraScript.freeCam = true;
        cameraScript.trackPlayer = false;
        LeanTween.moveY(cameraScript.gameObject, Mathf.Clamp(PlayerProperties.playerShipPosition.y, 0, 60), 0.5f);
        yield return new WaitForSeconds(0.5f);
        invisibleWall.SetActive(false);
        cameraScript.freeCam = false;
        cameraScript.trackPlayer = true;
        attackTimer = 0;
        Instantiate(waveWall, new Vector3(1600, 60f), Quaternion.identity);
        waveSplashAudio.Play();
    }

    void dropPodProjectileAttack()
    {
        if(attackTimer < 5.5f)
        {
            attackTimer += Time.deltaTime;
        }
        else
        {
            if (Vector2.Distance(cameraScript.transform.position, new Vector3(1600, 60)) > 15)
            {
                StartCoroutine(sendInDroppingPods());
                attackTimer = 0;
            }
        }

        if(Vector2.Distance(cameraScript.transform.position, new Vector3(1600, 60)) < 9)
        {
            // transition to the boss attacking people
            invisibleWall.SetActive(true);
            LeanTween.move(cameraScript.gameObject, new Vector3(1600, 60), 0.5f);
            cameraScript.trackPlayer = false;
            bossPhase = 1;
            attackTimer = 0;
            numberConsumed = 0;
            numberPodJumpersSpawned = 0;
            spawnedPodJumpers.Clear();
            animator.SetTrigger("StartSummoning");
        }
    }

    IEnumerator sendInDroppingPods()
    {
        Instantiate(willowNotification, PlayerProperties.playerShipPosition, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        float randAngle = Random.Range(0, Mathf.PI * 2);
        Vector3 positionToSpawn = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * 4;
        while (!validifySpawnPosition(positionToSpawn))
        {
            randAngle = Random.Range(0, Mathf.PI * 2);
            positionToSpawn = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * 4;
        }

        GameObject mushroomInstant = Instantiate(mushroomCovers[Random.Range(0, mushroomCovers.Length)], positionToSpawn, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        for(int i = 0; i < 3; i++)
        {
            GameObject dropPodInstant = Instantiate(dropPodProjectile, PlayerProperties.playerShipPosition, Quaternion.identity);
            dropPodInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;

            for(int k = 0; k < 5; k++)
            {
                dropPodInstant = Instantiate(dropPodProjectile, pickRandomPosition(), Quaternion.identity);
                dropPodInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(1f);
        mushroomInstant.GetComponent<GuardMushroom>().Sink();
    }

    Vector3 pickRandomPosition()
    {
        return new Vector3(cameraScript.transform.position.x + Random.Range(-8.0f, 8.0f), cameraScript.transform.position.y + Random.Range(-8.0f, 8.0f));
    }

    Vector3 pickRandomPositionLastRoom()
    {
        return new Vector3(cameraScript.transform.position.x + Random.Range(-8.0f, 8.0f), cameraScript.transform.position.y + Random.Range(-8.0f, 4.0f));
    }

    bool validifySpawnPosition(Vector3 pos)
    {
        return !Physics2D.OverlapCircle(pos, 0.75f, solidObstacleLayerMask);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (health > 0 && PlayerProperties.playerScript.playerDead == false)
        {
            if (collision.gameObject.GetComponent<DamageAmount>() && bossPhase != 0)
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            }
        }
    }

    public override void deathProcedure()
    {
        finishedThirdLevelProcedure();
        PlayerProperties.playerScript.enemiesDefeated = true;
        SaveSystem.SaveGame();
        addKills();
    }

    private void finishedThirdLevelProcedure()
    {
        audioManager.FadeOut("Final Boss Music", 0.2f);
        bossHealthBar.bossEnd();
        StopAllCoroutines();
        animator.SetTrigger("Death");
        flowerDeathAudio.Play();
        PlayerProperties.playerScript.playerDead = true;
        if (MiscData.dungeonLevelUnlocked == 3)
        {
            MiscData.dungeonLevelUnlocked = 4;
        }
        cameraScript.freeCam = true;
        cameraScript.trackPlayer = false;
        bossManager.bossBeaten(this.nameID, 2f);
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
