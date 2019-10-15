using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalPirate : Enemy
{
    Rigidbody2D rigidBody2D;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public GameObject deadSkeletalPirate;
    GameObject playerShip;
    public GameObject leftFacingHitbox, downFacingHitbox, upFacingHitbox, rightFacingHitbox;
    public Sprite facingUp, facingLeft, facingRight, facingDown;
    private float travelAngle = 0;
    public float travelSpeed = 1;
    public float slashPeriod = 0.55f;
    bool touchingBoat = false;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;

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

    void pickRendererLayer()
    {
        /*if (transform.position.y < playerShip.transform.position.y)
        {
            spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
        else
        {
            spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }*/
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * travelSpeed;
    }

    IEnumerator slash()
    {
        animator.enabled = true;
        this.GetComponents<AudioSource>()[1].Play();
        if (spriteRenderer.sprite == facingLeft)
        {
            animator.SetTrigger("Slash1");
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            animator.SetTrigger("Slash2");
        }
        else if(spriteRenderer.sprite == facingUp)
        {
            animator.SetTrigger("Slash3");
        }
        else  if(spriteRenderer.sprite == facingRight)
        {
            animator.SetTrigger("Slash4");
        }
        yield return new WaitForSeconds(4f / 12f);
        if (touchingBoat == true)
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
        leftFacingHitbox.SetActive(false);
        downFacingHitbox.SetActive(false);
        upFacingHitbox.SetActive(false);
        rightFacingHitbox.SetActive(false);
        animator.enabled = false;
    }

    void Start()
    {
        leftFacingHitbox.SetActive(false);
        downFacingHitbox.SetActive(false);
        upFacingHitbox.SetActive(false);
        rightFacingHitbox.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator.enabled = false;
        playerShip = GameObject.Find("PlayerShip");
    }

    void Update()
    {
        path = GetComponent<AStarPathfinding>().seekPath;
        this.GetComponent<AStarPathfinding>().target = playerShip.transform.position;
        AStarNode pathNode = path[0];
        Vector3 targetPos = pathNode.nodePosition;
        pickRendererLayer();
        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if (touchingBoat == false) {
            moveTowards(travelAngle);
            animator.enabled = false;
            slashPeriod = 1.0f;
            pickSpritePeriod += Time.deltaTime;
            if (pickSpritePeriod > 0.2f)
            {
                pickSprite(travelAngle);
                pickSpritePeriod = 0;
            }
        }
        else
        {
            float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            pickSprite(angleToShip);
            slashPeriod += Time.deltaTime;
            rigidBody2D.velocity = Vector3.zero;
            if(slashPeriod >= 1.0f && stopAttacking == false)
            {
                slashPeriod = 0;
                StartCoroutine(slash());
            }
        }
        spawnFoam();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "playerHitBox")
        {
            touchingBoat = true;
        }

        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            this.GetComponents<AudioSource>()[0].Play();
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            if (health <= 0)
            {
                GameObject deadPirate = Instantiate(deadSkeletalPirate, transform.position, Quaternion.identity);
                deadPirate.GetComponent<DeadSkeletalPirate>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
                deadPirate.GetComponent<DeadSkeletalPirate>().whatView = whatView();
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            touchingBoat = false;
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
