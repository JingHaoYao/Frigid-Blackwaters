using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerFish : Enemy
{
    public Sprite facingLeft, facingRight, facingDown, facingUp;
    SpriteRenderer spriteRenderer;
    Animator animator;
    BoxCollider2D boxCol;
    List<AStarNode> path;
    GameObject playerShip;
    Rigidbody2D rigidBody2D;
    public float travelSpeed = 4;
    private bool isSubmerged = false;
    public GameObject waterSplash;
    float angleToShip = 0, moveAngle = 0;
    private bool diveOutAnim = false, diveInAnim = false, nearShip = false;
    private float attackPeriod = 0, pickRotatePeriod = 0;
    public GameObject waterFoam;
    private float foamTimer = 0;
    public GameObject obstacleHitBox, deadEelMan, bloodSplatter;
    public GameObject anglerFishShot;
    Vector3 targetPosition;
    float offset = 0;

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
            animator.SetTrigger("DiveIn1");
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            animator.SetTrigger("DiveIn2");
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            animator.SetTrigger("DiveIn3");
        }
        else if (spriteRenderer.sprite == facingRight)
        {
            animator.SetTrigger("DiveIn4");
        }
        yield return new WaitForSeconds(4f / 12f);
        Instantiate(waterSplash, transform.position, Quaternion.identity);
        isSubmerged = true;
        spriteRenderer.enabled = false;
    }

    IEnumerator diveOut()
    {
        spriteRenderer.enabled = true;
        animator.enabled = true;
        pickSprite(angleToShip);
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
            animator.SetTrigger("Attack3");
        }
        else if (spriteRenderer.sprite == facingRight)
        {
            animator.SetTrigger("Attack4");
        }
        Instantiate(waterSplash, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3f / 12f);
        this.GetComponents<AudioSource>()[0].Play();
        for(int i = 0; i < 6; i++)
        {
            GameObject instant = Instantiate(anglerFishShot, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            instant.GetComponent<StarfishEnemyShot>().angleTravel = (i * 60 + offset) * Mathf.Deg2Rad;
            instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        yield return new WaitForSeconds(5f / 12f);
        animator.enabled = false;
        isSubmerged = false;
    }

    void pickSprite(float direction)
    {
        if (direction > 75 && direction < 105)
        {
            spriteRenderer.sprite = facingUp;
            transform.localScale = new Vector3(0.35f, 0.35f, 0);
        }
        else if (direction < 285 && direction > 265)
        {
            spriteRenderer.sprite = facingDown;
            transform.localScale = new Vector3(0.35f, 0.35f, 0);
        }
        else if (direction >= 285 && direction <= 360)
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(-0.35f, 0.35f, 0);
        }
        else if (direction >= 180 && direction <= 265)
        {
            spriteRenderer.sprite = facingLeft;
            transform.localScale = new Vector3(0.35f, 0.35f, 0);
        }
        else if (direction < 180 && direction >= 105)
        {
            spriteRenderer.sprite = facingRight;
            transform.localScale = new Vector3(-0.35f, 0.35f, 0);
        }
        else
        {
            spriteRenderer.sprite = facingRight;
            transform.localScale = new Vector3(0.35f, 0.35f, 0);
        }
    }

    Vector3 pickRandPos()
    {
        float randX;
        float randY;
        if (Random.Range(0, 2) == 1)
        {
            randX = transform.position.x + Random.Range(5.0f, 6.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(5.0f, 6.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-6.0f, -5.0f);
            }
        }
        else
        {
            randX = transform.position.x + Random.Range(-6.0f, -5.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(5.0f, 6.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-6.0f, -5.0f);
            }
        }

        Vector3 randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        while (Physics2D.OverlapCircle(randPos, .5f) || Vector2.Distance(randPos, transform.position) < 2)
        {
            if (Random.Range(0, 2) == 1)
            {
                randX = transform.position.x + Random.Range(5.0f, 6.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(5.0f, 6.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-6.0f, -5.0f);
                }
            }
            else
            {
                randX = transform.position.x + Random.Range(-6.0f, -5.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(5.0f, 6.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-6.0f, -5.0f);
                }
            }
            randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        }
        return randPos;
    }

    void swimTowardsShip()
    {
        if (isSubmerged == true)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            attackPeriod = 0;
            diveInAnim = false;
            AStarNode pathNode = path[0];
            Vector3 targetPos = pathNode.nodePosition;
            moveAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);
            if (Vector2.Distance(transform.position, path.ToArray()[path.Count - 1].nodePosition) > 1f && nearShip == false)
            {
                moveTowards(moveAngle);
                pickRotatePeriod += Time.deltaTime;
                if (pickRotatePeriod > 0.2f)
                {
                    pickRotatePeriod = 0;
                }
            }
            else
            {
                nearShip = true;
                rigidBody2D.velocity = Vector3.zero;
                if (diveOutAnim == false)
                {
                    diveOutAnim = true;
                    StartCoroutine(diveOut());
                }
            }
            spriteRenderer.sortingOrder = -950;
            boxCol.enabled = false;
            obstacleHitBox.SetActive(false);
        }
        else
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            boxCol.enabled = true;
            obstacleHitBox.SetActive(true);
            nearShip = false;
            diveOutAnim = false;

            pickRendererLayer();
            attackPeriod += Time.deltaTime;

            if (Vector2.Distance(transform.position, playerShip.transform.position) <= 1.5f)
            {
                attackPeriod = 0;
            }
            pickSprite(angleToShip);

            if (attackPeriod > 2 && stopAttacking == false)
            {
                if (diveInAnim == false)
                {
                    diveInAnim = true;
                    StartCoroutine(diveIn());
                    targetPosition = pickRandPos();
                }
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

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        playerShip = GameObject.Find("PlayerShip");
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator.enabled = false;
        targetPosition = pickRandPos();
        offset = Random.Range(0, 2) * 30;
    }

    void Update()
    {
        this.GetComponent<AStarPathfinding>().target = targetPosition;
        path = GetComponent<AStarPathfinding>().seekPath;
        angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        swimTowardsShip();
        spawnFoam();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            this.GetComponents<AudioSource>()[1].Play();
            int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
            health -= damageDealt;
            if (health <= 0)
            {
                Instantiate(bloodSplatter, collision.gameObject.transform.position, Quaternion.identity);
                GameObject deadPirate = Instantiate(deadEelMan, transform.position, Quaternion.identity);
                deadPirate.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
                deadPirate.GetComponent<DeadEnemyScript>().whatView = whatView();
                deadPirate.transform.localScale = transform.localScale;
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(this.gameObject);
                addKills();
            }
            else
            {
                StartCoroutine(hitFrame());
            }
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
