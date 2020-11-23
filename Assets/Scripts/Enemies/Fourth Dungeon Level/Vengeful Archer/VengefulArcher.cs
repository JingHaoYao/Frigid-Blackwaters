using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VengefulArcher : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D takeDamageHitBox;
    public BossManager bossManager;
    private BossHealthBar healthBar;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] AudioSource takeDamageAudio, bowPullAudio, bowReleaseAudio, wingFlapAudio;
    [SerializeField] GameObject spectralArrow, bounceArrow;
    Vector3 centerPosition;
    List<Vector3> positionsList = new List<Vector3>() { new Vector3(-3, 0), new Vector3(3, 0), new Vector3(0, -3), new Vector3(0, 3) };

    int whatView = 1;
    int prevView = -1;
    int mirror = 1;
    bool isAttacking = false;
    private float attackPeriod = 1;
    private int numberSpreadShotsFired = 2;

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
        animator.SetInteger("WhatView", whatView);
    }

    void pickIdleAnimation()
    {
        pickView(angleToShip);
        if(whatView != prevView)
        {
            prevView = whatView;
            animator.SetTrigger("Idle");
        }
        transform.localScale = new Vector3(4 * mirror, 4);
    }

    private float angleToShip
    {
        get
        {
            return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
    }

    float moveToNewLocation()
    {
        Vector3 newPosition = centerPosition + positionsList[Random.Range(0, positionsList.Count)];
        while(Vector2.Distance(newPosition, transform.position) < 0.5f)
        {
            newPosition = centerPosition + positionsList[Random.Range(0, positionsList.Count)];
        }
        float time = Vector2.Distance(transform.position, newPosition) / speed;
        LeanTween.move(this.gameObject, newPosition, Vector2.Distance(transform.position, newPosition) / speed).setEaseOutCirc();
        return time;
    }

    IEnumerator launchSpreadShot()
    {
        isAttacking = true;
        pickView(angleToShip);
        float firingAngle = angleToShip;
        animator.SetTrigger("SpreadshotAttack");
        yield return new WaitForSeconds(5 / 12f);
        bowPullAudio.Play();
        yield return new WaitForSeconds(8 / 12f);
        bowReleaseAudio.Play();
        for(int i = 0; i < 5; i++)
        {
            float angleTravel = firingAngle - 20 + i * 10;
            GameObject arrowInstant = Instantiate(spectralArrow, transform.position + new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad) + 4) * 0.5f, Quaternion.identity);
            arrowInstant.GetComponent<BasicProjectile>().angleTravel = angleTravel;
            arrowInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        yield return new WaitForSeconds(9 / 12f);
        isAttacking = false;
        attackPeriod = moveToNewLocation();
        prevView = -1;
    }

    IEnumerator launchBounceShot()
    {
        isAttacking = true;
        pickView(angleToShip);
        float firingAngle = angleToShip;
        animator.SetTrigger("BounceAttack");
        yield return new WaitForSeconds(5 / 12f);
        bowPullAudio.Play();
        yield return new WaitForSeconds(8 / 12f);
        bowReleaseAudio.Play();
        GameObject arrowInstant = Instantiate(bounceArrow, transform.position + new Vector3(Mathf.Cos(firingAngle * Mathf.Deg2Rad), Mathf.Sin(firingAngle * Mathf.Deg2Rad) + 4) * 0.5f, Quaternion.identity);
        arrowInstant.GetComponent<VengefulArcherBounceArrow>().Initialize(firingAngle * Mathf.Deg2Rad, this.gameObject);
        yield return new WaitForSeconds(9 / 12f);
        isAttacking = false;
        attackPeriod = moveToNewLocation();
        prevView = -1;
    }

    IEnumerator attackProcedure()
    {
        yield return new WaitForSeconds(0.75f);
        while (true)
        {
            if (isAttacking == false)
            {
                pickIdleAnimation();
                if (attackPeriod > 0)
                {
                    attackPeriod -= Time.deltaTime;
                }
                else
                {
                    if (numberSpreadShotsFired < 2)
                    {
                        numberSpreadShotsFired++;
                        StartCoroutine(launchSpreadShot());
                    }
                    else
                    {
                        StartCoroutine(launchBounceShot());
                        numberSpreadShotsFired = 0;
                    }
                }
            }

            yield return null;
        }
    }

    private void Start()
    {
        StartCoroutine(attackProcedure());
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.bossStartUp("Ethereal Archer");
        wingFlapAudio.Play();
        healthBar.targetEnemy = this;
        EnemyPool.addEnemy(this);
        centerPosition = Camera.main.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
      
        if (health > 0)
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
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
        animator.SetTrigger("Death");
        SaveSystem.SaveGame();
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        takeDamageAudio.Play();
        SpawnArtifactKillsAndGoOnCooldown(2);
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
