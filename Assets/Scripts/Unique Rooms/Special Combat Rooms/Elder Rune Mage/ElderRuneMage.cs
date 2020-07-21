using System.Collections;
using UnityEngine;

public class ElderRuneMage : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    PlayerScript playerScript;
    WhichRoomManager whichRoomManager;
    Rigidbody2D rigidBody2D;

    int mirror = 1;
    int whatView = 0;

    public Vector3[] magePositions;
    int currentPosition = 0;
    // 0 - center
    // 1 - top left
    // 2 - top right
    // 3 - bottom left
    // 4 - bottom right

    public Sprite[] viewSprites;

    public GameObject bolt;
    public GameObject depthBomb;
    public GameObject mageChest;
    public GameObject deadMage;

    float attackPeriod = 3;
    float movePeriod = 8;
    bool nextAttackDepthBomb = false;

    void pickView(float angle)
    {
        if (angle > 255 && angle <= 285)
        {
            whatView = 1;
            mirror = 1;
        }
        else if (angle > 285 && angle <= 360)
        {
            whatView = 2;
            mirror = 1;
        }
        else if (angle > 180 && angle <= 255)
        {
            whatView = 2;
            mirror = -1;
        }
        else if (angle > 75 && angle <= 105)
        {
            whatView = 3;
            mirror = 1;
        }
        else if (angle >= 0 && angle <= 75)
        {
            whatView = 4;
            mirror = -1;
        }
        else
        {
            whatView = 4;
            mirror = 1;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerScript = FindObjectOfType<PlayerScript>();
        whichRoomManager = GetComponent<WhichRoomManager>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        FindObjectOfType<BossHealthBar>().bossStartUp("Elder Rune Mage");
        FindObjectOfType<BossHealthBar>().targetEnemy = this;
        EnemyPool.addEnemy(this);
        StartCoroutine(startUp());
    }

    IEnumerator startUp()
    {
        yield return new WaitForSeconds(10/12f);
        GetComponents<AudioSource>()[4].Play();
        yield return new WaitForSeconds(4 / 12f + 0.1f);
        animator.enabled = false;
    }

    void moveToNewPosition()
    {
        int newPos = Random.Range(0, 5);

        while(newPos == currentPosition)
        {
            newPos = Random.Range(0, 5);
        }

        Vector3 newPosition = Camera.main.transform.position + magePositions[newPos];


        LeanTween.move(this.gameObject, newPosition, Vector2.Distance(newPosition, transform.position) / speed).setEaseOutCirc()
            .setOnComplete(() =>
            {
                transform.position = newPosition;
                rigidBody2D.velocity = Vector3.zero;
                currentPosition = newPos;
                movePeriod = 8;
            });
    }

    IEnumerator summonDepthBombs(float angle, Vector3 startPos)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector3 spawnPosition = startPos;
        while (Mathf.Abs(Camera.main.transform.position.x - spawnPosition.x) < 7.5f && Mathf.Abs(Camera.main.transform.position.y - spawnPosition.y) < 7.5f)
        {
            spawnPosition += directionVector * 2.5f;
        }
        spawnPosition += directionVector * -2.5f;
        while (Mathf.Abs(Camera.main.transform.position.x - spawnPosition.x) < 8f && Mathf.Abs(Camera.main.transform.position.y - spawnPosition.y) < 8f)
        {
            GameObject instant = Instantiate(depthBomb, spawnPosition, Quaternion.identity);
            instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            spawnPosition += directionVector * -2.5f;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator boltAttack()
    {
        animator.enabled = true;
        int currview = whatView;
        animator.SetTrigger("BoltAttack" + whatView.ToString());
        GetComponents<AudioSource>()[2].Play();
        yield return new WaitForSeconds(9f / 12f);
        summonBolt(currview);
        yield return new WaitForSeconds(8f / 12f);
        animator.enabled = false;
        attackPeriod = 1.5f;
    }

    IEnumerator depthBombAttack()
    {
        animator.enabled = true;
        animator.SetTrigger("Attack" + whatView.ToString());
        yield return new WaitForSeconds(4f / 12f);
        float randAngle = Random.Range(0, 8) * 45;
        Vector3 randPos = new Vector3(Camera.main.transform.position.x + Random.Range(-6.0f, 6.0f), Camera.main.transform.position.y + Random.Range(-6.0f, 6.0f));
        StartCoroutine(summonDepthBombs(randAngle, randPos));
        StartCoroutine(summonDepthBombs(randAngle + 90, randPos));
        GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(12f / 12f);
        animator.enabled = false;
        attackPeriod = 0.5f;
    }

    void summonBolt(int whichView)
    {
        GameObject boltInstant;
        if(whichView == 1)
        {
            boltInstant = Instantiate(bolt, transform.position + new Vector3(0.936f, 2.998f), Quaternion.identity);
        }
        else if(whichView == 2)
        {
            if(mirror == 1)
            {
                boltInstant = Instantiate(bolt, transform.position + new Vector3(-0.43f, 2.68f), Quaternion.identity);
            }
            else
            {
                boltInstant = Instantiate(bolt, transform.position + new Vector3(0.43f, 2.68f), Quaternion.identity);
            }
        }
        else if(whichView == 3)
        {
            boltInstant = Instantiate(bolt, transform.position + new Vector3(1.072f, 3.376f), Quaternion.identity);
        }
        else
        {
            if (mirror == 1)
            {
                boltInstant = Instantiate(bolt, transform.position + new Vector3(-1.16f, 3.19f), Quaternion.identity);
            }
            else
            {
                boltInstant = Instantiate(bolt, transform.position + new Vector3(1.16f, 3.19f), Quaternion.identity);
            }
        }
        boltInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
    }

    void Update()
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Elder Rune Mage Rise") && health > 0)
        {
            if (animator.enabled == false)
            {
                float angleToShip = (360 + Mathf.Atan2(playerScript.transform.position.y - transform.position.y, playerScript.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
                pickView(angleToShip);
                spriteRenderer.sprite = viewSprites[whatView - 1];
                transform.localScale = new Vector3(5f * mirror, 5f);
            }

            if(animator.enabled == false && movePeriod > 0 && Mathf.Abs(rigidBody2D.velocity.magnitude) < 0.001f)
            {
                attackPeriod -= Time.deltaTime;
                movePeriod -= Time.deltaTime;
                if(attackPeriod <= 0)
                {
                    if (nextAttackDepthBomb)
                    {
                        nextAttackDepthBomb = false;
                        StartCoroutine(depthBombAttack());
                    }
                    else
                    {
                        nextAttackDepthBomb = true;
                        StartCoroutine(boltAttack());
                    }
                }

                if(attackPeriod > 0 && movePeriod <= 0)
                {
                    moveToNewPosition();
                }
            }
        }
    }

    IEnumerator deathRoutine()
    {
        animator.enabled = true;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(6f / 12f);
        GetComponents<AudioSource>()[5].Play();
        yield return new WaitForSeconds(11f / 12f);
        Destroy(this.gameObject);
        Instantiate(deadMage, transform.position, Quaternion.identity);
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Elder Rune Mage Rise") && health > 0)
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);

            }
        }
    }

    public override void deathProcedure()
    {
        rigidBody2D.velocity = Vector3.zero;
        StopAllCoroutines();
        whichRoomManager.antiSpawnSpaceDetailer.trialDefeated = true;
        playerScript.enemiesDefeated = true;
        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = true;

        StartCoroutine(deathRoutine());
        FindObjectOfType<BossHealthBar>().bossEnd();

        if (transform.position.y < Camera.main.transform.position.y)
        {
            Instantiate(mageChest, Camera.main.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(mageChest, Camera.main.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
        }
    }

    public override void damageProcedure(int damage)
    {
        GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }
}
