using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sharkman : Enemy {
    public Sprite facingLeft, facingRight, facingDown, facingUp;
    public Sprite finLeft, finRight, finDown, finUp;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    Animator animator;
    BoxCollider2D boxCol;
    float travelSpeed = 4;
    GameObject playerShip;
    private float attackPeriod = 0;
    private float idlePeriod = 0;
    Vector3 tempTravelVector;
    private bool isAttacking = false;
    private bool collided = false;
    private bool hasAnimated = false;
    public GameObject waterSplash;
    float angleToShip = 0;
    public GameObject damageHitBox;
    public GameObject deadSharkMan, bloodSplatter, waterFoam;
    private float foamTimer = 0;
    bool clockWise = true;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * rigidBody2D.velocity.magnitude / 3f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    IEnumerator diveIn()
    {
        animator.enabled = true;
        if (spriteRenderer.sprite == facingLeft)
        {
            animator.SetTrigger("Dive1");
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            animator.SetTrigger("Dive2");
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            animator.SetTrigger("Dive4");
        }
        else if (spriteRenderer.sprite == facingRight)
        {
            animator.SetTrigger("Dive3");
        }
        yield return new WaitForSeconds(3f / 12f);
        GameObject splash = Instantiate(waterSplash, transform.position, Quaternion.identity);
        splash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder - 1;
        yield return new WaitForSeconds(2f / 12f);
        animator.enabled = false;
        if (spriteRenderer.sprite == facingLeft)
        {
            spriteRenderer.sprite = finLeft;
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            spriteRenderer.sprite = finDown;
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            spriteRenderer.sprite = finUp;
        }
        else if (spriteRenderer.sprite == facingRight)
        {
            spriteRenderer.sprite = finRight;
        }
        isAttacking = true;
        pickSprite(angleToShip);
        hasAnimated = false;
        attackPeriod = 0;
        idlePeriod = 0;
    }

    IEnumerator diveOut()
    {
        animator.enabled = true;
        if (spriteRenderer.sprite == finLeft)
        {
            animator.SetTrigger("DiveUp1");
        }
        else if (spriteRenderer.sprite == finDown)
        {
            animator.SetTrigger("DiveUp2");
        }
        else if (spriteRenderer.sprite == finUp)
        {
            animator.SetTrigger("DiveUp4");
        }
        else if (spriteRenderer.sprite == finRight)
        {
            animator.SetTrigger("DiveUp3");
        }
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(0.5f);
        animator.enabled = false;
        if (spriteRenderer.sprite == finLeft)
        {
            spriteRenderer.sprite = facingLeft;
        }
        else if (spriteRenderer.sprite == finDown)
        {
            spriteRenderer.sprite = facingDown;
        }
        else if (spriteRenderer.sprite == finUp)
        {
            spriteRenderer.sprite = facingUp;
        }
        else if (spriteRenderer.sprite == finRight)
        {
            spriteRenderer.sprite = facingRight;
        }
        hasAnimated = false;
        collided = false;
        idlePeriod = 0;
        attackPeriod = 0;
        isAttacking = false;
    }

    void pickSprite(float direction)
    {
        if (isAttacking == false)
        {
            if (direction > 75 && direction < 105)
            {
                spriteRenderer.sprite = facingUp;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
            else if (direction < 285 && direction > 265)
            {
                spriteRenderer.sprite = facingDown;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
            else if (direction >= 285 && direction <= 360)
            {
                spriteRenderer.sprite = facingLeft;
                transform.localScale = new Vector3(-0.2f, 0.2f, 0);
            }
            else if (direction >= 180 && direction <= 265)
            {
                spriteRenderer.sprite = facingLeft;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
            else if (direction < 180 && direction >= 105)
            {
                spriteRenderer.sprite = facingRight;
                transform.localScale = new Vector3(-0.2f, 0.2f, 0);
            }
            else
            {
                spriteRenderer.sprite = facingRight;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
        }
        else
        {
            if (direction > 75 && direction < 105)
            {
                spriteRenderer.sprite = finUp;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
            else if (direction < 285 && direction > 265)
            {
                spriteRenderer.sprite = finDown;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
            else if (direction >= 285 && direction <= 360)
            {
                spriteRenderer.sprite = finLeft;
                transform.localScale = new Vector3(-0.2f, 0.2f, 0);
            }
            else if (direction >= 180 && direction <= 265)
            {
                spriteRenderer.sprite = finLeft;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
            else if (direction < 180 && direction >= 105)
            {
                spriteRenderer.sprite = finRight;
                transform.localScale = new Vector3(-0.2f, 0.2f, 0);
            }
            else
            {
                spriteRenderer.sprite = finRight;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
        }
    }

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * travelSpeed;
    }

    void circleShip(bool clockWise)
    {
        rigidBody2D.velocity = Vector3.zero;
        float circleAngle;
        if (clockWise == true)
        {
            circleAngle = (270 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
        else
        {
            circleAngle = (450 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
        angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        rigidBody2D.velocity = new Vector3(Mathf.Cos(circleAngle * Mathf.Deg2Rad) + Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(circleAngle * Mathf.Deg2Rad) + Mathf.Sin(angleToShip * Mathf.Deg2Rad), 0) * travelSpeed;
        tempTravelVector = rigidBody2D.velocity;
    }

    void Start () {
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShip = GameObject.Find("PlayerShip");
        animator = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        animator.enabled = false;
        if(Random.Range(0,2) == 1)
        {
            clockWise = true;
        }
        else
        {
            clockWise = false;
        }
	}

	void Update () {
        pickRendererLayer();
        float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        spawnFoam();
        if (isAttacking == true)
        {
            boxCol.enabled = false;
            damageHitBox.SetActive(true);
            attackPeriod += Time.deltaTime;
            if (attackPeriod < 3)
            {
                circleShip(clockWise);
            }
            else
            {
                rigidBody2D.velocity = tempTravelVector;
            }
            float velocityAngle = (360 + Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg) % 360;
            pickSprite(velocityAngle);

            if(collided == true)
            {
                rigidBody2D.velocity = Vector3.zero;
                if(hasAnimated == false)
                {
                    StartCoroutine(diveOut());
                    hasAnimated = true;
                }
            }

            if(attackPeriod >= 12)
            {
                collided = true;
            }
        }
        else
        {
            boxCol.enabled = true;
            damageHitBox.SetActive(false);
            rigidBody2D.velocity = Vector3.zero;
            if(idlePeriod < 2)
            {
                idlePeriod += Time.deltaTime;
                pickSprite(angleToShip);
            }
            else
            {
                if (hasAnimated == false && stopAttacking == false)
                {
                    StartCoroutine(diveIn());
                    hasAnimated = true;
                }
            }
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != damageHitBox && attackPeriod >= 0.5f && isAttacking == true)
        {
            collided = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isAttacking == false)
        {
            if (collision.gameObject.GetComponent<DamageAmount>())
            {
                this.GetComponents<AudioSource>()[0].Play();
                int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
                health -= damageDealt;
                if (health <= 0)
                {
                    Instantiate(bloodSplatter, collision.gameObject.transform.position, Quaternion.identity);
                    GameObject deadPirate = Instantiate(deadSharkMan, transform.position, Quaternion.identity);
                    deadPirate.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
                    deadPirate.GetComponent<DeadEnemyScript>().whatView = whatView();
                    deadPirate.transform.localScale = transform.localScale;
                    this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    addKills();
                    Destroy(this.gameObject);
                }
                else
                {
                    StartCoroutine(hitFrame());
                }
            }
        }
    }

    int whatView()
    {
        if (spriteRenderer.sprite == facingUp)
        {
            return 3;
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            return 2;
        }
        else if (spriteRenderer.sprite == facingLeft)
        {
            return 1;
        }
        else
        {
            return 4;
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
