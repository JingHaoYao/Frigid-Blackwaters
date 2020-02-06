using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalFrostMage : Enemy {
    public Sprite facingLeft, facingDown, facingRight, facingUp;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    Animator animator;
    GameObject playerShip;
    Vector3 selectPos = new Vector3(0, 0, 0);
    private float travelAngle = 0;
    Camera camera;
    private float iceSpikePeriod = 0;
    public GameObject iceSpike, deadSkeletalMage;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    private bool nearShip = false;

    Vector3 pickRandPos()
    {
        Vector3 randPos = new Vector3(transform.position.x + Random.Range(-4.0f, 4.0f), transform.position.y + Random.Range(-4.0f, 4.0f), 0);
        while (Physics2D.OverlapCircle(randPos, .5f) || (Mathf.Abs(randPos.x - Camera.main.transform.position.x) > 8.5f || Mathf.Abs(randPos.y - Camera.main.transform.position.y) > 8.5f) || Vector2.Distance(playerShip.transform.position, randPos) < 2)
        {
            randPos = new Vector3(transform.position.x + Random.Range(-4.0f, 4.0f), transform.position.y + Random.Range(-4.0f, 4.0f), 0);
        }
        return randPos;
    }

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * rigidBody2D.velocity.magnitude/3f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    bool positionBefore(Vector3[] posArray, Vector3 randPos, int index)
    {
        for (int i = index; i >= 0; i--)
        {
            if (Vector2.Distance(randPos, posArray[i]) <= 1 || Vector2.Distance(playerShip.transform.position, randPos) < 2.5)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator spawnIceSpikes()
    {
        Vector3[] posArray = new Vector3[3];
        for(int i = 0; i < 3; i++)
        {
            Vector3 randPos = new Vector3(Mathf.Clamp(Random.Range(playerShip.transform.position.x - 2, playerShip.transform.position.x + 2), camera.transform.position.x - 7, camera.transform.position.x + 7), Mathf.Clamp(Random.Range(playerShip.transform.position.y - 2, playerShip.transform.position.y + 2), camera.transform.position.y - 7, camera.transform.position.y + 7), 0);
            while (positionBefore(posArray, randPos, i) == false)
            {
                randPos = new Vector3(Mathf.Clamp(Random.Range(playerShip.transform.position.x - 3, playerShip.transform.position.x + 3), camera.transform.position.x - 7, camera.transform.position.x + 7), Mathf.Clamp(Random.Range(playerShip.transform.position.y - 3, playerShip.transform.position.y + 3), camera.transform.position.y - 7, camera.transform.position.y + 7), 0);
            }
            GameObject instant = Instantiate(iceSpike, randPos, Quaternion.identity);
            instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            posArray[i] = randPos;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator iceSpikesAnim()
    {
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
        StartCoroutine(spawnIceSpikes());
        yield return new WaitForSeconds(5f / 12f);
        animator.enabled = false;
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

    void pickPos()
    {
        this.GetComponent<AStarPathfinding>().target = selectPos;
        path = GetComponent<AStarPathfinding>().seekPath;
        AStarNode pathNode = path[0];
        Vector3 targetPos = pathNode.nodePosition;
        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if(Vector2.Distance(playerShip.transform.position, transform.position) < 4 && nearShip == false)
        {
            selectPos = pickRandPos();
            nearShip = true;
        }

        if (Vector2.Distance(transform.position, selectPos) <= 1.5f)
        {
            nearShip = false;
            travelAngle = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            rigidBody2D.velocity = Vector3.zero;
            iceSpikePeriod += Time.deltaTime;
            if (iceSpikePeriod >= 5 && stopAttacking == false)
            {
                iceSpikePeriod = 0;
                StartCoroutine(iceSpikesAnim());
            }
            float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            pickSprite(angleToShip);
            return;
        }
        else
        {
            if (pickSpritePeriod >= 0.2f)
            {
                pickSpritePeriod = 0;
                pickSprite(travelAngle);
            }
            pickSpritePeriod += Time.deltaTime;
        }

        moveTowards(travelAngle);
    }

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
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

    void Start () {
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShip = GameObject.Find("PlayerShip");
        animator = GetComponent<Animator>();
        animator.enabled = false;
        camera = Camera.main;
        selectPos = pickRandPos();
    }

	void Update () {
        pickRendererLayer();
        pickPos();
        spawnFoam();
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
        GameObject deadPirate = Instantiate(deadSkeletalMage, transform.position, Quaternion.identity);
        deadPirate.GetComponent<DeadEnemyScript>().spriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
        deadPirate.GetComponent<DeadEnemyScript>().whatView = whatView();
        deadPirate.transform.localScale = transform.localScale;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        this.GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }
}
