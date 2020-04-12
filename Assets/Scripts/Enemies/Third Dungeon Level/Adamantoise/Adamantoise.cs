using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adamantoise : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D takeDamageHitBox;
    public BossManager bossManager;
    private BossHealthBar healthBar;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource takeDamageAudio;
    [SerializeField] AudioSource fireLightBallAudio;
    [SerializeField] AdamantoiseLeg leg;
    public GameObject lightBallProjectile;
    private float attackPeriod = 2;
    private int numberFireAttacks = 0;
    Camera mainCamera;
    bool isAttacking = false;

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(attackProcedure());
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.bossStartUp("Adamantoise");
        healthBar.targetEnemy = this;
    }


    IEnumerator attackProcedure()
    {
        while (true)
        {
            if(isAttacking == false)
            {
                pickIdleAnim();
            }

            if(attackPeriod > 0)
            {
                attackPeriod -= Time.deltaTime;
            }
            else
            {
                if(numberFireAttacks < 2)
                {
                    numberFireAttacks++;
                    StartCoroutine(fireLightBalls());
                }
                else
                {
                    attackPeriod = 5;
                    numberFireAttacks = 0;
                    leg.smash();
                }
            }
            yield return null;
        }
    }


    IEnumerator fireLightBalls()
    {
        isAttacking = true;
        attackPeriod = 3;
        animator.SetTrigger("Fire");
        yield return new WaitForSeconds(6 / 12f);
        fireLightBallAudio.Play();
        launchLightBalls();
        yield return new WaitForSeconds(13 / 12f);
        pickIdleAnim();
        isAttacking = false;
    }

    void launchLightBalls()
    {
        int whichAttack = Random.Range(0, 3);
        float baseAngle = Mathf.Atan2(mainCamera.transform.position.y - transform.position.y, mainCamera.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        if(whichAttack == 0)
        {
            for(int i = 0; i < 6; i++)
            {
                float angleTravel = baseAngle - 37.5f  + 15 * i;
                GameObject instant = Instantiate(lightBallProjectile, transform.position + new Vector3(Mathf.Cos((90 + transform.parent.rotation.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Sin((90 + transform.parent.rotation.eulerAngles.z) * Mathf.Deg2Rad)), Quaternion.identity);
                instant.GetComponent<AdamantoiseLightBall>().angleTravel = angleTravel;
                instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        else if(whichAttack == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                float angleTravel = baseAngle - 40 + 40 * i;
                GameObject instant = Instantiate(lightBallProjectile, transform.position + new Vector3(Mathf.Cos((90 + transform.parent.rotation.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Sin((90 + transform.parent.rotation.eulerAngles.z) * Mathf.Deg2Rad)), Quaternion.identity);
                instant.GetComponent<AdamantoiseLightBall>().angleTravel = angleTravel + 5;
                instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                instant = Instantiate(lightBallProjectile, transform.position + new Vector3(Mathf.Cos((90 + transform.parent.rotation.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Sin((90 + transform.parent.rotation.eulerAngles.z) * Mathf.Deg2Rad)), Quaternion.identity);
                instant.GetComponent<AdamantoiseLightBall>().angleTravel = angleTravel - 5;
                instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                float angleTravel = baseAngle - 30 + 60 * i;
                GameObject instant = Instantiate(lightBallProjectile, transform.position + new Vector3(Mathf.Cos((90 + transform.parent.rotation.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Sin((90 + transform.parent.rotation.eulerAngles.z) * Mathf.Deg2Rad)), Quaternion.identity);
                instant.GetComponent<AdamantoiseLightBall>().angleTravel = angleTravel + 5;
                instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                instant = Instantiate(lightBallProjectile, transform.position + new Vector3(Mathf.Cos((90 + transform.parent.rotation.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Sin((90 + transform.parent.rotation.eulerAngles.z) * Mathf.Deg2Rad)), Quaternion.identity);
                instant.GetComponent<AdamantoiseLightBall>().angleTravel = angleTravel - 5;
                instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                instant = Instantiate(lightBallProjectile, transform.position + new Vector3(Mathf.Cos((90 + transform.parent.rotation.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Sin((90 + transform.parent.rotation.eulerAngles.z) * Mathf.Deg2Rad)), Quaternion.identity);
                instant.GetComponent<AdamantoiseLightBall>().angleTravel = angleTravel;
                instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
    }

    void pickIdleAnim()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Adamantoise Head Idle 1"))
        {
            animator.SetTrigger("Idle1");
        }
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
        bossManager.bossBeaten(nameID, 1.083f);
        leg.stopAllCoroutines();
        PlayerProperties.playerScript.enemiesDefeated = true;
        healthBar.bossEnd();
        animator.SetTrigger("Death");
        SaveSystem.SaveGame();
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        takeDamageAudio.Play();
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
