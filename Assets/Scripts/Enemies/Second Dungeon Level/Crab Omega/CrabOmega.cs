using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabOmega : Enemy
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidBody2D;
    public Sprite[] closedViews;
    public Sprite[] openViews;
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
    bool isAttacking = false;

    public GameObject invulnerableHitBox;
    BoxCollider2D hitBox;
    bool invulnerable = true;
    public GameObject damageHitBox;
    public GameObject dash;

    public GameObject invulnerabilityIcon;

    public LayerMask mask;

    GameObject spawnedBurst;
    GameObject spawnedDash;
    float bufferPeriod = 0;

    public GameObject waterFoamBurst;

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
        }
        else if (angle > 285 && angle <= 360)
        {
            whatView = 1;
            mirror = 1;
        }
        else if (angle > 180 && angle <= 255)
        {
            whatView = 1;
            mirror = -1;
        }
        else if (angle > 75 && angle <= 105)
        {
            whatView = 4;
            mirror = -1;
        }
        else if (angle >= 0 && angle <= 75)
        {
            whatView = 3;
            mirror = -1;
        }
        else
        {
            whatView = 3;
            mirror = 1;
        }
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * travelSpeed;
    }

    IEnumerator attack()
    {
        rigidBody2D.velocity = Vector3.zero;
        yield return new WaitForSeconds(1.5f);
        float angleShip = (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 360f) % 360f;
        damageHitBox.SetActive(true);
        spawnedBurst = Instantiate(waterFoamBurst, transform.position, Quaternion.Euler(0, 0, angleShip + 90));
        rigidBody2D.velocity = new Vector3(Mathf.Cos(angleShip * Mathf.Deg2Rad), Mathf.Sin(angleShip * Mathf.Deg2Rad)) * 15;
        spawnedDash = Instantiate(dash, transform.position, Quaternion.Euler(0, 0, angleShip));
        animator.enabled = true;
        this.GetComponents<AudioSource>()[1].Play();
        animator.SetTrigger("Attack" + whatView);
        for(int i = 0; i < 15; i++)
        {
            rigidBody2D.velocity = new Vector3(Mathf.Cos(angleShip * Mathf.Deg2Rad), Mathf.Sin(angleShip * Mathf.Deg2Rad)) * (16 - i);
            yield return new WaitForSeconds((4f / 12f) / 15f);
        }
        isAttacking = false;
        rigidBody2D.velocity = Vector3.zero;
        animator.enabled = false;
        damageHitBox.SetActive(false);
        bufferPeriod = 1;
    }

    void travelLocation()
    {
        path = GetComponent<AStarPathfinding>().seekPath;
        this.GetComponent<AStarPathfinding>().target = playerShip.transform.position;
        AStarNode pathNode = path[0];
        Vector3 targetPos = pathNode.nodePosition;
        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if(bufferPeriod > 0)
        {
            bufferPeriod -= Time.deltaTime;
        }

        if (isAttacking == false && bufferPeriod <= 0)
        {
            moveTowards(travelAngle);
        }

        float angle = (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 360f) % 360f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0) + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)), new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)), 20, mask);

        if (Vector2.Distance(transform.position, playerShip.transform.position) < 3 && hit.transform.gameObject == playerShip && hit == isAttacking == false)
        {
            isAttacking = true;
            StartCoroutine(attack());
        }

        transform.localScale = new Vector3(2 * mirror, 2);
        pickSpritePeriod += Time.deltaTime;

        if (pickSpritePeriod >= 0.2f)
        {
            if (isAttacking == false)
            {
                spriteRenderer.sprite = closedViews[whatView - 1];
            }
            else
            {
                spriteRenderer.sprite = openViews[whatView - 1];
            }
            pickSpritePeriod = 0;
        }

        if (isAttacking == false)
        {
            hitBox.enabled = false;
            invulnerableHitBox.SetActive(true);
            invulnerabilityIcon.SetActive(true);
            invulnerable = true;
        }
        else
        {
            hitBox.enabled = true;
            invulnerableHitBox.SetActive(false);
            invulnerabilityIcon.SetActive(false);
            invulnerable = false;
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
        damageHitBox.SetActive(false);
    }

    void Update()
    {
        float angleToShip = (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 360f) % 360f;
        spawnFoam();
        pickView(angleToShip);
        travelLocation();
        if (spawnedDash)
        {
            spawnedDash.transform.position = transform.position + new Vector3(0, 0.5f,0);
            spawnedDash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 3;
        }

        if (spawnedBurst)
        {
            spawnedBurst.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0 && invulnerable == false)
        {
            int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
            health -= damageDealt;
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

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
