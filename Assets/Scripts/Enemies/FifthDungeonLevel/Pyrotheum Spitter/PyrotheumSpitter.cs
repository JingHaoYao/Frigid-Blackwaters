using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyrotheumSpitter : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    public GameObject deadCrab;

    //used for movement
    private int travelAngle;

    //etc
    GameObject playerShip;

    private float foamTimer = 0;
    public GameObject waterFoam;

    public int[] cardinalAngles = new int[4] { 0, 90, 180, 270 };
    float pickTravelDuration = 2;
    public LayerMask directionPickFilter;

    private float attackPeriod = 0;

    bool hasAttacked = false;

    private float currentOffset = 0;

    private bool isAttacking = false;

    [SerializeField] GameObject pyrotheumShot;
    [SerializeField] AudioSource attackAudio, damageAudio;

    IEnumerator shootPyrotheumShots()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        attackAudio.Play();
        yield return new WaitForSeconds(4 / 12f);
        for (int i = 0; i < 3; i++)
        {
            float angle = (i * 120 + currentOffset) * Mathf.Deg2Rad;
            GameObject projectileInstant = Instantiate(pyrotheumShot, transform.position + Vector3.up, Quaternion.identity);
            projectileInstant.GetComponent<PyrotheumProjectile>().angleTravel = angle * Mathf.Rad2Deg;
            projectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        yield return new WaitForSeconds(6 / 12f);
        animator.SetTrigger("Idle");
        isAttacking = false;
        currentOffset += 30;
        attackPeriod = 0;
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
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerShip = GameObject.Find("PlayerShip");
        travelAngle = cardinalAngles[Random.Range(0, cardinalAngles.Length)];
        currentOffset = Random.Range(0, 3) * 30;
    }

    void Update()
    {

        moveTowards(travelAngle);
        spawnFoam();

        pickTravelDuration -= Time.deltaTime;
        if (pickTravelDuration <= 0)
        {
            pickTravelDuration = 2;
            pickNewTravelDirection();
        }

        if (!isAttacking)
        {
            attackPeriod += Time.deltaTime;
            if(attackPeriod > 0.5f)
            {
                StartCoroutine(shootPyrotheumShots());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
        else if (collision.tag != "EnemyShield")
        {
            pickTravelDuration = 2;
            pickNewTravelDirection();
        }
    }

    public override void deathProcedure()
    {
        GameObject dead = Instantiate(deadCrab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        damageAudio.Play();
        StartCoroutine(hitFrame());
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
