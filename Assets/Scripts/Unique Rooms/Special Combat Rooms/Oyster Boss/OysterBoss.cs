using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OysterBoss : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    PlayerScript playerScript;
    WhichRoomManager whichRoomManager;
    Rigidbody2D rigidBody2D;

    int mirror = 1;
    int whatView = 0;

    public GameObject oysterChest;
    public GameObject spine;

    public GameObject invulnerableHitBox;
    public LayerMask directionPickFilter;
    public GameObject invulnerableIcon;
    public float travelAngle;
    public float pickTravelDuration = 0;

    public int[] cardinalAngles = new int[4] { 0, 90, 180, 270 };

    float shootSpikesDuration = 0;

    public Sprite[] shieldedSprites;
    public Sprite[] unShieldedSprites;

    public GameObject waterFoam;
    public GameObject dormantOysterBoss;

    float foamTimer = 0;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.035f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    void turnOffAnimator()
    {
        animator.enabled = false;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerScript = FindObjectOfType<PlayerScript>();
        whichRoomManager = GetComponent<WhichRoomManager>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        FindObjectOfType<BossHealthBar>().bossStartUp("Spiked Scuttler");
        FindObjectOfType<BossHealthBar>().targetEnemy = this;
        travelAngle = cardinalAngles[Random.Range(0, cardinalAngles.Length)];
        Invoke("turnOffAnimator", 0.45f);
        EnemyPool.addEnemy(this);
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
    }

    void pickView(float angle)
    {
        if(angle >= 22.5f && angle < 67.5f)
        {
            whatView = 4;
            mirror = 1;
        }
        else if(angle >= 67.5f && angle < 112.5f)
        {
            whatView = 5;
            mirror = 1;
        }
        else if(angle >= 112.5f && angle < 157.5f)
        {
            whatView = 4;
            mirror = -1;
        }
        else if(angle >= 157.5f && angle < 202.5f)
        {
            whatView = 3;
            mirror = -1;
        }
        else if(angle >= 202.5f && angle < 247.5f)
        {
            whatView = 2;
            mirror = -1;
        }
        else if(angle >= 247.5f && angle < 292.5f)
        {
            whatView = 1;
            mirror = 1;
        }
        else if(angle >= 292.5f && angle < 337.5f)
        {
            whatView = 2;
            mirror = 1;
        }
        else
        {
            whatView = 3;
            mirror = 1;
        }
    }

    void pickNewTravelDirection()
    {
        Vector3 dir1 = new Vector3(Mathf.Cos((travelAngle + 90) * Mathf.Deg2Rad), Mathf.Sin((travelAngle + 90) * Mathf.Deg2Rad));
        Vector3 dir2 = new Vector3(Mathf.Cos((travelAngle - 90) * Mathf.Deg2Rad), Mathf.Sin((travelAngle - 90) * Mathf.Deg2Rad));

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), dir1, 20, directionPickFilter);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), dir2, 20, directionPickFilter);

        float[] hitDistances = new float[2] { hit1.distance, hit2.distance };
        float smallestDistance = Mathf.Max(hitDistances);
        int index = System.Array.IndexOf(hitDistances, smallestDistance);

        if (index == 0)
        {
            travelAngle += 90;
        }
        else
        {
            travelAngle -= 90;
        }
        travelAngle = (travelAngle + 360) % 360;
    }

    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Oyster Boss Rise") && health > 0)
        {
            pickView(travelAngle);

            transform.localScale = new Vector3(5f * mirror, 5f);

            pickTravelDuration -= Time.deltaTime;
            if (pickTravelDuration <= 0 /*|| (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > 8.5f || Mathf.Abs(transform.position.y - Camera.main.transform.position.y) > 8.5f) */)
            {
                pickTravelDuration = 2;
                pickNewTravelDirection();
            }

            shootSpikesDuration += Time.deltaTime;
            if (shootSpikesDuration > 7)
            {
                shootSpikesDuration = 0;
                StartCoroutine(attack());
            }
            else
            {
                if (animator.enabled == true)
                {
                    rigidBody2D.velocity = Vector3.zero;
                }
                else
                {
                    spawnFoam();
                    moveTowards(travelAngle);
                }
            }
            
            if(invulnerableHitBox.activeSelf == true)
            {
                spriteRenderer.sprite = shieldedSprites[whatView - 1];
            }
            else
            {
                spriteRenderer.sprite = unShieldedSprites[whatView - 1];
            }
        }
    }
    
    IEnumerator attack()
    {
        animator.enabled = true;
        animator.SetTrigger("Attack" + whatView.ToString());
        yield return new WaitForSeconds(3f /12f);
        invulnerableHitBox.SetActive(false);
        invulnerableIcon.SetActive(false);
        GetComponents<AudioSource>()[0].Play();
        shootSpikes();
        yield return new WaitForSeconds(3 / 12f);
        animator.enabled = false;
        yield return new WaitForSeconds(2.5f);
        invulnerableHitBox.SetActive(true);
        invulnerableHitBox.SetActive(true);
    }

    void shootSpikes()
    {
        for(int i = 0; i < 8; i++)
        {
            float angle = i * 45 * Mathf.Deg2Rad;
            GameObject quill = Instantiate(spine, transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 1.5f, Quaternion.identity);
            quill.GetComponent<BasicProjectile>().angleTravel = i * 45;
            quill.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    void spawnDormant()
    {
        GameObject dormant = Instantiate(dormantOysterBoss, transform.position, Quaternion.identity);
        dormant.GetComponent<DormantOysterBoss>().enabled = false;
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Oyster Boss Rise") && health > 0 && invulnerableHitBox.activeSelf == false)
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            }
        }
        else if (collision.tag != "EnemyShield")
        {
            pickTravelDuration = 2;
            pickNewTravelDirection();
        }
    }

    public override void deathProcedure()
    {
        rigidBody2D.velocity = Vector3.zero;
        StopAllCoroutines();
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        whichRoomManager.antiSpawnSpaceDetailer.trialDefeated = true;
        playerScript.enemiesDefeated = true;
        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = true;
        animator.enabled = true;
        animator.SetTrigger("Death");
        Invoke("spawnDormant", 0.917f);
        FindObjectOfType<BossHealthBar>().bossEnd();

        if (transform.position.y < Camera.main.transform.position.y)
        {
            Instantiate(oysterChest, Camera.main.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(oysterChest, Camera.main.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
        }
    }

    public override void damageProcedure(int damage)
    {
        GetComponents<AudioSource>()[1].Play();
        StartCoroutine(hitFrame());
        SpawnArtifactKillsAndGoOnCooldown(1f);
    }
}
