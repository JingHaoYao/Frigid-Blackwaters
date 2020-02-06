using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManEnemy : Enemy {
    Animator animator;
    Rigidbody2D rigidBody2D;
    SpriteRenderer rend;
    public Sprite facingUp, facingDown, facingLeft, facingRight;
    public Sprite facingUpUnarmed, facingDownUnarmed, facingLeftUnarmed, facingRightUnarmed;
    private float periodBetweenMoves = 0, moveTimer = 0;
    private float travelAngle = 0;
    private bool pickedAngle = false;
    private bool throwSpear = false;
    public bool spearEquipped = true;
    public GameObject fishManSpear, bloodSplatter;
    private float throwAngle = 0;
    GameObject playerShip;
    BoxCollider2D boxCol;
    public GameObject deadFishman;
    public GameObject fishManIsland;
    private float stopFactor = 1;
    private bool strideEnded = false;
    private float foamTimer = 0;
    public GameObject waterFoam;
    bool isAttacking = false;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * speed / 3f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    void pickRendererLayer()
    {
        rend.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    IEnumerator animateThrow()
    {
        isAttacking = true;
        animator.enabled = true;
        this.GetComponents<AudioSource>()[1].Play();
        if (rend.sprite == facingUp)
        {
            animator.SetTrigger("3Throw");
        }
        else if (rend.sprite == facingDown)
        {
            animator.SetTrigger("2Throw");
        }
        else if (rend.sprite == facingLeft)
        {
            animator.SetTrigger("1Throw");
        }
        else
        {
            animator.SetTrigger("4Throw");
        }
            yield return new WaitForSeconds(8f / 12f);
        if (health > 0)
        {
            GameObject thrownSpear = Instantiate(fishManSpear, transform.position, Quaternion.identity);
            thrownSpear.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            thrownSpear.GetComponent<FishManSpear>().travelAngle = throwAngle;
            yield return new WaitForSeconds(0.750f);
            animator.enabled = false;
            yield return new WaitForSeconds(0.1f);
            throwSpear = false;
            moveTimer = 0;
        }
        isAttacking = false;
    }

    void movementSprite(float direction)
    {
        if (spearEquipped == true)
        {
            if (direction > 75 && direction < 105)
            {
                rend.sprite = facingUp;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
            else if (direction < 285 && direction > 265)
            {
                rend.sprite = facingDown;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
            else if (direction >= 285 && direction <= 360)
            {
                rend.sprite = facingLeft;
                transform.localScale = new Vector3(-0.2f, 0.2f, 0);
            }
            else if (direction >= 180 && direction <= 265)
            {
                rend.sprite = facingLeft;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
            else if (direction < 180 && direction >= 105)
            {
                rend.sprite = facingRight;
                transform.localScale = new Vector3(-0.2f, 0.2f, 0);
            }
            else
            {
                rend.sprite = facingRight;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
        }
        else
        {
            if (direction > 75 && direction < 105)
            {
                rend.sprite = facingUpUnarmed;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
            else if (direction < 285 && direction > 265)
            {
                rend.sprite = facingDownUnarmed;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
            else if (direction >= 285 && direction <= 360)
            {
                rend.sprite = facingLeftUnarmed;
                transform.localScale = new Vector3(-0.2f, 0.2f, 0);
            }
            else if (direction >= 180 && direction <= 265)
            {
                rend.sprite = facingLeftUnarmed;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
            else if (direction < 180 && direction >= 105)
            {
                rend.sprite = facingRightUnarmed;
                transform.localScale = new Vector3(-0.2f, 0.2f, 0);
            }
            else
            {
                rend.sprite = facingRightUnarmed;
                transform.localScale = new Vector3(0.2f, 0.2f, 0);
            }
        }
    }

    void moveDirection(float direction, float moveSpeed)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction*Mathf.Deg2Rad), Mathf.Sin(direction*Mathf.Deg2Rad), 0) * moveSpeed * stopFactor;
    }

    float cardinalizeDirections(float angle)
    {
        if (angle > 22.5f && angle <= 67.5f)
        {
            return 45;
        }
        else if (angle > 67.5f && angle <= 112.5f)
        {
            return 90;
        }
        else if (angle > 112.5f && angle <= 157.5f)
        {
            return 135;
        }
        else if (angle > 157.5f && angle <= 202.5f)
        {
            return 180;
        }
        else if (angle > 202.5f && angle <= 247.5f)
        {
            return 225;
        }
        else if (angle > 247.5 && angle < 292.5)
        {
            return 270;
        }
        else if (angle > 292.5 && angle < 337.5)
        {
            return 315;
        }
        else
        {
            return 0;
        }
    }

    void Start()
    {
        periodBetweenMoves = Random.Range(-1.5f, 1.5f);
        boxCol = GetComponent<BoxCollider2D>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
        playerShip = GameObject.Find("PlayerShip");
    }

	void Update () {
        if (health > 0)
        {
            periodBetweenMoves += Time.deltaTime;
            if (periodBetweenMoves >= .5f && throwSpear == false)
            {
                if(stopFactor == 0 && strideEnded == true)
                {
                    stopFactor = 1;
                }
                strideEnded = false;

                if (pickedAngle == false)
                {
                    pickedAngle = true;
                    if (spearEquipped == true)
                    {
                        travelAngle = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
                    }
                    else
                    {
                        travelAngle = (360 + Mathf.Atan2(fishManIsland.transform.position.y - transform.position.y, fishManIsland.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
                    }
                    movementSprite(travelAngle);
                }

                moveTimer += Time.deltaTime;
                if (moveTimer < 0.3f)
                {
                    updateSpeed(4);
                }
                else if (moveTimer <= 0.8f && moveTimer >= 0.3f)
                {
                    updateSpeed(4f - 2 * (3 * (moveTimer - 0.3f)));
                }
                else
                {
                    updateSpeed(0);
                    periodBetweenMoves = 0;
                    pickedAngle = false;
                    strideEnded = true;
                    moveTimer = 0;
                    if (Mathf.Sqrt(Mathf.Pow(transform.position.x - playerShip.transform.position.x, 2) + Mathf.Pow(transform.position.y - playerShip.transform.position.y, 2)) <= 5.5f)
                    {
                        if (spearEquipped == true)
                        {
                            throwSpear = true;
                        }
                    }
                }
            }

            if (throwSpear == true && spearEquipped == true && stopAttacking == false && isAttacking == false)
            {
                throwAngle = (360 + (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg)) % 360;
                movementSprite(throwAngle);
                StartCoroutine(animateThrow());
                spearEquipped = false;
                periodBetweenMoves = 0;
            }
        }

        if(spearEquipped == false)
        {
            this.gameObject.GetComponent<AStarPathfinding>().target = fishManIsland.transform.position;
            this.gameObject.layer = 14;

            if(Vector2.Distance(transform.position, fishManIsland.transform.position) < 0.5f)
            {
                spearEquipped = true;
                movementSprite(travelAngle);
                stopFactor = 0;
            }
        }
        else
        {
            this.gameObject.GetComponent<AStarPathfinding>().target = playerShip.transform.position;
            this.gameObject.layer = 10;
        }

        moveDirection(travelAngle, speed);
        spawnFoam();
        pickRendererLayer();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject == fishManIsland && spearEquipped == false)
        {
            spearEquipped = true;
            movementSprite(travelAngle);
            stopFactor = 0;
        }

        if (collision.gameObject.tag == "RoomHitbox" && (Mathf.Abs(transform.position.y) >= 8.5f || Mathf.Abs(transform.position.x) >= 8.5f))
        {
            stopFactor = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<DamageAmount>())
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            Instantiate(bloodSplatter, collision.gameObject.transform.position, Quaternion.identity);
        }
    }

    public override void deathProcedure()
    {
        rigidBody2D.velocity = Vector3.zero;
        GameObject deadFishMan = Instantiate(deadFishman, transform.position, Quaternion.identity);
        deadFishMan.GetComponent<DeadFishMan>().spriteRenderer.sortingOrder = rend.sortingOrder;
        deadFishMan.GetComponent<DeadFishMan>().whatView = whatView();
        deadFishMan.transform.localScale = transform.localScale;
        boxCol.enabled = false;
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        this.GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }

    int whatView()
    {
        if (rend.sprite == facingUp)
        {
            return 3;
        }
        else if (rend.sprite == facingDown)
        {
            return 2;
        }
        else if (rend.sprite == facingLeft)
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
        rend.color = Color.red;
        yield return new WaitForSeconds(.1f);
        rend.color = Color.white;
    }
}
