using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossSwordPhase : MonoBehaviour {
    Animator animator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCol;

    public int health = 15;
    GameObject playerShip;

    float attackDuration = 1f;

    List<Vector3> posArray = new List<Vector3>();

    Vector3 middlePos = new Vector3(-800, 22.5f, 0);
    Vector3 topPos = new Vector3(-800, 28.5f, 0);

    float speed = 4f;

    public GameObject summon, aStarGrid, swordSlashEffect;
    GameObject instantiatedAStarGrid;

    bool landed = false;
    bool dead = false;
    bool entryAnimPlayed = false;

    public GameObject[] doorSeals;
    public GameObject instaKill;
    int numberKillLength = 0;
    bool movedToTopPos;

    int numberSlashes = 0;
    float angleToShip = 0;
    bool isSwiping = false;
    float directionIndex = 0;

    public LayerMask filter;

    public GameObject guidanceArrow;

    IEnumerator summonSkeles()
    {
        attackDuration = 18f;
        isSwiping = true;
        for (int k = 0; k < 3; k++)
        {
            if (dead == false)
            {
                animator.SetTrigger("Summon");
                this.GetComponents<AudioSource>()[4].Play();
                yield return new WaitForSeconds(5f / 8f);
                for (int i = 0; i < 2; i++)
                {
                    Vector3 randPos = new Vector3(Random.Range(-9f, 9f), Random.Range(-9f, 9f), 0) + new Vector3(-800, 20, 0);
                    while (Physics2D.OverlapCircle(randPos, 0.7f))
                    {
                        randPos = new Vector3(Random.Range(-9f, 9f), Random.Range(-9f, 9f), 0) + new Vector3(-800, 20, 0);
                    }
                    Instantiate(summon, randPos, Quaternion.identity);
                }
                yield return new WaitForSeconds(2f / 8f);
            }
        }
        directionIndex = 0;
        pickAnim(angleToShip, false, false);
        isSwiping = false;
    }

    bool isPositionPicked(List<Vector3> posArray, Vector3 pos)
    {
        for (int i = 0; i < posArray.Count; i++)
        {
            if (Vector2.Distance(posArray[i], pos) < 0.6f || Physics2D.OverlapCircle(pos, 0.5f))
            {
                return true;
            }
        }
        return false;
    }

    void pickAnim(float direction, bool pointing, bool backing)
    {
        if (dead == false)
        {
            if (pointing || backing)
            {
                directionIndex = 0;
            }

            if (direction > 75 && direction < 105)
            {
                if (directionIndex != 1)
                {
                    directionIndex = 1;
                    if (pointing || backing)
                    {
                        if (pointing)
                        {
                            animator.SetTrigger("Point4");
                        }

                        if (backing)
                        {
                            animator.SetTrigger("Back4");
                        }
                    }
                    else
                    {
                        animator.SetTrigger("Idle4");
                    }
                    transform.localScale = new Vector3(0.2f, 0.2f, 0);
                }
            }
            else if (direction < 285 && direction > 265)
            {
                if (directionIndex != 2)
                {
                    directionIndex = 2;
                    if (pointing || backing)
                    {
                        if (pointing)
                        {
                            animator.SetTrigger("Point1");
                        }

                        if (backing)
                        {
                            animator.SetTrigger("Back1");
                        }
                    }
                    else
                    {
                        animator.SetTrigger("Idle1");
                    }
                    transform.localScale = new Vector3(0.2f, 0.2f, 0);
                }
            }
            else if (direction >= 285 && direction <= 360)
            {
                if (directionIndex != 3)
                {
                    directionIndex = 3;

                    if (pointing || backing)
                    {
                        if (pointing)
                        {
                            animator.SetTrigger("Point2");
                        }

                        if (backing)
                        {
                            animator.SetTrigger("Back2");
                        }
                    }
                    else
                    {
                        animator.SetTrigger("Idle2");
                    }
                    transform.localScale = new Vector3(0.2f, 0.2f, 0);
                }
            }
            else if (direction >= 180 && direction <= 265)
            {
                if (directionIndex != 4)
                {
                    directionIndex = 4;
                    if (pointing || backing)
                    {
                        if (pointing)
                        {
                            animator.SetTrigger("Point2");
                        }

                        if (backing)
                        {
                            animator.SetTrigger("Back2");
                        }
                    }
                    else
                    {
                        animator.SetTrigger("Idle2");
                    }
                    transform.localScale = new Vector3(-0.2f, 0.2f, 0);
                }
            }
            else if (direction < 180 && direction >= 105)
            {
                if (directionIndex != 5)
                {
                    directionIndex = 5;
                    if (pointing || backing)
                    {
                        if (pointing)
                        {
                            animator.SetTrigger("Point3");
                        }

                        if (backing)
                        {
                            animator.SetTrigger("Back3");
                        }
                    }
                    else
                    {
                        animator.SetTrigger("Idle3");
                    }
                    transform.localScale = new Vector3(-0.2f, 0.2f, 0);
                }
            }
            else
            {
                if (directionIndex != 6)
                {
                    directionIndex = 6;
                    if (pointing || backing)
                    {
                        if (pointing)
                        {
                            animator.SetTrigger("Point3");
                        }

                        if (backing)
                        {
                            animator.SetTrigger("Back3");
                        }
                    }
                    else
                    {
                        animator.SetTrigger("Idle3");
                    }
                    transform.localScale = new Vector3(0.2f, 0.2f, 0);
                }
            }
        }
    }

    IEnumerator slashAttack(float direction)
    {
        isSwiping = true;
        attackDuration = 3;
        Vector3 directionVector = Vector3.Normalize(new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad)));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionVector, Mathf.Infinity, filter);
        Vector3 target = hit.point;
        pickAnim(direction, true, false);
        this.GetComponents<AudioSource>()[3].Play();
        GameObject slash = Instantiate(swordSlashEffect, new Vector3((hit.point.x + transform.position.x) / 2f, (hit.point.y + transform.position.y) / 2f, 0), Quaternion.identity);
        slash.GetComponentInChildren<FirstBossSwordSlash>().angleAttack = direction;
        yield return new WaitForSeconds(.66f);
        transform.position = new Vector3(Mathf.Clamp(target.x, -7.5f - 800, 7.5f - 800), Mathf.Clamp(target.y, 15, 26f));
        yield return new WaitForSeconds(0.5f);
        pickAnim(direction, false, true);
        yield return new WaitForSeconds(0.5f / 0.667f);
        isSwiping = false;
    }

    void Start()
    {
        Camera.main.GetComponent<CameraShake>().shakeCamFunction(3f, 0.3f);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
        playerShip = GameObject.Find("PlayerShip");
        instantiatedAStarGrid = Instantiate(aStarGrid, new Vector3(-800, 20, 0), Quaternion.identity);
        for (int i = 0; i < doorSeals.Length; i++)
        {
            doorSeals[i].SetActive(true);
        }
        boxCol.enabled = false;
    }

    void Update()
    {
        angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y,
                       playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        if (landed == true)
        {
            if (dead == false)
            {
                if (movedToTopPos == false)
                {
                    pickAnim(angleToShip, false, false);
                    speed = Vector2.Distance(transform.position, topPos) * 4;
                    transform.position += Vector3.up * Time.deltaTime * speed;
                    if(Vector2.Distance(topPos, transform.position) < 0.1f)
                    {
                        transform.position = topPos;
                        movedToTopPos = true;
                    }
                }
                else
                {
                    if (attackDuration > 0)
                    {
                        if(isSwiping == false)
                        {
                            pickAnim(angleToShip, false, false);
                        }
                        attackDuration -= Time.deltaTime;
                    }
                    else
                    {
                        if (numberSlashes < 5)
                        {
                            numberSlashes++;
                            StartCoroutine(slashAttack(angleToShip));
                        }
                        else
                        {
                            StartCoroutine(summonSkeles());
                            numberSlashes = 0;
                        }
                    }
                }
            }
            else
            {
                transform.position += Vector3.up * Time.deltaTime * 5;
                if (transform.position.y > 38 || Camera.main.transform.position.y > 30)
                {
                    Instantiate(guidanceArrow, new Vector3(0, 25.6f, 0) + new Vector3(-800, 0, 0), Quaternion.Euler(0, 0, 90));
                    Destroy(instantiatedAStarGrid);
                    Destroy(this.gameObject);
                }

                GameObject[] swordMen = GameObject.FindGameObjectsWithTag("MeleeEnemy");
                if (numberKillLength != swordMen.Length)
                {
                    numberKillLength = swordMen.Length;
                    foreach (GameObject skele in swordMen)
                    {
                        Instantiate(instaKill, skele.transform.position, Quaternion.identity);
                    }
                }
            }
        }
        else
        {
            if (entryAnimPlayed == false)
            {
                speed = Vector2.Distance(transform.position, middlePos);
                transform.position += Vector3.down * Time.deltaTime * speed;
                if (Vector2.Distance(transform.position, middlePos) < 0.2f)
                {
                    entryAnimPlayed = true;
                    transform.position = middlePos;
                    this.GetComponents<AudioSource>()[2].Stop();
                    StartCoroutine(introAnim());
                }
            }
        }
    }

    IEnumerator introAnim()
    {
        animator.SetTrigger("Entry");
        this.GetComponents<AudioSource>()[5].Play();
        yield return new WaitForSeconds(1.083f / 0.667f);
        landed = true;
        boxCol.enabled = true;
    }

    IEnumerator fadeOut(AudioSource source, float speed)
    {
        while (source.volume > 0)
        {
            source.volume -= speed;
            yield return new WaitForSeconds(0.1f);
        }
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
                this.GetComponents<AudioSource>()[1].Play();
                StartCoroutine(fadeOut(this.GetComponents<AudioSource>()[1], 0.05f));
                StartCoroutine(hitFrame());
                this.gameObject.GetComponent<Collider2D>().enabled = false;
                animator.SetTrigger("Death");
                GameObject[] swordMen = GameObject.FindGameObjectsWithTag("MeleeEnemy");
                foreach (GameObject skele in swordMen)
                {
                    Instantiate(instaKill, skele.transform.position, Quaternion.identity);
                }
                for (int i = 0; i < doorSeals.Length; i++)
                {
                    doorSeals[i].GetComponent<DoorSeal>().open = true;
                }
                dead = true;
                Camera.main.GetComponent<CameraShake>().shakeCamFunction(3f, 0.3f);
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
