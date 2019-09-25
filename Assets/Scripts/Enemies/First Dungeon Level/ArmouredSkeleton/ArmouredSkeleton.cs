using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmouredSkeleton : Enemy {
    public Sprite facingLeft, facingUp, facingDown, facingRight;
    public GameObject leftFacingHitbox, downFacingHitbox, upFacingHitbox, rightFacingHitbox;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    Animator animator;
    private bool withinRange = false, touchingShip = false;
    public float travelSpeed = 2;
    private float travelAngle;
    GameObject playerShip;
    private float pokePeriod = 1.5f;
    public GameObject unarmouredSpearman;
    private float foamTimer = 0;
    public GameObject waterFoam;
    private bool shattering = false;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    bool alreadySpawned = false;

    IEnumerator spawnUnArmoured()
    {
        this.GetComponent<AudioSource>().Play();
        animator.enabled = true;
        shattering = true;
        if (spriteRenderer.sprite == facingLeft)
        {
            animator.SetTrigger("Shatter1");
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            animator.SetTrigger("Shatter2");
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            animator.SetTrigger("Shatter4");
        }
        else if (spriteRenderer.sprite == facingRight)
        {
            animator.SetTrigger("Shatter3");
        }
        yield return new WaitForSeconds(6f / 12f);
        Destroy(this.gameObject);
        GameObject spawnedSpearMan = Instantiate(unarmouredSpearman, transform.position, Quaternion.identity);
        this.GetComponent<Collider2D>().enabled = false;
        spawnedSpearMan.GetComponent<SkeletalSpearman>().health = 1;
        spawnedSpearMan.GetComponent<SkeletalSpearman>().travelAngle = travelAngle;
        animator.enabled = false;
    }

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

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * travelSpeed;
    }

    IEnumerator poke()
    {
        animator.enabled = true;
        if (spriteRenderer.sprite == facingLeft)
        {
            animator.SetTrigger("Attack1");
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            animator.SetTrigger("Attack2");
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            animator.SetTrigger("Attack4");
        }
        else if (spriteRenderer.sprite == facingRight)
        {
            animator.SetTrigger("Attack3");
        }
        yield return new WaitForSeconds(4f / 12f);
        if (shattering == false)
        {
            if (withinRange == true || touchingShip == true)
            {
                if (spriteRenderer.sprite == facingLeft)
                {
                    leftFacingHitbox.SetActive(true);
                }
                else if (spriteRenderer.sprite == facingDown)
                {
                    downFacingHitbox.SetActive(true);
                }
                else if (spriteRenderer.sprite == facingUp)
                {
                    upFacingHitbox.SetActive(true);
                }
                else if (spriteRenderer.sprite = facingRight)
                {
                    rightFacingHitbox.SetActive(true);
                }
            }
            yield return new WaitForSeconds(1f / 12f);
            animator.enabled = false;
        }
        leftFacingHitbox.SetActive(false);
        downFacingHitbox.SetActive(false);
        upFacingHitbox.SetActive(false);
        rightFacingHitbox.SetActive(false);
    }

    void pickSprite(float direction)
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
        leftFacingHitbox.SetActive(false);
        downFacingHitbox.SetActive(false);
        upFacingHitbox.SetActive(false);
        rightFacingHitbox.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        animator.enabled = false;
    }

    void Update()
    {
        pickRendererLayer();
        pickSpritePeriod += Time.deltaTime;
        this.GetComponent<AStarPathfinding>().target = playerShip.transform.position;
        path = GetComponent<AStarPathfinding>().seekPath;
        AStarNode pathNode = path[0];
        Vector3 targetPos = pathNode.nodePosition;
        if (shattering == false)
        {
            travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);
            if (withinRange == false && touchingShip == false)
            {
                moveTowards(travelAngle);
                animator.enabled = false;
                pokePeriod = 1.5f;
                if (pickSpritePeriod >= 0.2f)
                {
                    pickSprite(travelAngle);
                    pickSpritePeriod = 0;
                }
            }
            else
            {
                float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
                pickSprite(angleToShip);
                pokePeriod += Time.deltaTime;
                rigidBody2D.velocity = Vector3.zero;
                if (pokePeriod >= 1.5f && stopAttacking == false)
                {
                    pokePeriod = 0;
                    StartCoroutine(poke());
                }
            }

            if (Vector2.Distance(transform.position, playerShip.transform.position) <= 1.9f)
            {
                withinRange = true;
            }
            else
            {
                withinRange = false;
            }
            spawnFoam();
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }
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
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
            health -= damageDealt;
            if (health <= 0 && alreadySpawned == false)
            {
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(spawnUnArmoured());
                alreadySpawned = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == playerShip)
        {
            touchingShip = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == playerShip)
        {
            touchingShip = false;
        }
    }
}
