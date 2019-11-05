using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantedDoll : MonoBehaviour {
    public Sprite facingDown, facingUp, facingDownRight, facingUpRight;
    SpriteRenderer spriteRenderer;
    CircleCollider2D circCol;
    Animator animator;
    List<AStarNode> path;
    Rigidbody2D rigidBody2D;
    float travelSpeed = 2, travelAngle = 0;
    GameObject targetAttack;
    bool explode = false;
    public GameObject enchantedExplosion;
    private float foamTimer = 0;
    public GameObject waterFoam;
    float pickSpritePeriod = 0;
    float deathPeriod = 0;

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
            spriteRenderer.sprite = facingDownRight;
            transform.localScale = new Vector3(0.2f, 0.2f, 0);
        }
        else if (direction >= 180 && direction <= 265)
        {
            spriteRenderer.sprite = facingDownRight;
            transform.localScale = new Vector3(-0.2f, 0.2f, 0);
        }
        else if (direction < 180 && direction >= 105)
        {
            spriteRenderer.sprite = facingUpRight;
            transform.localScale = new Vector3(-0.2f, 0.2f, 0);
        }
        else
        {
            spriteRenderer.sprite = facingUpRight;
            transform.localScale = new Vector3(0.2f, 0.2f, 0);
        }
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
    
    IEnumerator explosion()
    {
        animator.enabled = true;
        if(spriteRenderer.sprite == facingDown)
        {
            animator.SetTrigger("Explode1");
        }
        else if(spriteRenderer.sprite == facingDownRight)
        {
            animator.SetTrigger("Explode2");
        }
        else if(spriteRenderer.sprite == facingUpRight)
        {
            animator.SetTrigger("Explode3");
        }
        else
        {
            animator.SetTrigger("Explode4");
        }
        yield return new WaitForSeconds(3f / 12f);
        Destroy(this.gameObject);
        Instantiate(enchantedExplosion, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
    }

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator.enabled = false;
	}

	void Update () {
        if (targetAttack != null)
        {
            if (explode == false)
            {
                this.GetComponent<AStarPathfinding>().enabled = true;
                path = GetComponent<AStarPathfinding>().seekPath;
                this.GetComponent<AStarPathfinding>().target = targetAttack.transform.position;
                AStarNode pathNode = path[0];
                Vector3 targetPos = pathNode.nodePosition;
                pickRendererLayer();
                travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);
                pickSpritePeriod += Time.deltaTime;
                if (pickSpritePeriod > 0.2f)
                {
                    pickSprite(travelAngle);
                    pickSpritePeriod = 0;
                }
                moveTowards(travelAngle);

                if (Vector2.Distance(targetAttack.transform.position, transform.position) < 1)
                {
                    explode = true;
                    StartCoroutine(explosion());
                }
            }
            else
            {
                rigidBody2D.velocity = Vector3.zero;
            }
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
            if (deathPeriod > 60 && targetAttack == null)
            {
                deathPeriod = 0;
                StartCoroutine(explosion());
            }
        }
        deathPeriod += Time.deltaTime;
        spawnFoam();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetAttack == null && (collision.gameObject.tag == "MeleeEnemy" || collision.gameObject.tag == "RangedEnemy" || collision.gameObject.tag == "EnemyShield"))
        {
            targetAttack = collision.gameObject;
            this.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
