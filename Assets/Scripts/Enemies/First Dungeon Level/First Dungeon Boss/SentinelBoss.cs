using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelBoss : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    GameObject playerShip;
    Rigidbody2D rigidBody2D;
    public Sprite[] armLessSprites;
    int mirror = 0;
    int prevView = 0;
    int whatView = 1;
    float attackPeriod = 5;
    int fistAttackCount = 0;
    float angleToShip = 0;

    public GameObject warningCircleFist;
    GameObject spawnedFist;
    bool fistAttacking = false;
    bool scatterRockAnimation = false;
    public GameObject deadBoss;

    public GameObject scatterRock;

    [SerializeField] BossManager bossManager;

    void pickView(float direction)
    {
        if (direction > 75 && direction < 105)
        {
            whatView = 3;
            mirror = 1;
        }
        else if (direction < 285 && direction > 265)
        {
            whatView = 1;
            mirror = 1;
        }
        else if (direction >= 285 && direction <= 360)
        {
            whatView = 2;
            mirror = 1;
        }
        else if (direction >= 180 && direction <= 265)
        {
            whatView = 2;
            mirror = -1;
        }
        else if (direction < 180 && direction >= 105)
        {
            whatView = 4;
            mirror = 1;
        }
        else
        {
            whatView = 4;
            mirror = -1;
        }
    }

    void pickIdle()
    {
        animator.SetTrigger("Idle" + whatView.ToString());
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShip = FindObjectOfType<PlayerScript>().gameObject;
        PlayerProperties.playerScript.removeRootingObject();
        EnemyPool.addEnemy(this);
    }

    void Update()
    {
        if (Camera.main.GetComponent<MoveCameraNextRoom>().freeCam == false)
        {
            rigidBody2D.velocity = new Vector3(Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(angleToShip * Mathf.Deg2Rad), 0) * speed;
        }
        angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        pickView(angleToShip);
        transform.localScale = new Vector3(15.5f * mirror, 15.5f);
        if(attackPeriod > 0)
        {
            attackPeriod -= Time.deltaTime;
            if (fistAttacking == false)
            {
                if (scatterRockAnimation == false)
                {
                    if (prevView != whatView)
                    {
                        pickIdle();
                        prevView = whatView;
                    }
            }
            }
            else
            {
                spriteRenderer.sprite = armLessSprites[whatView - 1];
            }
        }
        else
        {
            if(fistAttackCount >= 2)
            {
                attackPeriod = 12;
                StartCoroutine(fistAttack());
                fistAttackCount = 0;
            }
            else
            {
                attackPeriod = 6;
                StartCoroutine(shatterRockAttack());
                fistAttackCount++;
            }
        }

        if((float)health / 40 <= 0.5f)
        {
            foreach (SentinelRotateRock rock in FindObjectsOfType<SentinelRotateRock>())
            {
                rock.targetSpeed = 60;
            }
        }
    }

    IEnumerator fistAttack()
    {
        fistAttacking = true;
        animator.SetTrigger("Fist" + whatView.ToString());
        yield return new WaitForSeconds(4f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(3f / 12f);
        animator.enabled = false;
        for(int i = 0; i < 3; i++)
        {
            GameObject circle = Instantiate(warningCircleFist, playerShip.transform.position, Quaternion.identity);
            circle.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            yield return new WaitForSeconds(3f);
        }
        animator.enabled = true;
        animator.SetTrigger("Retract" + whatView.ToString());
        yield return new WaitForSeconds(1f / 12f);
        this.GetComponents<AudioSource>()[2].Play();
        yield return new WaitForSeconds(5f / 12f);
        fistAttacking = false;
    }

    IEnumerator shatterRockAttack()
    {
        scatterRockAnimation = true;
        animator.SetTrigger("Scatter Rock");
        yield return new WaitForSeconds(6f / 12f);
        this.GetComponents<AudioSource>()[3].Play();
        this.GetComponents<AudioSource>()[4].Play();
        yield return new WaitForSeconds(9f / 12f);
        this.GetComponents<AudioSource>()[5].Play();
        fireRocks();
        yield return new WaitForSeconds(3f / 12f);
        scatterRockAnimation = false;
        pickIdle();
        prevView = whatView;
        //fire off rocks
    }

    void fireRocks()
    {
        int whatFire = Random.Range(0, 4);
        if(whatFire == 1)
        {
            for(int i = 0; i < 4; i++)
            {
                for(int k = 0; k < 5; k++)
                {
                    GameObject projectile = Instantiate(scatterRock, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
                    projectile.GetComponent<SentinelScatterRockProjectile>().angleToTravel = (i * 90) - 20 + (10 * k);
                    projectile.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                }
            }
        }
        else if(whatFire == 2)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 5; k++)
                {
                    GameObject projectile = Instantiate(scatterRock, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
                    projectile.GetComponent<SentinelScatterRockProjectile>().angleToTravel = (i * 90 + 45) - 20 + (10 * k);
                    projectile.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                }
            }
        }
        else
        {
            for(int i = 0; i < 8; i++)
            {
                for(int k = 0; k < 3; k++)
                {
                    GameObject projectile = Instantiate(scatterRock, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
                    projectile.GetComponent<SentinelScatterRockProjectile>().angleToTravel = (i * 45) - 5 + (5 * k); 
                }
            }
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() == null)
        {
            return;
        }

        dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);

    }

    public override void deathProcedure()
    {
        GameObject deadBossInstant = Instantiate(deadBoss, transform.position, Quaternion.identity);
        deadBossInstant.GetComponent<DeadSentinel>().bossManager = this.bossManager;

        if (MiscData.dungeonLevelUnlocked == 1)
        {
            MiscData.dungeonLevelUnlocked = 2;
        }

        FindObjectOfType<BossHealthBar>().bossEnd();
        SaveSystem.SaveGame();
        FindObjectOfType<AudioManager>().FadeOut("First Boss Background Music", 0.2f);
        FindObjectOfType<AudioManager>().PlaySound("First Boss Defeated Music");
        FindObjectOfType<AudioManager>().FadeIn("First Boss Defeated Music", 0.2f, 1f);

        foreach (SentinelRotateRock rock in FindObjectsOfType<SentinelRotateRock>())
        {
            rock.GetComponent<Animator>().SetTrigger("Fall");
            rock.GetComponent<Collider2D>().enabled = false;
        }

        foreach (SentinelScatterRockProjectile projectile in FindObjectsOfType<SentinelScatterRockProjectile>())
        {
            projectile.breakRock();
        }
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        this.GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }
}
