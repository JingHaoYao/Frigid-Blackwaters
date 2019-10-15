using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalScatterMage : Enemy
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
    public float relativeScale = 2.5f;
    bool attacking = false;
    public GameObject scatterShot;
    Vector3 randPos;

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

    IEnumerator fireCharges(float angleAttack)
    {
        animator.enabled = true;
        attacking = true;
        this.GetComponents<AudioSource>()[1].Play();
        pickSprite(angleAttack);
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
        yield return new WaitForSeconds(4f / 12f);

        for (int i = 0; i < 10; i++)
        {
            for (int k = 0; k < 4; k++)
            {
                GameObject spawnedShot = Instantiate(scatterShot, transform.position + Vector3.up + new Vector3(Mathf.Cos(angleAttack * Mathf.Deg2Rad) + Random.Range(-1f, 1f), Mathf.Sin(angleAttack * Mathf.Deg2Rad) + Random.Range(-1f, 1f)) * 0.5f, Quaternion.identity);
                spawnedShot.GetComponent<SkeletalScatterMageShot>().angleTravel = Random.Range(angleAttack - 15, angleAttack + 15) * Mathf.Deg2Rad;
                spawnedShot.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(8f / 12f);
        animator.enabled = false;
        attacking = false;
    }

    Vector3 pickRandPos()
    {
        float randX;
        float randY;
        if (Random.Range(0, 2) == 1)
        {
            randX = transform.position.x + Random.Range(3.0f, 4.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(3.0f, 4.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-4.0f, -3.0f);
            }
        }
        else
        {
            randX = transform.position.x + Random.Range(-4.0f, -3.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(3.0f, 4.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-4.0f, -3.0f);
            }
        }

        Vector3 randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        while (Physics2D.OverlapCircle(randPos, .5f) || Vector2.Distance(randPos, transform.position) < 2)
        {
            if (Random.Range(0, 2) == 1)
            {
                randX = transform.position.x + Random.Range(3.0f, 4.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(3.0f, 4.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-4.0f, -3.0f);
                }
            }
            else
            {
                randX = transform.position.x + Random.Range(-4.0f, -3.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(3.0f, 4.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-4.0f, -3.0f);
                }
            }
            randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        }
        return randPos;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        animator.enabled = false;
        pickSprite(travelAngle);
        randPos = pickRandPos();
    }

    void Update()
    {
        pickRendererLayer();
        path = GetComponent<AStarPathfinding>().seekPath;
        this.GetComponent<AStarPathfinding>().target = randPos;
        Vector3 targetPos = Vector3.zero;
        float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;

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
            if (attacking == false)
            {
                pickSpritePeriod += Time.deltaTime;
                if (pickSpritePeriod >= 0.2f)
                {
                    pickSprite(angleToShip);
                    pickSpritePeriod = 0;
                }
                rigidBody2D.velocity = Vector3.zero;
            }
        }

        if (attacking == false)
        {
            chargeTimer += Time.deltaTime;
            if (chargeTimer > 6 && stopAttacking == false)
            {
                StartCoroutine(fireCharges(angleToShip));
                chargeTimer = 0;
            }
        }

        if (Vector2.Distance(transform.position, randPos) <= 1.5f)
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
            randPos = pickRandPos();
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
