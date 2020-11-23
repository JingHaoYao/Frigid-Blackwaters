using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireReaper : Enemy
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] WhichRoomManager roomManager;
    [SerializeField] AudioSource damageAudio;
    [SerializeField] Collider2D takeDamageHitbox;
    BossHealthBar bossHealthBar;
    private bool dormant = true;
    private float attackPeriod = 0.5f;
    private Camera mainCamera;
    [SerializeField] GameObject treasureChest;
    [SerializeField] GameObject reaperScythe;
    [SerializeField] GameObject pedestal;
    FireReaperScythe fireReaperScythe;
    bool isAttacking = false;

    Vector3 groundPosition;
    private int whatView = 0;
    private int mirror = 1;
    private int prevView = 0;

    bool hookAttack = false;

    [SerializeField] GameObject fireWall;

    List<FireReaperWallFire> allWallFires = new List<FireReaperWallFire>();

    IEnumerator awakenRoutine()
    {
        EnemyPool.addEnemy(this);
        dormant = false;
        roomManager.antiSpawnSpaceDetailer.spawnDoorSeals();
        PlayerProperties.playerScript.enemiesDefeated = false;
        bossHealthBar.bossStartUp("Fire Reaper");
        bossHealthBar.targetEnemy = this;
        groundPosition = transform.position - Vector3.up * 1.5f;
        fireReaperScythe.StartUp(this.gameObject);
        StartCoroutine(spawnFiresBottomLeft());
        StartCoroutine(spawnFiresTopRight());

        animator.Play("Reaper Rise");

        yield return new WaitForSeconds(6 / 12f);

        animator.Play("Idle View 1");

        StartCoroutine(moveLoop());
    }

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
        transform.localScale = new Vector3(mirror * 4, 4);
    }

    void PickIdleAnimation()
    {
        PickView(angleToShip);
        if (whatView != prevView)
        {
            prevView = whatView;
            animator.Play("Idle View " + whatView);
        }
    }

    IEnumerator moveLoop()
    {
        while(true)
        {
            if(Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 3 & isAttacking == false) {
                groundPosition += new Vector3(Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(angleToShip * Mathf.Deg2Rad)) * speed * Time.deltaTime;
                transform.position = groundPosition + Vector3.up * 1.5f;

                PickIdleAnimation();
                if (whatView >= 3)
                {
                    fireReaperScythe.UpdatePosition(transform.position - Vector3.up * 0.5f, spriteRenderer.sortingOrder + 2);
                }
                else
                {
                    fireReaperScythe.UpdatePosition(transform.position - Vector3.up * 0.5f, spriteRenderer.sortingOrder - 2);
                }

            }
            else
            {
                if(isAttacking == false)
                {
                    if (hookAttack == false)
                    {
                        prevView = -1;
                        isAttacking = true;
                        if (whatView >= 3)
                        {
                            fireReaperScythe.UpdatePosition(transform.position - Vector3.up * 0.5f, spriteRenderer.sortingOrder - 2);
                        }
                        else
                        {
                            fireReaperScythe.UpdatePosition(transform.position - Vector3.up * 0.5f, spriteRenderer.sortingOrder + 2);
                        }

                        if (stopAttacking == false)
                        {
                            fireReaperScythe.swipeAttack(angleToShip - 89, angleToShip + 89, () => { isAttacking = false; });
                        }

                        hookAttack = true;
                        speed += 3;
                    }
                    else
                    {
                        prevView = -1;
                        isAttacking = true;
                        if (whatView >= 3)
                        {
                            fireReaperScythe.UpdatePosition(transform.position - Vector3.up * 0.5f, spriteRenderer.sortingOrder - 2);
                        }
                        else
                        {
                            fireReaperScythe.UpdatePosition(transform.position - Vector3.up * 0.5f, spriteRenderer.sortingOrder + 2);
                        }

                        if (stopAttacking == false)
                        {
                            fireReaperScythe.hookAttack(angleToShip - 89, () => { isAttacking = false; });
                        }

                        hookAttack = false;
                        speed -= 3;
                    }
                }
            }


            yield return null;
        }
    }

    IEnumerator spawnFiresBottomLeft()
    {
        GameObject instant = Instantiate(fireWall, mainCamera.transform.position + new Vector3(-7.5f, -7.5f), Quaternion.identity);
        Vector3 startingPosition = mainCamera.transform.position + new Vector3(-7.5f, -7.5f);
        FireReaperWallFire fire = instant.GetComponent<FireReaperWallFire>();
        fire.Initialize(this.gameObject);
        allWallFires.Add(fire);

        for (int i = 0; i < 15; i++)
        {
            instant = Instantiate(fireWall, mainCamera.transform.position + startingPosition + Vector3.up * (i + 1), Quaternion.identity);
            fire = instant.GetComponent<FireReaperWallFire>();
            fire.Initialize(this.gameObject);
            allWallFires.Add(fire);

            instant = Instantiate(fireWall, mainCamera.transform.position + startingPosition + Vector3.right * (i + 1), Quaternion.identity);
            fire = instant.GetComponent<FireReaperWallFire>();
            fire.Initialize(this.gameObject);
            allWallFires.Add(fire);

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator spawnFiresTopRight()
    {
        GameObject instant = Instantiate(fireWall, mainCamera.transform.position + new Vector3(7.5f, 7.5f), Quaternion.identity);
        Vector3 startingPosition = mainCamera.transform.position + new Vector3(7.5f, 7.5f);
        FireReaperWallFire fire = instant.GetComponent<FireReaperWallFire>();
        fire.Initialize(this.gameObject);
        allWallFires.Add(fire);

        for (int i = 0; i < 15; i++)
        {
            instant = Instantiate(fireWall, mainCamera.transform.position + startingPosition + Vector3.down * (i + 1), Quaternion.identity);
            fire = instant.GetComponent<FireReaperWallFire>();
            fire.Initialize(this.gameObject);
            allWallFires.Add(fire);

            instant = Instantiate(fireWall, mainCamera.transform.position + startingPosition + Vector3.left * (i + 1), Quaternion.identity);
            fire = instant.GetComponent<FireReaperWallFire>();
            fire.Initialize(this.gameObject);
            allWallFires.Add(fire);

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        GameObject reaperScytheInstant = Instantiate(reaperScythe, transform.position - Vector3.up * 0.5f, Quaternion.Euler(0, 0, 90));
        fireReaperScythe = reaperScytheInstant.GetComponent<FireReaperScythe>();
        fireReaperScythe.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        fireReaperScythe.UpdatePosition(transform.position - Vector3.up * 0.5f, spriteRenderer.sortingOrder - 2);
        bossHealthBar = FindObjectOfType<BossHealthBar>();
        Instantiate(pedestal, transform.position - Vector3.up * 1.9f, Quaternion.identity);
    }

    private float angleToShip
    {
        get
        {
            return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
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
        animator.Play("Reaper Death");
        LeanTween.move(this.gameObject, mainCamera.transform.position, 1f).setOnUpdate((float val) => { fireReaperScythe.UpdatePosition(transform.position - Vector3.up * 0.5f, spriteRenderer.sortingOrder - 2); });
        FadeOutAllFires();
        fireReaperScythe.DieDown();
        fireReaperScythe.UpdatePosition(transform.position - Vector3.up * 0.5f, spriteRenderer.sortingOrder - 2);
        bossHealthBar.bossEnd();
        takeDamageHitbox.enabled = false;
        Instantiate(treasureChest, mainCamera.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
    }

    void FadeOutAllFires()
    {
        foreach(FireReaperWallFire fire in allWallFires)
        {
            fire.FadeOut();
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
