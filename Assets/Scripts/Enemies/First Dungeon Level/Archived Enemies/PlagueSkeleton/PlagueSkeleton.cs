using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueSkeleton : Enemy {
    Rigidbody2D rigidBody2D;
    SpriteRenderer spriteRenderer;
    Animator animator;
    public Sprite facingLeft, facingUp, facingDown, facingRight;
    private float foamTimer = 0;
    public GameObject waterFoam, plaguePuddle;
    public float travelSpeed = 1;
    private bool spawningPuddle = false;
    private float plaguePeriod = 0;
    private float travelAngle = 0;
    GameObject playerShip;
    Camera maincamera;
    Vector3 randomPos = Vector3.zero;
    public GameObject deadPlagueSkeleton;
    List<AStarNode> path;
    float pickSpritePeriod = 0;

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

    bool isCollision(Vector3 pos)
    {
        if(Physics2D.OverlapCircle(pos, .5f) == true)
        {
            return true;
        }
        return false;
    }

    Vector3 pickRandPos()
    {
        float randX = transform.position.x + Random.Range(-5.0f, 5.0f);
        float randY = transform.position.y + Random.Range(-5.0f, 5.0f);

        Vector3 randPos = new Vector3(Mathf.Clamp(randX, maincamera.transform.position.x - 7, maincamera.transform.position.x + 7), Mathf.Clamp(randY, maincamera.transform.position.y - 8, maincamera.transform.position.y + 8), 0);
        while (Physics2D.OverlapCircle(randPos, .5f))
        {
            randX = transform.position.x + Random.Range(-5.0f, 5.0f);
            randY = transform.position.y + Random.Range(-5.0f, 5.0f);
            randPos = new Vector3(Mathf.Clamp(randX, maincamera.transform.position.x - 7, maincamera.transform.position.x + 7), Mathf.Clamp(randY, maincamera.transform.position.y - 8, maincamera.transform.position.y + 8), 0);
        }
        return randPos;
    }

    void travelLocation()
    {
        AStarNode pathNode = path[0];
        Vector3 targetPos = pathNode.nodePosition;
        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);
        pickSpritePeriod += Time.deltaTime;

        if (pickSpritePeriod >= 0.2f)
        {
            pickSprite(travelAngle);
            pickSpritePeriod = 0;
        }

        moveTowards(travelAngle);

        if (Vector2.Distance(transform.position, randomPos) < 1.5f)
        {
            randomPos = pickRandPos();
        }
    }

    IEnumerator spawnPlague()
    {
        spawningPuddle = true;
        yield return new WaitForSeconds(0.2f);
        animator.enabled = true;
        this.GetComponents<AudioSource>()[1].Play();
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
            animator.SetTrigger("Attack4");
        }
        else if (spriteRenderer.sprite == facingRight)
        {
            animator.SetTrigger("Attack3");
        }
        yield return new WaitForSeconds(9f / 12f);
        animator.enabled = false;
        spawningPuddle = false;
        Vector3 spawnPos = transform.position;
        yield return new WaitForSeconds(.5f);
        GameObject instant = Instantiate(plaguePuddle, spawnPos, Quaternion.identity);
        instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
    }

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * travelSpeed / 3f)
            {
                foamTimer = 0;
                Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
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

    void Start() {
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        path = GetComponent<AStarPathfinding>().seekPath;
        animator = GetComponent<Animator>();
        maincamera = Camera.main;
        randomPos = pickRandPos();
        animator.enabled = false;
        playerShip = GameObject.Find("PlayerShip");
	}

	void Update() {
        path = GetComponent<AStarPathfinding>().seekPath;
        this.GetComponent<AStarPathfinding>().target = randomPos;
        pickRendererLayer();
        spawnFoam();
        plaguePeriod += Time.deltaTime;
        if(plaguePeriod >= 3 && stopAttacking == false)
        {
            StartCoroutine(spawnPlague());
            plaguePeriod = 0;
        }

        if(spawningPuddle == false)
        {
            travelLocation();
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
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
            int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
            health -= damageDealt;
            this.GetComponents<AudioSource>()[0].Play();
            if (health <= 0)
            {
                GameObject deadPirate = Instantiate(deadPlagueSkeleton, transform.position, Quaternion.identity);
                deadPirate.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
                deadPirate.GetComponent<DeadEnemyScript>().whatView = whatView();
                deadPirate.transform.localScale = transform.localScale;
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
