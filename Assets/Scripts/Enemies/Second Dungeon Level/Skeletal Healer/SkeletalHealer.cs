using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalHealer : Enemy
{
    public Sprite facingLeft, facingUp, facingDown, facingRight;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    Animator animator;
    private bool withinRange = false, touchingShip = false;
    public float travelAngle;
    GameObject playerShip;
    private float pokePeriod = 1.5f;
    public GameObject deadSpearman;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    public float withinRangeRadius = 1f;
    public float relativeScale = 0.2f;
    bool attacking = false;
    public GameObject healEffect;
    GameObject targetEnemy;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * speed / 3f)
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
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
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

    IEnumerator upgradeArmor(Enemy enemy)
    {
        animator.enabled = true;
        attacking = true;
        if (spriteRenderer.sprite == facingLeft)
        {
            animator.SetTrigger("Heal1");
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            animator.SetTrigger("Heal2");
        }
        else if (spriteRenderer.sprite == facingUp)
        {
            animator.SetTrigger("Heal4");
        }
        else if (spriteRenderer.sprite == facingRight)
        {
            animator.SetTrigger("Heal3");
        }
        yield return new WaitForSeconds(7f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        enemy.heal(1);
        targetEnemy = pickEnemyToUpgrade();
        GameObject instant = Instantiate(healEffect, enemy.transform.position, Quaternion.identity);
        instant.GetComponent<FollowObject>().objectToFollow = enemy.gameObject;
        yield return new WaitForSeconds(8f / 12f);
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
            transform.localScale = new Vector3(-relativeScale, relativeScale, 0);
        }
        else
        {
            spriteRenderer.sprite = facingRight;
            transform.localScale = new Vector3(relativeScale, relativeScale, 0);
        }
    }

    GameObject pickEnemyToUpgrade()
    {
        GameObject[] ActiveRangedEnemies = GameObject.FindGameObjectsWithTag("RangedEnemy");
        GameObject[] ActiveMeleeEnemies = GameObject.FindGameObjectsWithTag("MeleeEnemy");

        if (ActiveRangedEnemies.Length > 0 || ActiveMeleeEnemies.Length > 0)
        {
            if (ActiveRangedEnemies.Length == 0)
            {
                foreach (GameObject enemy in ActiveMeleeEnemies)
                {
                    if (enemy.GetComponent<Enemy>().health < enemy.GetComponent<Enemy>().maxHealth != this.gameObject)
                    {
                        return enemy;
                    }
                }
                return null;
            }
            else if (ActiveMeleeEnemies.Length == 0)
            {
                foreach (GameObject enemy in ActiveRangedEnemies)
                { 
                    if (enemy.GetComponent<Enemy>().health < enemy.GetComponent<Enemy>().maxHealth && enemy != this.gameObject)
                    {
                        return enemy;
                    }
                }
                return null;
            }
            else
            {
                int whichToUpgrade = Random.Range(0, 2);
                if (whichToUpgrade == 1)
                {
                    foreach (GameObject enemy in ActiveRangedEnemies)
                    {
                        if (enemy.GetComponent<Enemy>().health < enemy.GetComponent<Enemy>().maxHealth && enemy != this.gameObject)
                        {
                            return enemy;
                        }
                    }

                    foreach (GameObject enemy in ActiveMeleeEnemies)
                    {
                        if (enemy.GetComponent<Enemy>().health < enemy.GetComponent<Enemy>().maxHealth && enemy != this.gameObject)
                        {
                            return enemy;
                        }
                    }

                    return null;
                }
                else
                {
                    foreach (GameObject enemy in ActiveMeleeEnemies)
                    {
                        if (enemy.GetComponent<Enemy>().health < enemy.GetComponent<Enemy>().maxHealth && enemy != this.gameObject)
                        {
                            return enemy;
                        }
                    }

                    foreach (GameObject enemy in ActiveRangedEnemies)
                    {
                        if (enemy.GetComponent<Enemy>().health < enemy.GetComponent<Enemy>().maxHealth && enemy != this.gameObject)
                        {
                            return enemy;
                        }
                    }

                    return null;
                }
            }
        }
        else
        {
            return null;
        }

    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        animator.enabled = false;
        pickSprite(travelAngle);
        targetEnemy = pickEnemyToUpgrade();
    }

    void Update()
    {
        if (targetEnemy != null)
        {
            path = GetComponent<AStarPathfinding>().seekPath;
            this.GetComponent<AStarPathfinding>().target = targetEnemy.transform.position;
            Vector3 targetPos = Vector3.zero;
            if (path[0] != null)
            {
                AStarNode pathNode = path[0];
                targetPos = pathNode.nodePosition;
            }
            travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);
            pickSpritePeriod += Time.deltaTime;

            if (withinRange == false)
            {
                if (attacking == false)
                {
                    moveTowards(travelAngle);
                    animator.enabled = false;
                    if (pickSpritePeriod >= 0.2f)
                    {
                        pickSprite(travelAngle);
                        pickSpritePeriod = 0;
                    }
                }
            }
            else
            {
                if (attacking == false)
                {
                    StartCoroutine(upgradeArmor(targetEnemy.GetComponent<Enemy>()));
                }
                rigidBody2D.velocity = Vector3.zero;
            }

            if (Vector2.Distance(transform.position, targetEnemy.transform.position) <= withinRangeRadius)
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
            float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            pickSpritePeriod += Time.deltaTime;
            if (pickSpritePeriod >= 0.2f)
            {
                targetEnemy = pickEnemyToUpgrade();
                pickSprite(angleToShip);
                pickSpritePeriod = 0;
            }
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
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
    }

    public override void deathProcedure()
    {
        GameObject deadPirate = Instantiate(deadSpearman, transform.position, Quaternion.identity);
        if (deadPirate.GetComponent<DeadEnemyScript>())
        {
            deadPirate.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
            deadPirate.GetComponent<DeadEnemyScript>().whatView = whatView();
            deadPirate.transform.localScale = transform.localScale;
        }
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        this.GetComponents<AudioSource>()[0].Play();
    }
}
