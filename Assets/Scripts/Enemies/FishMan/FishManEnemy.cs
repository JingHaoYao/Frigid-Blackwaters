using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManEnemy : MonoBehaviour {
    Animator animator;
    Rigidbody2D rigidBody2D;
    SpriteRenderer rend;
    public Sprite facingUp, facingDown, facingLeft, facingRight;
    public Sprite facingUpUnarmed, facingDownUnarmed, facingLeftUnarmed, facingRightUnarmed;
    private float periodBetweenMoves = 0, moveTimer = 0;
    private float travelAngle = 0;
    public float travelSpeed = 4;
    private bool pickedAngle = false;
    private bool throwSpear = false;
    public bool spearEquipped = true;
    GameObject playerShip;

    IEnumerator animateThrow()
    {
        animator.enabled = true;
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
        yield return new WaitForSeconds(0.750f);
        animator.enabled = false;
        movementSprite(travelAngle);
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
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction*Mathf.Deg2Rad), Mathf.Sin(direction*Mathf.Deg2Rad), 0) * moveSpeed;
    }

    void Start () {
        rigidBody2D = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
        playerShip = GameObject.Find("PlayerShip");
	}

	void Update () {
        periodBetweenMoves += Time.deltaTime;
        if(periodBetweenMoves >= 2.5 && throwSpear == false)
        {
            if(pickedAngle == false)
            {
                pickedAngle = true;
                if (spearEquipped == true)
                {
                    travelAngle = (360 + (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg)) % 360;
                }
                else
                {
                    travelAngle = (360 + (Mathf.Atan2(GameObject.Find("FishManIsland").transform.position.y - transform.position.y, GameObject.Find("FishManIsland").transform.position.x - transform.position.x) * Mathf.Rad2Deg)) % 360;
                }
                movementSprite(travelAngle);
            }

            moveTimer += Time.deltaTime;
            if(moveTimer < 0.6f)
            {
                travelSpeed = 4;
            }
            else if(moveTimer <= 1.6f && moveTimer >= 0.6f)
            {
                travelSpeed = 4 - (4 * (moveTimer - 0.6f));
            }
            else
            {
                travelSpeed = 0;
                periodBetweenMoves = 0;
                moveTimer = 0;
                pickedAngle = false;
                if(Mathf.Sqrt(Mathf.Pow(transform.position.x - playerShip.transform.position.x, 2) + Mathf.Pow(transform.position.y - playerShip.transform.position.y, 2)) <= 5.5f)
                {
                    if (spearEquipped == true)
                    {
                        throwSpear = true;
                    }
                }
            }
            moveDirection(travelAngle, travelSpeed);
        }

        if(throwSpear == true && spearEquipped == true)
        {
            StartCoroutine(animateThrow());
            throwSpear = false;
            spearEquipped = false;
            periodBetweenMoves = 0;
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.name == "FishManIsland" && spearEquipped == false)
        {
            spearEquipped = true;
            movementSprite(travelAngle);
        } 
    }
}
