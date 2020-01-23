using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlailGolem : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    PlayerScript playerScript;
    Rigidbody2D rigidBody2D;

    int whatView = 0;

    public BossManager bossManager;
    public GameObject flail;
    float throwPeriod = 0;
    public float flailRadius = 4;
    float flailPeriod = 0;
    bool isThrowing = false;
    float angleToShip = 0;
    Vector3 targetTravel;
    float flailSpeed = 12;

    public GameObject deadFlailGolem;

    int pickView(float angle)
    {
        if (angle > 255 && angle <= 285)
        {
            return 1;
        }
        else if (angle > 285 && angle <= 360)
        {
            return 2;
        }
        else if (angle > 180 && angle <= 255)
        {
            return 6;
        }
        else if (angle > 75 && angle <= 105)
        {
            return 4;
        }
        else if (angle > 0 && angle <= 75)
        {
            return 3;
        }
        else
        {
            return 5;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        playerScript = FindObjectOfType<PlayerScript>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        angleToShip = (360 + Mathf.Atan2(playerScript.transform.position.y - transform.position.y, playerScript.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        targetTravel = playerScript.transform.position + new Vector3(Mathf.Cos((angleToShip + 180) * Mathf.Deg2Rad), Mathf.Sin((angleToShip + 180) * Mathf.Deg2Rad)) * flailRadius;
        flail.SetActive(true);
        FindObjectOfType<BossHealthBar>().bossStartUp("Tome Guardian");
        FindObjectOfType<BossHealthBar>().targetEnemy = this;
    }

    IEnumerator throwFlail()
    {
        rigidBody2D.velocity = Vector3.zero;
        isThrowing = true;
        animator.SetTrigger("Throw" + pickView(angleToShip).ToString());

        
        while(flailSpeed < 16)
        {
            flailSpeed += 1;
            flail.GetComponent<Rigidbody2D>().velocity = (playerScript.transform.position - flail.transform.position).normalized * flailSpeed;
            yield return new WaitForSeconds(0.05f);
        }

        GetComponents<AudioSource>()[2].Play();
        yield return new WaitForSeconds(7f / 12f);
        
        while(Mathf.Abs(flail.transform.position.y - Camera.main.transform.position.y) < 7.5f && Mathf.Abs(flail.transform.position.x - Camera.main.transform.position.x) < 7.5f)
        {
            yield return null;
        }

        animator.SetTrigger("PullBack");

        while (flailSpeed > 12)
        {
            flailSpeed -= 1;
            flail.GetComponent<Rigidbody2D>().velocity = ((transform.position + new Vector3(Mathf.Cos(flailPeriod), Mathf.Sin(flailPeriod)) * flailRadius + Vector3.up * 1.5f) - flail.transform.position).normalized * flailSpeed;
            yield return new WaitForSeconds(0.05f);
        }

        while(Vector2.Distance(flail.transform.position, (transform.position + new Vector3(Mathf.Cos(flailPeriod), Mathf.Sin(flailPeriod)) * flailRadius + Vector3.up * 1.5f)) > 0.5f)
        {
            yield return null;
        }

        animator.SetTrigger("Swing" + pickView(angleToShip).ToString());

        isThrowing = false;
    }



    void Update()
    {
        angleToShip = (360 + Mathf.Atan2(playerScript.transform.position.y - transform.position.y, playerScript.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        if(health > 0)
        {
            throwPeriod += Time.deltaTime;
            if (throwPeriod > 8)
            {
                throwPeriod = 0;
                if (GetComponents<AudioSource>()[1].isPlaying == true)
                {
                    GetComponents<AudioSource>()[1].Stop();
                }
                StartCoroutine(throwFlail());
            }
            else
            {
                if (isThrowing == false)
                {
                    flailPeriod += Time.deltaTime * 2;
                    if (flailPeriod > Mathf.PI * 2)
                    {
                        flailPeriod = 0;
                    }

                    if (pickView(angleToShip) != whatView)
                    {
                        animator.SetTrigger("Swing" + pickView(angleToShip).ToString());
                        whatView = pickView(angleToShip);
                    }

                    flail.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    flail.transform.position = transform.position + new Vector3(Mathf.Cos(flailPeriod), Mathf.Sin(flailPeriod)) * flailRadius + Vector3.up * 1.5f;
                    if (GetComponents<AudioSource>()[1].isPlaying == false)
                    {
                        GetComponents<AudioSource>()[1].Play();
                    }

                    if (Vector2.Distance(transform.position, targetTravel) > 0.2f)
                    {
                        rigidBody2D.velocity = new Vector3(targetTravel.x - transform.position.x, targetTravel.y - transform.position.y).normalized * 2.5f;
                    }
                    else
                    {
                        rigidBody2D.velocity = Vector3.zero;
                        targetTravel = playerScript.transform.position + new Vector3(Mathf.Cos((angleToShip + 180) * Mathf.Deg2Rad), Mathf.Sin((angleToShip + 180) * Mathf.Deg2Rad)) * flailRadius;
                    }
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            this.GetComponents<AudioSource>()[0].Play();
            if (health <= 0)
            {
                rigidBody2D.velocity = Vector3.zero;
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                playerScript.enemiesDefeated = true;
                SaveSystem.SaveGame();
                bossManager.bossBeaten(nameID, 1.083f);
                addKills();
                Instantiate(deadFlailGolem, transform.position, Quaternion.identity);
                flail.GetComponent<Animator>().enabled = true;
                flail.GetComponent<CircleCollider2D>().enabled = false;
                StopAllCoroutines();
                flail.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                Destroy(this.gameObject);
                FindObjectOfType<BossHealthBar>().bossEnd();
            }
            else
            {
                StartCoroutine(hitFrame());
            }
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
