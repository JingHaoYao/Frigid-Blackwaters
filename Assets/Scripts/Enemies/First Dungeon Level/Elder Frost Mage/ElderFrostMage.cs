using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderFrostMage : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    public Sprite facingDown, facingDownRight, facingUp, facingUpLeft;
    int whatView = 0;
    int mirrorScale = 1;
    float angleToShip;
    GameObject playerShip;
    float attackPeriod = 5;
    public GameObject icePillar, iceMissle;
    bool isAttacking = false;
    bool summonMissile = false;
    int numberPillars = 0;
    public List<GameObject> spawnedPillars = new List<GameObject>();
    public GameObject activateBlast;
    int numberPillarsBeforeExplode = 3;
    Rigidbody2D rigidBody2D;

    Vector3 initPlayerPos;

    public GameObject waterSplash;
    public GameObject waterFoam;
    float foamTimer = 0;

    public BossManager bossManager;

    Vector3 targetTravel;

    Camera mainCamera;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    void pickView(float angle)
    {
        if (angle > 255 && angle <= 285)
        {
            whatView = 1;
            mirrorScale = 1;
        }
        else if (angle > 285 && angle <= 360)
        {
            whatView = 2;
            mirrorScale = 1;
        }
        else if (angle > 180 && angle <= 255)
        {
            whatView = 2;
            mirrorScale = -1;
        }
        else if (angle > 75 && angle <= 105)
        {
            whatView = 3;
            mirrorScale = 1;
        }
        else if (angle > 0 && angle <= 75)
        {
            whatView = 4;
            mirrorScale = -1;
        }
        else
        {
            whatView = 4;
            mirrorScale = 1;
        }
    }

    void pickSprite()
    {
        if(whatView == 1)
        {
            spriteRenderer.sprite = facingDown;
        }
        else if(whatView == 2)
        {
            spriteRenderer.sprite = facingDownRight;
        }
        else if(whatView == 3)
        {
            spriteRenderer.sprite = facingUp;
        }
        else
        {
            spriteRenderer.sprite = facingUpLeft;
        }
        transform.localScale = new Vector3(0.2f * mirrorScale, 0.2f);
    }

    IEnumerator summonPillar()
    {
        isAttacking = true;
        attackPeriod = 1.5f;
        animator.enabled = true;
        animator.SetTrigger("Pillar" + whatView.ToString());
        this.GetComponents<AudioSource>()[3].Play();
        yield return new WaitForSeconds(5f / 12f);
        Vector3 spawnPos = playerShip.transform.position;
        yield return new WaitForSeconds(4f / 12f);
        GameObject pillar = Instantiate(icePillar, spawnPos, Quaternion.identity);
        pillar.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        if(Random.Range(0,2) == 1)
        {
            Vector3 scale = pillar.transform.localScale;
            pillar.transform.localScale = new Vector3(-scale.x, scale.y);
        }
        spawnedPillars.Add(pillar);
        yield return new WaitForSeconds(3f / 12f);
        animator.enabled = false;
        isAttacking = false;
    }

    IEnumerator shootMissile()
    {
        isAttacking = true;
        attackPeriod = 2f;
        animator.enabled = true;
        animator.SetTrigger("Missile" + whatView.ToString());
        this.GetComponents<AudioSource>()[2].Play();
        yield return new WaitForSeconds(6f / 12f);

        float angleFromShipToBoss = Mathf.Atan2(mainCamera.transform.position.y - PlayerProperties.playerShipPosition.y, mainCamera.transform.position.x - PlayerProperties.playerShipPosition.x);
        for(int i = 0; i < 3; i++)
        {
            GameObject missileInstant = Instantiate(iceMissle, PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angleFromShipToBoss + (-15 + (15 * i)) * Mathf.Deg2Rad), Mathf.Sin(angleFromShipToBoss + (-15 + (15 * i)) * Mathf.Deg2Rad)) * (Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) - 1.5f), Quaternion.identity);
            missileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        yield return new WaitForSeconds(7f / 12f);
        animator.enabled = false;
        isAttacking = false;
    }

    IEnumerator shootActivate()
    {
        isAttacking = true;
        attackPeriod = 3;
        animator.enabled = true;
        animator.SetTrigger("Missile" + whatView.ToString());
        this.GetComponents<AudioSource>()[2].Play();
        yield return new WaitForSeconds(6f / 12f);
        summonActivateBlast();
        yield return new WaitForSeconds(7f / 12f);
        animator.enabled = false;
        isAttacking = false;
        spawnedPillars.Clear();
    }

    void summonActivateBlast()
    {
        GameObject missileInstant = Instantiate(activateBlast, transform.position + new Vector3(spawnedPillars[0].transform.position.x - transform.position.x, spawnedPillars[0].transform.position.y - transform.position.y).normalized * 0.5f + new Vector3(0, 1.5f, 0), Quaternion.identity);
        foreach(GameObject pillar in spawnedPillars)
        {
            missileInstant.GetComponent<ElderFrostMageActivateBlast>().pillarsToActivate.Add(pillar);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShip = GameObject.Find("PlayerShip");
        initPlayerPos = playerShip.transform.position;
        FindObjectOfType<BossHealthBar>().targetEnemy = GetComponent<Enemy>();
        FindObjectOfType<BossHealthBar>().bossStartUp("Elder Frost Mage");
        StartCoroutine(mainGameloop());
        mainCamera = Camera.main;
    }

    IEnumerator mainGameloop()
    {
        yield return new WaitForSeconds(8 / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(16 / 12f);
        animator.enabled = false;
        while (true)
        {
            if (health > 0)
            {
                spawnFoam();
                angleToShip = (360 + (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg)) % 360;
                pickView(angleToShip);
                if (attackPeriod > 0)
                {
                    attackPeriod -= Time.deltaTime;
                    if (isAttacking == false)
                    {
                        pickSprite();
                        if (Vector2.Distance(initPlayerPos, playerShip.transform.position) > 4 || Vector2.Distance(targetTravel, transform.position) > 0.2f)
                        {
                            targetTravel = Camera.main.transform.position + new Vector3(Camera.main.transform.position.x - playerShip.transform.position.x, Camera.main.transform.position.y - playerShip.transform.position.y).normalized * 6f;
                            rigidBody2D.velocity = new Vector3(targetTravel.x - transform.position.x, targetTravel.y - transform.position.y).normalized * speed;
                            initPlayerPos = playerShip.transform.position;
                        }
                        else
                        {
                            rigidBody2D.velocity = Vector3.zero;
                        }
                    }
                    else
                    {
                        rigidBody2D.velocity = Vector3.zero;
                    }
                }
                else
                {
                    if (summonMissile == true || numberPillars >= numberPillarsBeforeExplode)
                    {
                        if (summonMissile == false && numberPillars >= numberPillarsBeforeExplode)
                        {
                            numberPillars = 0;
                            numberPillarsBeforeExplode = Random.Range(3, 6);
                            StartCoroutine(shootActivate());
                        }
                        else
                        {
                            StartCoroutine(shootMissile());
                            summonMissile = false;
                        }
                    }
                    else
                    {
                        StartCoroutine(summonPillar());
                        summonMissile = true;
                        numberPillars++;
                    }
                }
            }
            else
            {
                animator.enabled = true;
                rigidBody2D.velocity = Vector3.zero;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    public override void deathProcedure()
    {
        rigidBody2D.velocity = Vector3.zero;
        bossManager.bossBeaten(nameID, 0.8f);
        animator.enabled = true;
        animator.SetTrigger("Death");
        FindObjectOfType<BossHealthBar>().bossEnd();
        this.GetComponents<AudioSource>()[1].Play();
    }

    public override void damageProcedure(int damage)
    {
        this.GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }
}
