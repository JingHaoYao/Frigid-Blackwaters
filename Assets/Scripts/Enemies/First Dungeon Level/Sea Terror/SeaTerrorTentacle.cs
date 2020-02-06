using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaTerrorTentacle : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    public bool slamTentacle = false;
    public GameObject slamHitBox;
    public GameObject swingHitBox;
    bool isAttacking = false;
    PlayerScript playerScript;
    float aliveDuration = 0;
    Rigidbody2D rigidBody2D;
    SeaTerror seaTerror;

    public GameObject waterFoam;
    float foamTimer = 0;

    void spawnFoam(Vector3 velocity)
    {
        if (velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * 2 / 3f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerScript = FindObjectOfType<PlayerScript>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        seaTerror = FindObjectOfType<SeaTerror>();
        updateSpeed(2);
    }

    void Update()
    {
        if (aliveDuration < 6)
        {
            aliveDuration += Time.deltaTime;

            if (health > 0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Sea Terror Tentacle Rise"))
            {
                if (slamTentacle == false)
                {
                    if (Vector2.Distance(playerScript.transform.position, transform.position) > 0.8f)
                    {
                        rigidBody2D.velocity = (playerScript.transform.position - transform.position).normalized * speed;
                        spawnFoam(rigidBody2D.velocity);
                    }

                    if (isAttacking == false && Vector2.Distance(transform.position, playerScript.transform.position) < 1.6f)
                    {
                        isAttacking = true;
                        StartCoroutine(swing());
                    }
                }
                else
                {
                    if (isAttacking == false)
                    {
                        StartCoroutine(slam());
                        isAttacking = true;
                    }
                }
            }
        }
        else
        {
            if (aliveDuration != 256)
            {
                sinkTentacle();
                aliveDuration = 256;
            }
        }

        if(seaTerror.health <= 0 && aliveDuration != 256)
        {
            sinkTentacle();
            aliveDuration = 256;
        }
    }

    IEnumerator slam()
    {
        animator.SetTrigger("Slam");
        yield return new WaitForSeconds(5f / 12f);
        GetComponents<AudioSource>()[2].Play();
        slamHitBox.SetActive(true);
        yield return new WaitForSeconds(2f / 12f);
        slamHitBox.SetActive(false);
        yield return new WaitForSeconds(3f / 12f);
        sinkTentacle();
    }

    IEnumerator swing()
    {
        animator.SetTrigger("Swipe");
        yield return new WaitForSeconds(3f / 12f);
        GetComponents<AudioSource>()[1].Play();
        swingHitBox.SetActive(true);
        yield return new WaitForSeconds(2f / 12f);
        swingHitBox.SetActive(false);
        yield return new WaitForSeconds(5f / 12f);
        isAttacking = false;
    }

    public void sinkTentacle()
    {
        animator.SetTrigger("Sink");
        GetComponents<AudioSource>()[3].Play();
        Destroy(gameObject, 1f);
        seaTerror.tentacleList.Remove(this);
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
            this.GetComponents<AudioSource>()[0].Play();
            StartCoroutine(hitFrame());
        }
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        slamHitBox.SetActive(false);
        swingHitBox.SetActive(false);
        sinkTentacle();
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public override void damageProcedure(int damage)
    {

    }
}
