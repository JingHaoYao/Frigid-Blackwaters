using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridCrab1 : Enemy
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidBody2D;
    public Sprite[] viewSprites;
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
    float angleToShip = 0;

    public GameObject smallShell, mediumShell, bigShell, shieldShell;

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

    IEnumerator summonShells(float delayBetweenShells, Vector3 target, float angleAttack)
    {
        yield return new WaitForSeconds(delayBetweenShells);
        GameObject spawnedShell = Instantiate(smallShell, target + new Vector3(Mathf.Cos((angleAttack + 180) * Mathf.Deg2Rad), Mathf.Sin((angleAttack + 180) * Mathf.Deg2Rad)) * 3.5f, Quaternion.identity);
        spawnedShell.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        if (Random.Range(0, 2) == 1)
        {
            Vector3 scale = spawnedShell.transform.localScale;
            scale.x *= -1;
            spawnedShell.transform.localScale = scale;
        }
        yield return new WaitForSeconds(delayBetweenShells);
        spawnedShell = Instantiate(mediumShell, target + new Vector3(Mathf.Cos((angleAttack + 180) * Mathf.Deg2Rad), Mathf.Sin((angleAttack + 180) * Mathf.Deg2Rad)) * 2f, Quaternion.identity);
        spawnedShell.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        if (Random.Range(0, 2) == 1)
        {
            Vector3 scale = spawnedShell.transform.localScale;
            scale.x *= -1;
            spawnedShell.transform.localScale = scale;
        }
        yield return new WaitForSeconds(delayBetweenShells);
        spawnedShell = Instantiate(bigShell, target, Quaternion.identity);
        spawnedShell.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        if (Random.Range(0, 2) == 1)
        {
            Vector3 scale = spawnedShell.transform.localScale;
            scale.x *= -1;
            spawnedShell.transform.localScale = scale;
        }
    }

    void summonShieldShells()
    {
        float factor = 0;

        if (Random.Range(0, 2) == 1)
        {
            factor = 45;
        }

        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90 + factor;
            Vector3 pos = transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 1f;
            GameObject spawnedShell = Instantiate(shieldShell, pos, Quaternion.identity);
            spawnedShell.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            if (Random.Range(0, 2) == 1)
            {
                Vector3 scale = spawnedShell.transform.localScale;
                scale.x *= -1;
                spawnedShell.transform.localScale = scale;
            }
        }
    }

    IEnumerator attack()
    {
        isAttacking = true;
        animator.enabled = true;
        animator.SetTrigger("Attack" + whatView.ToString());
        yield return new WaitForSeconds(4f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        if (stopAttacking == false)
        {
            float angleAttack = angleToShip;
            //summonShieldShells();
            StartCoroutine(summonShells(0.2f, playerShip.transform.position, angleAttack));
        }
        yield return new WaitForSeconds(6f / 12f);
        animator.enabled = false;
        yield return new WaitForSeconds(18f / 12f + 0.5f);
        isAttacking = false;
    }

    void travelLocation()
    {
        path = GetComponent<AStarPathfinding>().seekPath;
        this.GetComponent<AStarPathfinding>().target = playerShip.transform.position;
        AStarNode pathNode = path[0];
        Vector3 targetPos = pathNode.nodePosition;
        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if (Vector2.Distance(transform.position, playerShip.transform.position) > 4 && attackPeriod > 1 && isAttacking == false)
        {
            moveTowards(travelAngle);
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
        }

        if (attackPeriod > 0)
        {
            attackPeriod -= Time.deltaTime;
        }
        else
        {
            if (stopAttacking == false)
            {
                StartCoroutine(attack());
                attackPeriod = 6;
            }
        }

        transform.localScale = new Vector3(0.35f * mirror, 0.35f);
        pickSpritePeriod += Time.deltaTime;

        if (pickSpritePeriod >= 0.2f)
        {
            spriteRenderer.sprite = viewSprites[whatView - 1];
            pickSpritePeriod = 0;
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator.enabled = false;
        playerShip = FindObjectOfType<PlayerScript>().gameObject;
        attackPeriod = Random.Range(2f, 6f);
    }

    void Update()
    {
        angleToShip = (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 360f) % 360f;
        spawnFoam();
        pickView(angleToShip);
        travelLocation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
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

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
