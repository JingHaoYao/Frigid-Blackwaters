using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalHeavyBlademan : Enemy
{
    public Sprite facingLeft, facingUp, facingDown, facingRight;
    public GameObject leftFacingHitbox, downFacingHitbox, upFacingHitbox, rightFacingHitbox;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    Animator animator;
    private bool withinRange = false, touchingShip = false;
    public float travelSpeed = 2;
    public float travelAngle;
    GameObject playerShip;
    private float pokePeriod = 1.5f;
    public GameObject deadSpearman;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    public float withinRangeRadius = 1.9f;
    public float relativeScale = 0.2f;
    bool attacking = false;

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

    IEnumerator poke(float angle)
    {
        int whatView = 0;
        pickSprite(angle);
        if (spriteRenderer.sprite == facingLeft)
        {
            whatView = 1;
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            whatView = 2;
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            whatView = 3;
        }
        else if (spriteRenderer.sprite = facingRight)
        {
            whatView = 4;
        }

        animator.enabled = true;
        attacking = true;
        this.GetComponents<AudioSource>()[1].Play();
        if (whatView == 1)
        {
            animator.SetTrigger("Attack1");
        }
        else if (whatView == 2)
        {
            animator.SetTrigger("Attack2");
        }
        else if (whatView == 3)
        {
            animator.SetTrigger("Attack4");
        }
        else if (whatView == 4)
        {
            animator.SetTrigger("Attack3");
        }
        yield return new WaitForSeconds(6f / 12f);
        if (whatView == 1)
        {
            leftFacingHitbox.SetActive(true);
        }
        else if (whatView == 2)
        {
            downFacingHitbox.SetActive(true);
        }
        else if (whatView == 3)
        {
            upFacingHitbox.SetActive(true);
        }
        else if (whatView == 4)
        {
            rightFacingHitbox.SetActive(true);
        }
        yield return new WaitForSeconds(1f / 12f);
        leftFacingHitbox.SetActive(false);
        downFacingHitbox.SetActive(false);
        upFacingHitbox.SetActive(false);
        rightFacingHitbox.SetActive(false);
        yield return new WaitForSeconds(2f / 12f);
        animator.enabled = false;
        attacking = false;
    }

    void pickSprite(float direction)
    {
        if (direction > 75 && direction < 105)
        {
            spriteRenderer.sprite = facingUp;
            transform.localScale = new Vector3(relativeScale, relativeScale, 0);
        }
        else if (direction < 285 && direction > 265)
        {
            spriteRenderer.sprite = facingDown;
            transform.localScale = new Vector3(relativeScale, relativeScale, 0);
        }
        else if (direction >= 285 && direction <= 360)
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(-relativeScale, relativeScale, 0);
        }
        else if (direction >= 180 && direction <= 265)
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(relativeScale, relativeScale, 0);
        }
        else if (direction < 180 && direction >= 105)
        {
            spriteRenderer.sprite = facingRight;
            transform.localScale = new Vector3(relativeScale, relativeScale, 0);
        }
        else
        {
            spriteRenderer.sprite = facingRight;
            transform.localScale = new Vector3(-relativeScale, relativeScale, 0);
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
        pickSprite(travelAngle);
    }

    void Update()
    {
        pickRendererLayer();
        path = GetComponent<AStarPathfinding>().seekPath;
        this.GetComponent<AStarPathfinding>().target = playerShip.transform.position;
        Vector3 targetPos = Vector3.zero;
        if (path[0] != null)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }
        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);
        pickSpritePeriod += Time.deltaTime;

        if (withinRange == false && touchingShip == false)
        {
            if (animator.enabled == false)
            {
                moveTowards(travelAngle);
                pokePeriod = 1.5f;
                if (pickSpritePeriod >= 0.2f)
                {
                    pickSprite(travelAngle);
                    pickSpritePeriod = 0;
                }
            }
        }
        else
        {
            pokePeriod += Time.deltaTime;
            rigidBody2D.velocity = Vector3.zero;
            float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            if (attacking == false)
            {
                pickSprite(angleToShip);
            }

            if (pokePeriod >= 1f && stopAttacking == false)
            {
                pokePeriod = 0;
                StartCoroutine(poke(angleToShip));
            }
        }

        if (Vector2.Distance(transform.position, playerShip.transform.position) <= withinRangeRadius)
        {
            withinRange = true;
        }
        else
        {
            withinRange = false;
        }
        spawnFoam();
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
            this.GetComponents<AudioSource>()[0].Play();
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            if (health <= 0)
            {
                GameObject deadPirate = Instantiate(deadSpearman, transform.position, Quaternion.identity);
                if (deadPirate.GetComponent<DeadEnemyScript>())
                {
                    deadPirate.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
                    deadPirate.GetComponent<DeadEnemyScript>().whatView = whatView();
                    deadPirate.transform.localScale = transform.localScale;
                }
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
