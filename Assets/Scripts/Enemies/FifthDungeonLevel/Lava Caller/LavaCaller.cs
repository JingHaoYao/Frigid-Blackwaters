using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCaller : Enemy
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D rigidBody2D;

    //used for movement
    private int travelAngle;

    //etc
    GameObject playerShip;

    private float foamTimer = 0;
    public GameObject waterFoam;

    float pickSpritePeriod = 0;
    public int[] cardinalAngles = new int[4] { 0, 90, 180, 270 };
    float pickTravelDuration = 2;
    public LayerMask directionPickFilter;

    private float attackPeriod = 0;

    [SerializeField] AudioSource damageAudio;
    [SerializeField] GameObject lavaCallerHead;
    LavaCallerHead headInstant;

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
        GameObject headInstantObject = Instantiate(lavaCallerHead, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        headInstant = headInstantObject.GetComponent<LavaCallerHead>();
        headInstant.Initialize(this.gameObject);
        travelAngle = cardinalAngles[Random.Range(0, cardinalAngles.Length)];
    }

    void Update()
    {

        pickSpritePeriod += Time.deltaTime;
        spawnFoam();

        if (pickSpritePeriod >= 0.2f)
        {
            pickSpritePeriod = 0;
        }

        pickTravelDuration -= Time.deltaTime;
        if (pickTravelDuration <= 0)
        {
            pickTravelDuration = 2;
            pickNewTravelDirection();
        }

        if (!headInstant.IsAttacking)
        {
            moveTowards(travelAngle);
            attackPeriod += Time.deltaTime;
            if (attackPeriod > 1.5f)
            {
                headInstant.StartLavaPull(3);
                attackPeriod = 0;
            }
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
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
        StopAllCoroutines();
        headInstant.Death();
        animator.SetTrigger("Death");
        Destroy(this.gameObject, 9 / 12f);
    }

    public override void damageProcedure(int damage)
    {
        damageAudio.Play();
        headInstant.DamageHit();
        StartCoroutine(hitFrame());
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
