using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalBoltMage : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    List<AStarNode> path;
    public GameObject deadSkele;
    public GameObject bolt;
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
    public float boltOffset = 0;

    AStarPathfinding aStarPathfinding;

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
        path = aStarPathfinding.seekPath;
        this.aStarPathfinding.target = randomPos;

        Vector3 targetPos = randomPos;
        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }
        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);
        moveTowards(travelAngle);

        pickView(travelAngle);
        transform.localScale = new Vector3(3.3f * mirror, 3.3f);
        if (pickSpritePeriod >= 0.2f)
        {
            pickSpritePeriod = 0;
            spriteRenderer.sprite = viewSprites[whatView - 1];
        }

        if (Vector2.Distance(transform.position, randomPos) < 1.5f)
        {
            attackPeriod = 2.5f;
            //attack
            if (stopAttacking == false)
            {
                StartCoroutine(summonBolts());
            }
            randomPos = pickRandPos();
        }
    }

    IEnumerator summonBolts()
    {
        animator.enabled = true;
        animator.SetTrigger("Attack" + whatView.ToString());
        yield return new WaitForSeconds(4f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(2f / 12f);
        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90 + boltOffset;
            GameObject spawnedBolt = Instantiate(bolt, transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)), Quaternion.Euler(0, 0, angle));
            spawnedBolt.GetComponent<SkeletalBoltMageBolt>().angleTravel = angle * Mathf.Deg2Rad;
            spawnedBolt.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        yield return new WaitForSeconds(6f / 12f);
        animator.enabled = false;
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
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerShip = GameObject.Find("PlayerShip");
        animator.enabled = false;
        randomPos = pickRandPos();
        aStarPathfinding = GetComponent<AStarPathfinding>();
    }

    void Update()
    {
        float angleToShip = (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 360f) % 360f;
        spawnFoam();
        if (attackPeriod <= 0)
        {
            attackPeriod = 0;
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

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
