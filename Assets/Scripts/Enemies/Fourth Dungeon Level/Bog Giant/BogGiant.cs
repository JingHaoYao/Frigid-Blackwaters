using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BogGiant : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D takeDamageHitBox;
    public BossManager bossManager;
    private BossHealthBar healthBar;
    [SerializeField] Animator animator;
    [SerializeField] Sprite[] idleSprites;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] AudioSource takeDamageAudio, throwAudio, deathAudio;
    [SerializeField] GameObject obstaclehitBox;
    List<SmallBogGiant> smallBogGiants = new List<SmallBogGiant>();

    [SerializeField] GameObject bouncingBogBall, splittingBogBall;

    Camera mainCamera;
    int whatView = 1;
    int mirror = 1;
    bool isAttacking = false;
    private float attackPeriod = 3;
    int numberBogBallsThrown = 0;

    Vector3 centerPosition;

    int phase = 0;

    public void addSmallBogGiant(SmallBogGiant smallBogGiant)
    {
        smallBogGiants.Add(smallBogGiant);
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
    }

    private float angleToShip
    {
        get
        {
            return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
    }

    void pickIdleSprite()
    {
        transform.localScale = new Vector3(8 * mirror, 8);
        spriteRenderer.sprite = idleSprites[whatView - 1];
    }

    IEnumerator attackProcedure()
    {
        yield return new WaitForSeconds(1.5f);
        animator.enabled = false;
        while (true)
        {
            if (isAttacking == false)
            {
                pickView(angleToShip);
                pickIdleSprite();
                if (attackPeriod > 0)
                {
                    attackPeriod -= Time.deltaTime;
                }
                else
                {
                    if(numberBogBallsThrown < 4)
                    {
                        numberBogBallsThrown++;
                        StartCoroutine(throwBogBall());
                        attackPeriod = 1;
                    }
                    else
                    {
                        if(smallBogGiants.Count >= 4)
                        {
                            attackPeriod = 1;
                            numberBogBallsThrown = 0;
                            StartCoroutine(smallGiantAttack());
                        }

                    }
                }
            }

            yield return null;
        }
    }

    IEnumerator throwBogBall()
    {
        animator.enabled = true;
        phase = 1;
        pickView(angleToShip);
        isAttacking = true;
        float randAngle = Random.Range(0, Mathf.PI * 2);

        Vector3 throwPosition = PlayerProperties.playerShipPosition;
        if (Random.Range(0, 3) <= 1)
        {
            throwPosition = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * Random.Range(2.0f, 4.0f);

            while (Physics2D.OverlapCircle(throwPosition, 1f, 12))
            {
                randAngle = Random.Range(0, Mathf.PI * 2);
                throwPosition = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * Random.Range(2.0f, 4.0f);
            }
        }

        animator.SetTrigger("Throw" + whatView);

        yield return new WaitForSeconds(7 / 12f);

        GameObject bogProjectileInstant = Instantiate(Random.Range(0, 2) == 0 ? bouncingBogBall : splittingBogBall, transform.position, Quaternion.identity);
        bogProjectileInstant.GetComponent<BogGiantProjectile>().Initialize(this.gameObject, 0, new Vector3(Mathf.Clamp(throwPosition.x, centerPosition.x - 7.5f, centerPosition.x + 7.5f), Mathf.Clamp(throwPosition.y, centerPosition.y - 7.5f, centerPosition.y + 7.5f)), true);

        yield return new WaitForSeconds(4 / 12f);

        isAttacking = false;
        animator.enabled = false;
        phase = 0;
    }

    IEnumerator smallGiantAttack()
    {
        animator.enabled = true;
        isAttacking = true;
        takeDamageHitBox.enabled = false;
        obstaclehitBox.SetActive(false);
        animator.SetTrigger("Dissolve");
        phase = 2;

        yield return new WaitForSeconds(1.667f);

        spriteRenderer.enabled = false;

        foreach (SmallBogGiant small in smallBogGiants)
        {
            small.Initialize(this);
        }

        yield return new WaitForSeconds(1.083f);

        for(int i = 0; i < 4; i++)
        {
            foreach (SmallBogGiant small in smallBogGiants)
            {
                small.triggerAttack();
                yield return new WaitForSeconds(0.25f);
            }

            yield return new WaitForSeconds(11 / 12f);
        }

        foreach (SmallBogGiant small in smallBogGiants)
        {
            small.triggerDisappear();
        }


        smallBogGiants.Clear();

        yield return new WaitForSeconds(1.667f);

        animator.SetTrigger("Awaken");

        phase = 0;

        spriteRenderer.enabled = true;
        takeDamageHitBox.enabled = true;
        obstaclehitBox.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        animator.enabled = false;
        isAttacking = false;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(attackProcedure());
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.bossStartUp("Bog Giant");
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
        takeDamageHitBox.enabled = false;
        StopAllCoroutines();
        bossManager.bossBeaten(nameID, 1.5f);
        PlayerProperties.playerScript.enemiesDefeated = true;
        healthBar.bossEnd();
        deathAudio.Play();
        if (phase == 2)
        {
            foreach (SmallBogGiant small in smallBogGiants)
            {
                small.triggerDisappear();
            }
        }
        else
        {
            animator.enabled = true;
            animator.SetTrigger("Death");
            Destroy(this.gameObject, 1.5f);
        }
        SaveSystem.SaveGame();
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
