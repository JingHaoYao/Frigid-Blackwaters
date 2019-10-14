using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAlpha : Enemy
{
    public Sprite[] spriteList;
    public GameObject wave1, wave2, wave3, wave4, wave5;
    int whatView = 0;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    Animator animator;
    private bool withinRange = false;
    public float travelSpeed = 2;
    public float travelAngle;
    GameObject playerShip;
    public GameObject deadSpearman;
    private float foamTimer = 0;
    public GameObject waterFoam;
    List<AStarNode> path;
    float pickSpritePeriod = 0;
    public float relativeScale = 3.5f;
    bool attacking = false;
    public LayerMask layerMask;
    public GameObject damageBox;
    public GameObject waterFoamBurst;
    float dashPeriod = 3;
    int mirror = 1;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * travelSpeed / 3f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }
    
    void spawnWaves()
    {
        GameObject wave = Instantiate(wave1, transform.position, Quaternion.identity);
        wave.GetComponent<SerpentFinWave>().angleTravel = 270;
        wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

        wave = Instantiate(wave2, transform.position, Quaternion.identity);
        wave.GetComponent<SerpentFinWave>().angleTravel = 225;
        wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

        wave = Instantiate(wave2, transform.position, Quaternion.identity);
        wave.transform.localScale = new Vector3(-0.2f, 0.2f);
        wave.GetComponent<SerpentFinWave>().angleTravel = 315;
        wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

        wave = Instantiate(wave3, transform.position, Quaternion.identity);
        wave.GetComponent<SerpentFinWave>().angleTravel = 180;
        wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

        wave = Instantiate(wave3, transform.position, Quaternion.identity);
        wave.transform.localScale = new Vector3(-0.2f, 0.2f);
        wave.GetComponent<SerpentFinWave>().angleTravel = 0;
        wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

        wave = Instantiate(wave4, transform.position, Quaternion.identity);
        wave.GetComponent<SerpentFinWave>().angleTravel = 90;
        wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

        wave = Instantiate(wave5, transform.position, Quaternion.identity);
        wave.GetComponent<SerpentFinWave>().angleTravel = 135;
        wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;

        wave = Instantiate(wave5, transform.position, Quaternion.identity);
        wave.GetComponent<SerpentFinWave>().angleTravel = 45;
        wave.transform.localScale = new Vector3(-0.2f, 0.2f);
        wave.GetComponent<ProjectileParent>().instantiater = this.gameObject;
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

    IEnumerator waveDash(float angle)
    {
        rigidBody2D.velocity = Vector2.zero;
        pickSprite(angle);
        attacking = true;
        animator.enabled = true;
        if (mirror == 1)
        {
            damageBox.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            damageBox.transform.rotation = Quaternion.Euler(0, 0, 180 - angle);
        }
        animator.SetTrigger("Attack" + whatView);
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(5f / 12f);
        damageBox.SetActive(true);
        Instantiate(waterFoamBurst, transform.position, Quaternion.Euler(0, 0, angle + 90));
        float attackPeriod = 0;
        rigidBody2D.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 15;
        float speedMagnitude = 15;
        while (attackPeriod <= 4f / 12f)
        {
            attackPeriod += Time.deltaTime;
            speedMagnitude -= Time.deltaTime * 3;
            rigidBody2D.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speedMagnitude;
            yield return null;
        }
        rigidBody2D.velocity = Vector3.zero;
        damageBox.SetActive(false);
        spawnWaves();
        yield return new WaitForSeconds(3f / 12f);
        yield return new WaitForSeconds(3f);
        animator.enabled = false;
        attacking = false;
    }

    void pickSprite(float angle)
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
            mirror = -1;
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
        spriteRenderer.sprite = spriteList[whatView - 1];
        transform.localScale = new Vector3(mirror * relativeScale, relativeScale);
    }

    bool determineHit(Vector3 pos)
    {
        float determineAngle = 0;
        
        if(whatView == 1 || whatView == 2)
        {
            determineAngle = 90;
        }
        else
        {
            determineAngle = 270;
        }

        float angleBullet = (Mathf.Atan2(pos.y - (transform.position.y + 0.5f), pos.x - transform.position.x) * Mathf.Rad2Deg + 360) % 360;

        if (determineAngle - 75 < angleBullet && angleBullet < determineAngle + 75)
        {
            return true;
        }
        return false;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        animator.enabled = false;
        damageBox.SetActive(false);
        pickSprite(travelAngle);
    }

    void Update()
    {
        pickRendererLayer();
        path = GetComponent<AStarPathfinding>().seekPath;
        this.GetComponent<AStarPathfinding>().target = playerShip.transform.position;
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
            if (animator.enabled == false)
            {
                moveTowards(travelAngle);
                if (pickSpritePeriod >= 0.2f)
                {
                    pickSprite(travelAngle);
                    pickSpritePeriod = 0;
                }
            }
        }
        else
        {
            float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
            if (attacking == false && stopAttacking == false)
            {
                pickSprite(angleToShip);
                StartCoroutine(waveDash(angleToShip));
            }
        }

        dashPeriod -= Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (playerShip.transform.position - transform.position).normalized, 20, layerMask);

        if (hit.rigidbody.gameObject.GetComponent<PlayerScript>() && dashPeriod <= 0 && Vector2.Distance(transform.position, playerShip.transform.position) < 4)
        {
            withinRange = true;
            dashPeriod = 3;
        }
        else
        {
            withinRange = false;
        }
        spawnFoam();
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
            if (determineHit(collision.gameObject.transform.position))
            {
                this.GetComponents<AudioSource>()[0].Play();
                int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
                health -= damageDealt;
                if (health <= 0)
                {
                    GameObject deadPirate = Instantiate(deadSpearman, transform.position, Quaternion.identity);
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
}
