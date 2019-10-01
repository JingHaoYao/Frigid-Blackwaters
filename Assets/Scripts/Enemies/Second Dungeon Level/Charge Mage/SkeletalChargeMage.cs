using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalChargeMage : Enemy
{
    public Sprite facingLeft, facingUp, facingDown, facingRight;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    Animator animator;
    private bool withinRange = false;
    public float travelSpeed = 2;
    public float travelAngle;
    GameObject playerShip;
    public GameObject deadSpearman;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    public float withinRangeRadius = 5f;
    public float relativeScale = 4f;
    bool attacking = false;

    public GameObject chargeMageCharge;
    GameObject[] chargeList = new GameObject[3];

    float chargeTimer = 0;

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
            transform.localScale = new Vector3(-relativeScale, relativeScale, 0);
        }
        else
        {
            spriteRenderer.sprite = facingRight;
            transform.localScale = new Vector3(relativeScale, relativeScale, 0);
        }
    }
    
    IEnumerator addCharge()
    {
        animator.enabled = true;
        attacking = true;
        if (spriteRenderer.sprite == facingLeft)
        {
            animator.SetTrigger("Attack1");
        }
        else if (spriteRenderer.sprite == facingRight)
        {
            animator.SetTrigger("Attack3");
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            animator.SetTrigger("Attack4");
        }
        else
        {
            animator.SetTrigger("Attack2");
        }
        yield return new WaitForSeconds(5f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        for(int i = 0; i < chargeList.Length; i++)
        {
            if(chargeList[i] == null)
            {
                float angleToShip = Vector2.Angle(transform.position, playerShip.transform.position);
                GameObject charge = Instantiate(chargeMageCharge, transform.position + new Vector3(Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(angleToShip * Mathf.Deg2Rad)) * (i * 2 + 4), Quaternion.identity);
                charge.GetComponent<ChargeMageProjectile>().target = gameObject;
                charge.GetComponent<ProjectileParent>().instantiater = gameObject;
                break;
            }
        }
        yield return new WaitForSeconds(9f / 12f);
        animator.enabled = false;
        attacking = false;
    }


    void Start()
    {
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


        if (withinRange == false && attacking == false)
        {
            moveTowards(travelAngle);
            pickSpritePeriod += Time.deltaTime;
            if (pickSpritePeriod >= 0.2f)
            {
                pickSprite(travelAngle);
                pickSpritePeriod = 0;
            }
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }

        if(attacking == false && chargeList[chargeList.Length - 1] == null)
        {
            chargeTimer += Time.deltaTime;
            if (chargeTimer > 2 && stopAttacking == false)
            {
                StartCoroutine(addCharge());
                chargeTimer = 0;
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
            int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
            health -= damageDealt;
            if (health <= 0)
            {
                GameObject deadPirate = Instantiate(deadSpearman, transform.position, Quaternion.identity);
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
