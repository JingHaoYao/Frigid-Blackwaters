using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalNecromancer : Enemy {
    public Sprite facingUp, facingDown, facingLeft, facingRight;
    public GameObject necroMancerSummon, deadNecromancer;
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidBody2D;
    GameObject playerShip;
    Camera camera;
    private float travelAngle;
    public float numSkeles = 3;
    Vector3 newPos;
    private bool isSummoning = false;
    private int countUntilSummon = 0;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;

    Vector3 pickRandPos()
    {
        Vector3 randPos = new Vector3(camera.transform.position.x + Random.Range(-7.0f, 7.0f), camera.transform.position.y + Random.Range(-7.0f, 7.0f), 0);
        while (Physics2D.OverlapCircle(randPos, .5f))
        {
            randPos = new Vector3(camera.transform.position.x + Random.Range(-7.0f, 7.0f), camera.transform.position.y + Random.Range(-7.0f, 7.0f), 0);
        }
        return randPos;
    }

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

    IEnumerator animSummonSkele()
    {
        isSummoning = true;
        animator.enabled = true;
        float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        pickSprite(angleToShip);
        this.GetComponents<AudioSource>()[1].Play();
        if (spriteRenderer.sprite == facingUp)
        {
            animator.SetTrigger("Summon3");
        }
        else if (spriteRenderer.sprite == facingDown)
        {
            animator.SetTrigger("Summon2");
        }
        else if (spriteRenderer.sprite == facingLeft)
        {
            animator.SetTrigger("Summon1");
        }
        else
        {
            animator.SetTrigger("Summon4");
        }
        yield return new WaitForSeconds(7f / 12f);
        summonSkele();
        yield return new WaitForSeconds(5f / 12f);
        animator.enabled = false;
        isSummoning = false;
    }

    void summonSkele()
    {
        float angleToShip = Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        for(int i = 0; i < numSkeles; i++)
        {
            Instantiate(necroMancerSummon, transform.position + new Vector3(Mathf.Cos((angleToShip - 30f + (60f/(numSkeles - 1))*i) * Mathf.Deg2Rad), Mathf.Sin((angleToShip - 30f + (60f / (numSkeles - 1)) * i) * Mathf.Deg2Rad), 0) * 2.5f, Quaternion.identity);
        }
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
    }

    void travelLocation()
    {
        if (isSummoning == false)
        {
            this.GetComponent<AStarPathfinding>().target = newPos;
            path = GetComponent<AStarPathfinding>().seekPath;
            AStarNode pathNode = path[0];
            Vector3 targetPos = pathNode.nodePosition;
            travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

            if (Vector2.Distance(transform.position, newPos) <= 1.5f)
            {
                countUntilSummon++;
                if (countUntilSummon >= 3 && stopAttacking == false)
                {
                    StartCoroutine(animSummonSkele());
                    countUntilSummon = 0;
                }
                newPos = pickRandPos();
            }
            pickSpritePeriod += Time.deltaTime;
            if (pickSpritePeriod >= 0.2f)
            {
                pickSprite(travelAngle);
                pickSpritePeriod = 0;
            }
            moveTowards(travelAngle);
        }
        else
        {   
            rigidBody2D.velocity = Vector3.zero;
        }
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

    void pickRendererLayer()
    {
        /*if (transform.position.y < playerShip.transform.position.y)
        {
            spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
        else
        {
            spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }*/
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void Start () {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.enabled = false;
        camera = Camera.main;
        playerShip = GameObject.Find("PlayerShip");
        rigidBody2D = GetComponent<Rigidbody2D>();
        newPos = pickRandPos();
    }

	void Update () {
        pickRendererLayer();
        travelLocation();
        spawnFoam();
	}

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
    }

    public override void deathProcedure()
    {
        GameObject deadNecro = Instantiate(deadNecromancer, transform.position, Quaternion.identity);
        deadNecro.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
        deadNecro.GetComponent<DeadEnemyScript>().whatView = whatView();
        deadNecro.transform.localScale = transform.localScale;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        this.GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }
}
