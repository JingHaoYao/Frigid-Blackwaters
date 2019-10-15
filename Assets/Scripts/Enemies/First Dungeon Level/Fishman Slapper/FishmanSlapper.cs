using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishmanSlapper : Enemy
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    Animator animator;
    public Sprite facingLeft, facingDown, facingRight, facingUp;
    public GameObject leftFacingHitbox, downFacingHitbox, upFacingHitbox, rightFacingHitbox;
    private float moveTimer = 0, periodBetweenMoves = 0, travelAngle = 0, travelSpeed = 0;
    private float attackTimer = 0;
    private bool strideEnded = false, pickedAngle = false, crossedLocation = false, firingAnimation = false;
    GameObject playerShip;
    private float foamTimer = 0;
    public GameObject waterFoam, deadShaman, bloodSplatter;
    List<AStarNode> path;
    float angleToShip = 0;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * travelSpeed / 3f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    IEnumerator attackAnim()
    {
        pickSprite(angleToShip);
        animator.enabled = true;
        int view = 0;
        if (spriteRenderer.sprite == facingLeft)
        {
            view = 1;
            animator.SetTrigger("Attack1");
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            view = 2;
            animator.SetTrigger("Attack2");
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            view = 4;
            animator.SetTrigger("Attack4");
        }
        else if (spriteRenderer.sprite == facingRight)
        {
            view = 3;
            animator.SetTrigger("Attack3");
        }
        yield return new WaitForSeconds(7f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        if (view == 1)
        {
            leftFacingHitbox.SetActive(true);
        }
        else if (view == 2)
        {
            downFacingHitbox.SetActive(true);
        }
        else if(view == 4)
        {
            upFacingHitbox.SetActive(true);
        }
        else if (view == 3)
        {
            rightFacingHitbox.SetActive(true);
        }
        yield return new WaitForSeconds(3f / 12f);
        leftFacingHitbox.SetActive(false);
        downFacingHitbox.SetActive(false);
        upFacingHitbox.SetActive(false);
        rightFacingHitbox.SetActive(false);
        firingAnimation = false;
        animator.enabled = false;
    }

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void moveDirection(float direction, float moveSpeed)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * moveSpeed;
    }

    void pickSprite(float direction)
    {
        if (animator.enabled == false)
        {
            //picks sprite depending on what direction the enemy is facing
            //sets scale to mirror the sprite if necessary
            if (direction > 75 && direction < 105)
            {
                spriteRenderer.sprite = facingUp;
                transform.localScale = new Vector3(3.5f, 3.5f, 0);
            }
            else if (direction < 285 && direction > 265)
            {
                spriteRenderer.sprite = facingDown;
                transform.localScale = new Vector3(3.5f, 3.5f, 0);
            }
            else if (direction >= 285 && direction <= 360)
            {
                spriteRenderer.sprite = facingLeft;
                transform.localScale = new Vector3(-3.5f, 3.5f, 0);
            }
            else if (direction >= 180 && direction <= 265)
            {
                spriteRenderer.sprite = facingLeft;
                transform.localScale = new Vector3(3.5f, 3.5f, 0);
            }
            else if (direction < 180 && direction >= 105)
            {
                spriteRenderer.sprite = facingRight;
                transform.localScale = new Vector3(-3.5f, 3.5f, 0);
            }
            else
            {
                spriteRenderer.sprite = facingRight;
                transform.localScale = new Vector3(3.5f, 3.5f, 0);
            }
        }
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

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
        moveTimer = Random.Range(-1f, 0f);
    }

    private void Update()
    {
        this.GetComponent<AStarPathfinding>().target = playerShip.transform.position;
        path = GetComponent<AStarPathfinding>().seekPath;
        AStarNode pathNode = path[0];
        angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        if ((crossedLocation == false || strideEnded == false) && firingAnimation == false)
        {
            periodBetweenMoves += Time.deltaTime;
            if (periodBetweenMoves >= .6f)
            {
                strideEnded = false;

                //setting travel direction based on whether the ship is close to the enemy
                if (pickedAngle == false)
                {
                    pickedAngle = true;
                    Vector3 targetPos = pathNode.nodePosition;
                    travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);
                    pickSprite(travelAngle);
                }

                //acceleration and deacceleration
                moveTimer += Time.deltaTime;
                if (moveTimer < 0.2f)
                {
                    travelSpeed = 4;
                }
                else if (moveTimer <= 0.5f && moveTimer >= 0.2f)
                {
                    travelSpeed = 4 - 4 * (3 * (moveTimer - 0.2f));
                }
                else
                {
                    pickedAngle = false;
                    strideEnded = true;
                    travelSpeed = 0;
                    periodBetweenMoves = 0;
                    moveTimer = 0;
                }
            }
            moveDirection(travelAngle, travelSpeed);

            if (Vector2.Distance(transform.position, playerShip.transform.position) <= 2f)
            {
                //checks if the guy has swam over the targeted position
                crossedLocation = true;
            }

            attackTimer = 3.5f;
        }
        else
        {

            if (Vector2.Distance(transform.position, playerShip.transform.position) > 2)
            {
                strideEnded = false;
                crossedLocation = false;
                periodBetweenMoves = 0.6f;
            }

            attackTimer += Time.deltaTime;
            if (attackTimer > 1.5f && stopAttacking == false && firingAnimation == false)
            {
                //cooldown for attacking and starting to spawn attacks
                firingAnimation = true; //<-- so the enemy doesn't start moving while attacking
                attackTimer = 0;
                StartCoroutine(attackAnim());
            }

            pickSprite(angleToShip); //makes it so the enemy faces the ship
        }
        spawnFoam();
        pickRendererLayer();
    }

    int whatView()
    {
        if (spriteRenderer.sprite == facingUp)
        {
            return 4;
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
            return 3;
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
        this.GetComponents<AudioSource>()[0].Play();
        if (health <= 0)
        {
            Instantiate(bloodSplatter, collision.gameObject.transform.position, Quaternion.identity);
            GameObject deadPirate = Instantiate(deadShaman, transform.position, Quaternion.identity);
            deadPirate.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
            deadPirate.GetComponent<DeadEnemyScript>().whatView = whatView();
            deadPirate.transform.localScale = transform.localScale;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            addKills();
            Destroy(this.gameObject);
        }
        else
        {
            Instantiate(bloodSplatter, collision.gameObject.transform.position, Quaternion.identity);
            StartCoroutine(hitFrame());
        }
    }
}
