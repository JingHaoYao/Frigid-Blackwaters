using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eelman : Enemy {
    public Sprite facingLeft, facingRight, facingDown, facingUp;
    SpriteRenderer spriteRenderer;
    Animator animator;
    BoxCollider2D boxCol;
    List<AStarNode> path;
    GameObject playerShip;
    Rigidbody2D rigidBody2D;
    public float travelSpeed = 4;
    private bool isAttacking = false, isSubmerged = false;
    public GameObject waterSplash;
    float angleToShip = 0, moveAngle = 0;
    private bool diveOutAnim = false, diveInAnim = false, attackAnim = false, nearShip = false;
    private float attackPeriod = 0, pickRotatePeriod = 0;
    public GameObject waterFoam;
    private float foamTimer = 0;
    GameObject spawnedElectricPulse;
    public GameObject electricPulse;
    public GameObject obstacleHitBox, deadEelMan, bloodSplatter;
    private int view = 0;

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

    void playAttackAnimations()
    {
        animator.enabled = true;
        if (spriteRenderer.sprite == facingLeft)
        {
            animator.SetTrigger("Attack1");
            view = 1;
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            animator.SetTrigger("Attack2");
            view = 2;
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            animator.SetTrigger("Attack3");
            view = 3;
        }
        else if (spriteRenderer.sprite == facingRight)
        {
            animator.SetTrigger("Attack4");
            view = 4;
        }
        isAttacking = true;
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
        yield return new WaitForSeconds(5f / 12f);
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
            animator.SetTrigger("DiveOut1");
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            animator.SetTrigger("DiveOut2");
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            animator.SetTrigger("DiveOut3");
        }
        else if (spriteRenderer.sprite == facingRight)
        {
            animator.SetTrigger("DiveOut4");
        }
        yield return new WaitForSeconds(6f / 12f);
        animator.enabled = false;
        isSubmerged = false;
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

    void swimTowardsShip()
    {
        if(isSubmerged == true)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            isAttacking = false;
            attackPeriod = 0;
            attackAnim = false;
            diveInAnim = false;
            AStarNode pathNode = path[0];
            Vector3 targetPos = pathNode.nodePosition;
            moveAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);
            if (Vector2.Distance(transform.position, playerShip.transform.position) > 1.5f && nearShip == false)
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
                if(diveOutAnim == false)
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
            if (attackAnim == false)
            {
                attackAnim = true;
                pickSprite(angleToShip);
                playAttackAnimations();
            }

            pickRendererLayer();
            attackPeriod += Time.deltaTime;

            if(Vector2.Distance(transform.position, playerShip.transform.position) <= 1.5f)
            {
                attackPeriod = 0;
            }

            if(attackPeriod > 2 && stopAttacking == false)
            {
                if(diveInAnim == false)
                {
                    diveInAnim = true;
                    StartCoroutine(diveIn());
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

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        playerShip = GameObject.Find("PlayerShip");
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator.enabled = false;
	}
	
	void Update () {
        this.GetComponent<AStarPathfinding>().target = playerShip.transform.position;
        path = GetComponent<AStarPathfinding>().seekPath;
        angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        swimTowardsShip();
        spawnFoam();
        if(isAttacking == true)
        {
            if(spawnedElectricPulse == null && stopAttacking == false)
            {
                spawnedElectricPulse = Instantiate(electricPulse, transform.position, Quaternion.identity);
                spawnedElectricPulse.transform.parent = this.transform;
            }

            if(stopAttacking && spawnedElectricPulse != null)
            {
                Destroy(spawnedElectricPulse);
            }
        }
        else
        {
            if (spawnedElectricPulse != null)
            {
                Destroy(spawnedElectricPulse);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            this.GetComponent<AudioSource>().Play();
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            if (health <= 0)
            {
                Instantiate(bloodSplatter, collision.gameObject.transform.position, Quaternion.identity);
                GameObject deadPirate = Instantiate(deadEelMan, transform.position, Quaternion.identity);
                deadPirate.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
                deadPirate.GetComponent<DeadEnemyScript>().whatView = view;
                deadPirate.transform.localScale = transform.localScale;
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(this.gameObject);
                addKills();
                if(spawnedElectricPulse != null)
                {
                    Destroy(spawnedElectricPulse);
                }
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
