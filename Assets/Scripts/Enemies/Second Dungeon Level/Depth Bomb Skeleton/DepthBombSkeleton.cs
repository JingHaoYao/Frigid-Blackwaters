using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthBombSkeleton : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    List<AStarNode> path;
    public GameObject deadSkele;
    public GameObject depthBomb;
    public Sprite[] viewSprites;

    //used for movement
    Vector3 randomPos;
    float travelAngle;

    //attacking
    float attackPeriod = 2;

    //choosing sprites
    int whatView, mirror;

    //etc
    GameObject playerShip;

    private float foamTimer = 0;
    public GameObject waterFoam;

    float pickSpritePeriod = 0;
    float movePeriod = 0;
    bool isAttacking = false;
    float angleToShip;
    public float angleAttack = 0;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * speed / 3f)
            {
                foamTimer = 0;
                Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
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

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
    }

    void travelLocation()
    {
        path = GetComponent<AStarPathfinding>().seekPath;
        this.GetComponent<AStarPathfinding>().target = randomPos;
        Vector3 targetPos = Vector3.zero;

        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }

        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);
        movePeriod -= Time.deltaTime;
        if (movePeriod <= 0)
        {
            movePeriod = 4;
            randomPos = pickRandPos();
        }

        if (path != null && path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f)
        {
            moveTowards(travelAngle);
            pickView(travelAngle);
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
            pickView(angleToShip);
        }

        transform.localScale = new Vector3(3.3f * mirror, 3.3f);
        if (pickSpritePeriod >= 0.2f)
        {
            pickSpritePeriod = 0;
            spriteRenderer.sprite = viewSprites[whatView - 1];
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
            mirror = -1;
        }
        else if (angle > 180 && angle <= 255)
        {
            whatView = 1;
            mirror = 1;
        }
        else if (angle > 75 && angle <= 105)
        {
            whatView = 4;
            mirror = 1;
        }
        else if (angle >= 0 && angle <= 75)
        {
            whatView = 3;
            mirror = 1;
        }
        else
        {
            whatView = 3;
            mirror = -1;
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

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerShip = GameObject.Find("PlayerShip");
        animator.enabled = false;
        randomPos = transform.position;
        movePeriod = Random.Range(2, 6);
        attackPeriod = Random.Range(4, 7);
        angleAttack = Random.Range(0, 8) * 45;
    }

    IEnumerator summonDepthBombs(float angle)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector3 spawnPosition = transform.position;
        while(Mathf.Abs(Camera.main.transform.position.x - spawnPosition.x) < 7.5f && Mathf.Abs(Camera.main.transform.position.y - spawnPosition.y) < 7.5f)
        {
            spawnPosition += directionVector * 2.5f;
        }
        spawnPosition += directionVector * -2.5f;
        while (Mathf.Abs(Camera.main.transform.position.x - spawnPosition.x) < 8f && Mathf.Abs(Camera.main.transform.position.y - spawnPosition.y) < 8f)
        {
            GameObject instant = Instantiate(depthBomb, spawnPosition, Quaternion.identity);
            instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            spawnPosition += directionVector * -2.5f;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator summonBombs(float angle)
    {
        animator.enabled = true;
        animator.SetTrigger("Attack" + whatView.ToString());
        isAttacking = true;
        yield return new WaitForSeconds(1f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(3f / 12f);
        StartCoroutine(summonDepthBombs(angleAttack));
        yield return new WaitForSeconds(8f / 12f);
        animator.enabled = false;
        isAttacking = false;
    }

    void Update()
    {
        angleToShip = (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 360f) % 360f;
        spawnFoam();
        if (isAttacking == false)
        {
            if (attackPeriod > 0)
            {
                attackPeriod -= Time.deltaTime;
                travelLocation();
            }
            else
            {
                rigidBody2D.velocity = Vector3.zero;
                attackPeriod -= Time.deltaTime;
                pickView(angleToShip);
                transform.localScale = new Vector3(3.3f * mirror, 3.3f);
                if (pickSpritePeriod >= 0.2f)
                {
                    pickSpritePeriod = 0;
                    spriteRenderer.sprite = viewSprites[whatView - 1];
                }

                if (stopAttacking == false)
                {
                    StartCoroutine(summonBombs(angleToShip));
                }
                attackPeriod = 7;
            }
        }
        pickSpritePeriod += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);

        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    public override void deathProcedure()
    {
        GameObject dead = Instantiate(deadSkele, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        this.GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }
}
