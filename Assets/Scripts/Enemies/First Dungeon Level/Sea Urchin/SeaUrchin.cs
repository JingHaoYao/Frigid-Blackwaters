using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaUrchin : Enemy {
    Animator animator;
    Rigidbody2D rigidBody2D;
    public GameObject urchinShot;
    public GameObject deadStarFish;
    int whatSide = 0;
    Vector3[] cornerList;
    int cw = 0;
    public GameObject enemyMarker;
    SpriteRenderer spriteRenderer;
    public int whatUrchinType = 1;
    Camera mainCamera;

    float attackPeriod = 3;

    void setLocation()
    {
        whatSide = Random.Range(0, 4);
        if (whatSide == 0)
        {
            transform.parent.gameObject.transform.position = cornerList[0] + new Vector3(0, Random.Range(2f, 8f));
        }
        else if (whatSide == 1)
        {
            transform.parent.gameObject.transform.position = cornerList[1] + new Vector3(Random.Range(-8, -2f), 0);
        }
        else if (whatSide == 2)
        {
            transform.parent.gameObject.transform.position = cornerList[2] + new Vector3(0, Random.Range(-8f, -2f));
        }
        else
        {
            transform.parent.gameObject.transform.position = cornerList[3] + new Vector3(Random.Range(2, 8), 0);
        }
    }

    void moveAlongWall()
    {
        if (cw == 0)
        {
            if (whatSide == 0)
            {
                rigidBody2D.velocity = Vector3.down * speed;
                if (Vector2.Distance(transform.position, cornerList[whatSide]) < 0.5f)
                {
                    transform.position = cornerList[whatSide];
                    whatSide++;
                    if (whatSide > 3)
                    {
                        whatSide = 0;
                    }
                }
            }
            else if (whatSide == 1)
            {
                rigidBody2D.velocity = Vector3.right * speed;
                if (Vector2.Distance(transform.position, cornerList[whatSide]) < 0.5f)
                {
                    transform.position = cornerList[whatSide];
                    whatSide++;
                    if (whatSide > 3)
                    {
                        whatSide = 0;
                    }
                }
            }
            else if (whatSide == 2)
            {
                rigidBody2D.velocity = Vector3.up * speed;
                if (Vector2.Distance(transform.position, cornerList[whatSide]) < 0.5f)
                {
                    transform.position = cornerList[whatSide];
                    whatSide++;
                    if (whatSide > 3)
                    {
                        whatSide = 0;
                    }
                }
            }
            else
            {
                rigidBody2D.velocity = Vector3.left * speed;
                if (Vector2.Distance(transform.position, cornerList[whatSide]) < 0.5f)
                {
                    transform.position = cornerList[whatSide];
                    whatSide++;
                    if (whatSide > 3)
                    {
                        whatSide = 0;
                    }
                }
            }
        }
        else
        {
            if (whatSide == 0)
            {
                rigidBody2D.velocity = Vector3.up * speed;
                if (Vector2.Distance(transform.position, cornerList[3]) < 0.5f)
                {
                    transform.position = cornerList[3];
                    whatSide--;
                    if (whatSide < 0)
                    {
                        whatSide = 3;
                    }
                }
            }
            else if (whatSide == 1)
            {
                rigidBody2D.velocity = Vector3.left * speed;
                if (Vector2.Distance(transform.position, cornerList[0]) < 0.5f)
                {
                    transform.position = cornerList[0];
                    whatSide--;
                    if (whatSide < 0)
                    {
                        whatSide = 3;
                    }
                }
            }
            else if (whatSide == 2)
            {
                rigidBody2D.velocity = Vector3.down * speed;
                if (Vector2.Distance(transform.position, cornerList[1]) < 0.5f)
                {
                    transform.position = cornerList[1];
                    whatSide--;
                    if (whatSide < 0)
                    {
                        whatSide = 3;
                    }
                }
            }
            else
            {
                rigidBody2D.velocity = Vector3.right * speed;
                if (Vector2.Distance(transform.position, cornerList[2]) < 0.5f)
                {
                    transform.position = cornerList[2];
                    whatSide--;
                    if (whatSide < 0)
                    {
                        whatSide = 3;
                    }
                }
            }
        }
    }

    IEnumerator shootShot()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(3f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        if (whatUrchinType == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject shot = Instantiate(urchinShot, transform.position + new Vector3(Mathf.Cos(whatSide * 90 * Mathf.Deg2Rad), Mathf.Sin(whatSide * 90 * Mathf.Deg2Rad)).normalized, Quaternion.Euler(0, 0, (whatSide * 90 + (-12.5f + 25 * i))));
                shot.GetComponent<SeaUrchinSpike>().angleTravel = (whatSide * 90 + (-12.5f + 25 * i)) * Mathf.Deg2Rad;
                shot.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject shot = Instantiate(urchinShot, transform.position + new Vector3(Mathf.Cos(whatSide * 90 * Mathf.Deg2Rad), Mathf.Sin(whatSide * 90 * Mathf.Deg2Rad)).normalized, Quaternion.Euler(0, 0, (whatSide * 90 + (-25 + 25 * i))));
                shot.GetComponent<SeaUrchinSpike>().angleTravel = (whatSide * 90 + (-25 + 25 * i)) * Mathf.Deg2Rad;
                shot.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }
        yield return new WaitForSeconds(3f / 12f);
        animator.SetTrigger("Idle");
    }

    void Start()
    {
        mainCamera = Camera.main;
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cornerList = new Vector3[] { mainCamera.transform.position + new Vector3(-9f, -9f), mainCamera.transform.position + new Vector3(9f, -9f), mainCamera.transform.position + new Vector3(9f, 9f), mainCamera.transform.position + new Vector3(-9f, 9f) };
        setLocation();
        cw = Random.Range(0, 2);
        updateSpeed(4);
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, whatSide * 90);
        enemyMarker.transform.position = transform.position + new Vector3(0, 1, 0);
        moveAlongWall();

        if (attackPeriod > 0)
        {
            attackPeriod -= Time.deltaTime;
        }
        else
        {
            StartCoroutine(shootShot());
            attackPeriod = 2;
        }

        if (Vector2.Distance(transform.position, mainCamera.transform.position) < 8 || Vector2.Distance(transform.position, mainCamera.transform.position) > 14.5f)
        {
            destroyProcedure();
        }

        updateSpeed((Mathf.Sin(Time.time * 3) + 1) * 3);
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
        GameObject deadPirate = Instantiate(deadStarFish, transform.position, Quaternion.identity);
        deadPirate.transform.rotation = transform.rotation;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(transform.parent.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        this.GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }
}
