using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterCrab : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    public GameObject deadCrab;

    //used for movement
    Vector3 randomPos;
    public int travelAngle;
    public float travelSpeed;

    //choosing sprites
    int whatView, mirror;

    public Sprite[] closedSprites;
    public Sprite[] openSprites;

    //etc
    GameObject playerShip;

    private float foamTimer = 0;
    private float invulnerableTimer;
    public GameObject waterFoam;

    float pickSpritePeriod = 0;
    public int[] cardinalAngles = new int[4] { 0, 90, 180, 270 };
    float pickTravelDuration = 2;
    public GameObject invulnerableHitBox;
    public LayerMask directionPickFilter;
    public GameObject invulnerableIcon;
    public GameObject shot;

    bool hasAttacked = false;
    public int whatType = 0;

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

    void pickNewTravelDirection()
    {
        Vector3 dir1 = new Vector3(Mathf.Cos((travelAngle + 90) * Mathf.Deg2Rad), Mathf.Sin((travelAngle + 90) * Mathf.Deg2Rad));
        Vector3 dir2 = new Vector3(Mathf.Cos((travelAngle - 90) * Mathf.Deg2Rad), Mathf.Sin((travelAngle - 90) * Mathf.Deg2Rad));

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), dir1, 20, directionPickFilter);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), dir2, 20, directionPickFilter);

        float[] hitDistances = new float[2] { hit1.distance, hit2.distance };
        float smallestDistance = Mathf.Max(hitDistances);
        int index = System.Array.IndexOf(hitDistances, smallestDistance);

        if (index == 0)
        {
            travelAngle += 90;
        }
        else
        {
            travelAngle -= 90;
        }
        travelAngle = (travelAngle + 360) % 360;
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * travelSpeed;
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

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerShip = GameObject.Find("PlayerShip");
        randomPos = transform.position;
        travelAngle = cardinalAngles[Random.Range(0, cardinalAngles.Length)];
        invulnerableTimer = Random.Range(1, 4);
    }

    IEnumerator shootBalls()
    {
        this.GetComponents<AudioSource>()[1].Play();
        travelSpeed = 0;
        if (whatType == 0)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject instant = Instantiate(shot, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);
                instant.GetComponent<AnemoneShot>().angleTravel = (i * 45) * Mathf.Deg2Rad;
                instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        else if(whatType == 1)
        {
            for(int i = 0; i < 4; i++)
            {
                float baseAngle = i * 90;
                GameObject instant = Instantiate(shot, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);
                instant.GetComponent<AnemoneShot>().angleTravel = (baseAngle + 5) * Mathf.Deg2Rad;
                instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                instant = Instantiate(shot, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);
                instant.GetComponent<AnemoneShot>().angleTravel = (baseAngle - 5) * Mathf.Deg2Rad;
                instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                float baseAngle = (i * 90) + 45;
                GameObject instant = Instantiate(shot, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);
                instant.GetComponent<AnemoneShot>().angleTravel = (baseAngle + 5) * Mathf.Deg2Rad;
                instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                instant = Instantiate(shot, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);
                instant.GetComponent<AnemoneShot>().angleTravel = (baseAngle - 5) * Mathf.Deg2Rad;
                instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        yield return new WaitForSeconds(0.5f);
        travelSpeed = 4;
    }

    void Update()
    {

        pickSpritePeriod += Time.deltaTime;
        moveTowards(travelAngle);
        pickView(travelAngle);
        spawnFoam();

        transform.localScale = new Vector3(0.3f * mirror, 0.3f);
        if (pickSpritePeriod >= 0.2f)
        {
            pickSpritePeriod = 0;
        }

        pickTravelDuration -= Time.deltaTime;
        if (pickTravelDuration <= 0 /*|| (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > 8.5f || Mathf.Abs(transform.position.y - Camera.main.transform.position.y) > 8.5f) */)
        {
            pickTravelDuration = 2;
            pickNewTravelDirection();
        }

        invulnerableTimer += Time.deltaTime;
        if (invulnerableTimer > 4 && invulnerableTimer <= 8)
        {
            invulnerableIcon.SetActive(false);
            invulnerableHitBox.SetActive(false);
            spriteRenderer.sprite = openSprites[whatView - 1];
            if(hasAttacked == false && stopAttacking == false)
            {
                StartCoroutine(shootBalls());
                hasAttacked = true;
            }
        }
        else if (invulnerableTimer > 8)
        {
            hasAttacked = false;
            invulnerableTimer = 0;
        }
        else
        {
            invulnerableIcon.SetActive(true);
            invulnerableHitBox.SetActive(true);
            spriteRenderer.sprite = closedSprites[whatView - 1];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0 && invulnerableHitBox.activeSelf == false)
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
        else if (collision.tag != "EnemyShield")
        {
            pickTravelDuration = 2;
            pickNewTravelDirection();
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
