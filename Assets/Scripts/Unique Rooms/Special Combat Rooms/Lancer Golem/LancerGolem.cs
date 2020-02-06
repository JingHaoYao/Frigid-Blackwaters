using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerGolem : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    WhichRoomManager whichRoomManager;
    PlayerScript playerScript;
    Rigidbody2D rigidBody2D;
    public GameObject lanceHitBox;
    int whatView = 1;
    int prevView = 1;
    bool attacking = false;
    public GameObject deadGolem;
    public SwordFist[] swordFists;
    float dashPeriod = 0;
    float dashPeriod2 = 3;
    public GameObject aStarGrid;
    GameObject gridInstant;
    public GameObject golemChest;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        whichRoomManager = GetComponent<WhichRoomManager>();
        playerScript = FindObjectOfType<PlayerScript>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        gridInstant = Instantiate(aStarGrid, Camera.main.transform.position, Quaternion.identity);
        FindObjectOfType<BossHealthBar>().bossStartUp("Lancer Golem");
        FindObjectOfType<BossHealthBar>().targetEnemy = this;
    }

    IEnumerator attack()
    {
        attacking = true;
        rigidBody2D.velocity = Vector3.zero;
        animator.SetTrigger("Attack" + whatView.ToString());
        yield return new WaitForSeconds(2f / 12f);
        GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(4f / 12f);
        lanceHitBox.GetComponents<PolygonCollider2D>()[whatView - 1].enabled = true;
        yield return new WaitForSeconds(1f / 12f);
        lanceHitBox.GetComponents<PolygonCollider2D>()[whatView - 1].enabled = false;
        yield return new WaitForSeconds(2f / 12f);
        attacking = false;
    }

    void pickView(float angleOrientation)
    {
        if(angleOrientation >= 0 && angleOrientation < 60)
        {
            whatView = 3;
        }
        else if(angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 4;
        }
        else if(angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 5;
        }
        else if(angleOrientation >= 180 && angleOrientation < 240)
        {
            whatView = 6;
        }
        else if(angleOrientation >= 240 && angleOrientation < 300)
        {
            whatView = 1;
        }
        else
        {
            whatView = 2;
        }
    }

    void pickIdleAnimation()
    {
        if (prevView != whatView)
        {
            animator.SetTrigger("Idle" + whatView.ToString());
            prevView = whatView;
        }
    }

    private void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Lancer Golem Rise") && health > 0)
        {
            float angleToShip = (360 + Mathf.Atan2(playerScript.transform.position.y - transform.position.y, playerScript.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            pickView(angleToShip);
            if(Vector2.Distance(playerScript.transform.position, transform.position) < 3 && attacking == false)
            {
                StartCoroutine(attack());
            }
            else
            {
                if (attacking == false)
                {
                    pickIdleAnimation();
                    rigidBody2D.velocity = (playerScript.transform.position - transform.position).normalized * speed;
                }
            }

            if(dashPeriod > 5)
            {
                swordFists[0].dashAttack();
                dashPeriod = 0;
            }


            if (dashPeriod2 > 5)
            {
                swordFists[1].dashAttack();
                dashPeriod2 = 0;
            }

            dashPeriod += Time.deltaTime;
            dashPeriod2 += Time.deltaTime;
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    void spawnDeadGolem()
    {
        rigidBody2D.velocity = Vector3.zero;
        GameObject _deadGolem = Instantiate(deadGolem, transform.position, Quaternion.identity);
        _deadGolem.GetComponent<DormantLancerGolem>().enabled = false;
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Lancer Golem Rise") && health > 0)
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);

            }
        }
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        Destroy(gridInstant);
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        whichRoomManager.antiSpawnSpaceDetailer.trialDefeated = true;
        playerScript.enemiesDefeated = true;
        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = true;
        animator.SetTrigger("Death");
        FindObjectOfType<BossHealthBar>().bossEnd();
        Invoke("spawnDeadGolem", 0.917f);
        GetComponents<AudioSource>()[3].Play();
        foreach (PolygonCollider2D col in lanceHitBox.GetComponents<PolygonCollider2D>())
        {
            col.enabled = false;
        }

        if (transform.position.y < Camera.main.transform.position.y)
        {
            Instantiate(golemChest, Camera.main.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(golemChest, Camera.main.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
        }
    }

    public override void damageProcedure(int damage)
    {
        GetComponents<AudioSource>()[2].Play();
        StartCoroutine(hitFrame());
    }
}
