using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeGolem : Enemy {
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    public float movementSpeed = 3.5f;
    public GameObject hitBox1, hitBox2, hitBox3, hitBox4, hitBox5;
    GameObject[] hitBoxList;
    public float angleToShip;
    GameObject playerShip;
    int whichMoveAnim = 0;
    private bool isMeleeAttacking;
    private float meleeChaseTimer = 0;
    int numSingleBlasts = 0;
    bool isFiring = false;
    public GameObject laserCircle, laserBlast, energyball;
    public GameObject deadGolem;
    public AntiSpawnSpaceDetailer anti;

    IEnumerator summonEnergyBall()
    {
        isFiring = true;
        animator.SetTrigger("EnergyBlast");
        this.GetComponent<AudioSource>().Play();
        float angleAdd = Random.Range(0, 9) * 10;
        for(int i = 0; i < 8; i++)
        {
            float blastAngle = 45 * i + angleAdd;
            GameObject instant = Instantiate(energyball, transform.position, Quaternion.Euler(0, 0, blastAngle));
            instant.GetComponent<EnergyBall>().angleTravel = blastAngle;
            instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        yield return new WaitForSeconds(0.75f);
        isFiring = false;
        pickMoveAnimation2(angleToShip);
    }
    
    IEnumerator fireSingleBlast(float angleFire)
    {
        isFiring = true;
        float angleFromCircle = (360 + Mathf.Atan2(playerShip.transform.position.y - (transform.position.y + 4), playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        GameObject spawnedLaserCircle = Instantiate(laserCircle, transform.position + new Vector3(0, 4f, 0), Quaternion.Euler(0, 0, angleFromCircle));
        yield return new WaitForSeconds(12f / 36f);
        GameObject spawnedLaserBlast = Instantiate(laserBlast, spawnedLaserCircle.transform.position + new Vector3(Mathf.Cos(angleFromCircle * Mathf.Deg2Rad), Mathf.Sin(angleFromCircle * Mathf.Deg2Rad) * 1.5f, 0), Quaternion.Euler(0, 0, angleFire));
        spawnedLaserBlast.GetComponent<ChallengeGolemLaserBlast>().angleTravel = (180 + Mathf.Atan2(spawnedLaserCircle.transform.position.y - playerShip.transform.position.y, spawnedLaserCircle.transform.position.x - playerShip.transform.position.x) * Mathf.Rad2Deg) % 360;
        spawnedLaserBlast.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        yield return new WaitForSeconds(14f / 36f);
        isFiring = false;
        numSingleBlasts++;
    }

    IEnumerator meleeAttack(float angleOrientation)
    {
        isMeleeAttacking = true;
        int whatHitBox = 0;
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            animator.SetTrigger("Attack4");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whatHitBox = 3;
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            animator.SetTrigger("Attack5");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whatHitBox = 4;
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            animator.SetTrigger("Attack4");
            transform.localScale = new Vector3(-0.6f, 0.6f, 0);
            whatHitBox = 3;
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            animator.SetTrigger("Attack3");
            transform.localScale = new Vector3(-0.6f, 0.6f, 0);
            whatHitBox = 2;
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            animator.SetTrigger("Attack2");
            transform.localScale = new Vector3(-0.6f, 0.6f, 0);
            whatHitBox = 1;
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {
            animator.SetTrigger("Attack1");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whatHitBox = 0;
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            animator.SetTrigger("Attack2");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whatHitBox = 1;
        }
        else
        {
            animator.SetTrigger("Attack3");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whatHitBox = 2;
        }
        yield return new WaitForSeconds(0.583f);
        hitBoxList[whatHitBox].SetActive(true);
        yield return new WaitForSeconds(2f / 12f);
        hitBoxList[whatHitBox].SetActive(false);
        yield return new WaitForSeconds(1f / 12f);
        isMeleeAttacking = false;
        pickMoveAnimation2(angleToShip);
    }

    void pickMoveAnimation(float angleOrientation)
    {
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            if (whichMoveAnim != 4)
                animator.SetTrigger("Move4");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whichMoveAnim = 4;
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            if (whichMoveAnim != 5)
                animator.SetTrigger("Move5");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whichMoveAnim = 5;
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            if (whichMoveAnim != 4)
                animator.SetTrigger("Move4");
            transform.localScale = new Vector3(-0.6f, 0.6f, 0);
            whichMoveAnim = 4;
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            if (whichMoveAnim != 3)
                animator.SetTrigger("Move3");
            transform.localScale = new Vector3(-0.6f, 0.6f, 0);
            whichMoveAnim = 3;
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            if (whichMoveAnim != 2)
                animator.SetTrigger("Move2");
            transform.localScale = new Vector3(-0.6f, 0.6f, 0);
            whichMoveAnim = 2;
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {
            if (whichMoveAnim != 1)
                animator.SetTrigger("Move1");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whichMoveAnim = 1;
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            if (whichMoveAnim != 2)
                animator.SetTrigger("Move2");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whichMoveAnim = 2;
        }
        else
        {
            if (whichMoveAnim != 3)
                animator.SetTrigger("Move3");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whichMoveAnim = 3;
        }
    }

    void pickMoveAnimation2(float angleOrientation)
    {
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            animator.SetTrigger("Move4");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whichMoveAnim = 4;
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            animator.SetTrigger("Move5");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whichMoveAnim = 5;
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            animator.SetTrigger("Move4");
            transform.localScale = new Vector3(-0.6f, 0.6f, 0);
            whichMoveAnim = 4;
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            animator.SetTrigger("Move3");
            transform.localScale = new Vector3(-0.6f, 0.6f, 0);
            whichMoveAnim = 3;
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            animator.SetTrigger("Move2");
            transform.localScale = new Vector3(-0.6f, 0.6f, 0);
            whichMoveAnim = 2;
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {
            animator.SetTrigger("Move1");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whichMoveAnim = 1;
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            animator.SetTrigger("Move2");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whichMoveAnim = 2;
        }
        else
        {
            animator.SetTrigger("Move3");
            transform.localScale = new Vector3(0.6f, 0.6f, 0);
            whichMoveAnim = 3;
        }
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * movementSpeed;
    }

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void pickAttack()
    {
        if(Vector2.Distance(playerShip.transform.position, transform.position) < 4f && (meleeChaseTimer < 3f || isMeleeAttacking == true) && isFiring == false)
        {
            meleeChaseTimer += Time.deltaTime;
            if (Vector2.Distance(playerShip.transform.position, transform.position) < 2f)
            {
                if (isMeleeAttacking == false)
                {
                    StartCoroutine(meleeAttack(angleToShip));
                    rigidBody2D.velocity = Vector3.zero;
                }
            }
            else
            {
                if (isMeleeAttacking == false)
                {
                    moveTowards(angleToShip);
                    pickMoveAnimation(angleToShip);
                }
            }
        }
        else
        {
            if(Vector2.Distance(playerShip.transform.position, transform.position) >= 4f)
            {
                meleeChaseTimer = 0;
            }
            rigidBody2D.velocity = Vector3.zero;

            if (isFiring == false)
            {
                if (numSingleBlasts < 6)
                {
                    isFiring = true;
                    pickMoveAnimation(angleToShip);
                    StartCoroutine(fireSingleBlast(angleToShip));
                }
                else
                {
                    numSingleBlasts = 0;
                    isFiring = true;
                    StartCoroutine(summonEnergyBall());
                }
            }
        }
    } 

    void Start () {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitBoxList = new GameObject[5] { hitBox1, hitBox2, hitBox3, hitBox4, hitBox5 };
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerShip = GameObject.Find("PlayerShip");
	}

	void Update () {
        angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        pickRendererLayer();
        pickAttack();
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
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            if (health <= 0)
            {
                GameObject deadMusketeer = Instantiate(deadGolem, transform.position, Quaternion.identity);
                deadMusketeer.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder;
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                anti.trialDefeated = true;
                GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = true;
                Destroy(this.gameObject);
                addKills();
            }
            else
            {
                StartCoroutine(hitFrame());
            }
        }
    }
}
