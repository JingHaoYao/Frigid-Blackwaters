using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMage : Enemy
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidBody2D;
    public Sprite[] closedViews;
    public GameObject crabMageShot;
    public GameObject deadCrab;
    GameObject playerShip;
    float travelAngle;

    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    int whatView = 0;
    int mirror = 1;
    public float travelSpeed = 1;
    float attackPeriod = 0;
    bool isAttacking = false;

    BoxCollider2D hitBox;
    float angleToShip = 0;

    Vector3 targetPosition;

    public GameObject invulnerableIcon;

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

    void pickView(float angle)
    {
        if (angle > 255 && angle <= 285)
        {
            whatView = 2;
            mirror = 1;
            invulnerableIcon.GetComponent<DirectionalInvulnerableEffect>().whichDirectionToAvoid = 1;
        }
        else if (angle > 285 && angle <= 360)
        {
            whatView = 1;
            mirror = 1;
            invulnerableIcon.GetComponent<DirectionalInvulnerableEffect>().whichDirectionToAvoid = 2;
        }
        else if (angle > 180 && angle <= 255)
        {
            whatView = 1;
            mirror = -1;
            invulnerableIcon.GetComponent<DirectionalInvulnerableEffect>().whichDirectionToAvoid = 0;
        }
        else if (angle > 75 && angle <= 105)
        {
            whatView = 4;
            mirror = -1;
            invulnerableIcon.GetComponent<DirectionalInvulnerableEffect>().whichDirectionToAvoid = 3;
        }
        else if (angle >= 0 && angle <= 75)
        {
            whatView = 3;
            mirror = -1;
            invulnerableIcon.GetComponent<DirectionalInvulnerableEffect>().whichDirectionToAvoid = 2;
        }
        else
        {
            whatView = 3;
            mirror = 1;
            invulnerableIcon.GetComponent<DirectionalInvulnerableEffect>().whichDirectionToAvoid = 0;
        }
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * travelSpeed;
    }

    bool determineHit(Vector3 pos)
    {
        float determineAngle = 0;
        if(whatView == 1)
        {
            if (mirror == 1)
            {
                determineAngle = 135;
            }
            else
            {
                determineAngle = 45;
            }
        }
        else if(whatView == 2)
        {
            determineAngle = 90;
        }
        else if(whatView == 3)
        {
            if (mirror == 1)
            {
                determineAngle = 315;
            }
            else
            {
                determineAngle = 225;
            }
        }
        else
        {
            determineAngle = 270;
        }

        float angleBullet = (Mathf.Atan2(pos.y - transform.position.y, pos.x - transform.position.x) * Mathf.Rad2Deg + 360) % 360;

        if (determineAngle - 45 < angleBullet && angleBullet < determineAngle + 45)
        {
            return true;
        }
        return false;
    }

    IEnumerator attack()
    {
        targetPosition = pickRandPos();
        for (int i = 0; i < 3; i++)
        {
            pickView(angleToShip);
            int viewPicked = whatView;
            float attackAngle = angleToShip;
            animator.enabled = true;
            animator.SetTrigger("Attack" + whatView.ToString());
            yield return new WaitForSeconds(4f / 12f);
            this.GetComponents<AudioSource>()[1].Play();
            Vector3 summonPos = Vector3.zero;
            switch (viewPicked)
            {
                case 1:
                    summonPos = new Vector3(mirror * 1.93f, 0.81f);
                    break;
                case 2:
                    summonPos = new Vector3(mirror * 0.43f, 1.06f);
                    break;
                case 3:
                    summonPos = new Vector3(mirror * -1.69f, 0.94f);
                    break;
                case 4:
                    summonPos = new Vector3(mirror * -0.5f, 1.17f);
                    break;
            }
            GameObject blast = Instantiate(crabMageShot, transform.position + summonPos, Quaternion.identity);
            blast.GetComponent<BasicProjectile>().angleTravel = attackAngle;
            blast.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            yield return new WaitForSeconds(7f / 12f);
        }
        animator.enabled = false;
        isAttacking = false;
    }

    Vector3 pickRandPos()
    {
        Vector3 currRandPos = new Vector3(Camera.main.transform.position.x + -7 + Random.Range(0, 15), Camera.main.transform.position.y + -7 + Random.Range(0, 15));
        while(Physics2D.OverlapCircle(currRandPos, 0.5f) == true || Vector2.Distance(currRandPos, transform.position) < 4)
        {
           currRandPos = new Vector3(Camera.main.transform.position.x + -7 + Random.Range(0, 15), Camera.main.transform.position.y + -7 + Random.Range(0, 15));
        }
        return currRandPos;
    }

    void travelLocation()
    {
        path = GetComponent<AStarPathfinding>().seekPath;
        this.GetComponent<AStarPathfinding>().target = targetPosition;
        AStarNode pathNode = path[1];
        Vector3 targetPos = pathNode.nodePosition;
        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if (isAttacking == false)
        {
            moveTowards(travelAngle);
            pickView(travelAngle);
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }

        if (Vector2.Distance(transform.position, targetPosition) < 1.5f && isAttacking == false && stopAttacking == false)
        {
            isAttacking = true;
            StartCoroutine(attack());
        }

        transform.localScale = new Vector3(4f * mirror, 4f);
        pickSpritePeriod += Time.deltaTime;

        if (pickSpritePeriod >= 0.4f)
        {
            if (isAttacking == false)
            {
                spriteRenderer.sprite = closedViews[whatView - 1];
            }
            pickSpritePeriod = 0;
        }
    }

    void Start()
    {
        hitBox = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator.enabled = false;
        playerShip = FindObjectOfType<PlayerScript>().gameObject;
        attackPeriod = Random.Range(2f, 6f);
        targetPosition = pickRandPos();
    }

    void Update()
    {
        angleToShip = (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 360f) % 360f;
        spawnFoam();
        travelLocation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            if (determineHit(collision.gameObject.transform.position))
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
                this.GetComponents<AudioSource>()[0].Play();
                if (health <= 0)
                {
                    GameObject dead = Instantiate(deadCrab, transform.position, Quaternion.identity);
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

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
