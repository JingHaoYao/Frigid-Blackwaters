using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossSpearPhase : MonoBehaviour {
    Animator animator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCol;

    public int health = 15;
    public GameObject[] spreadSpears;
    public GameObject spearIndicator;
    GameObject playerShip;

    float attackDuration = 1f;

    List<Vector3> posArray = new List<Vector3>();

    Vector3 middlePos = new Vector3(-800, 2.5f, 0);
    Vector3 topPos = new Vector3(-800, 8f, 0);

    bool spreadAttacking = false;
    float speed = 4f;

    public GameObject summon, aStarGrid;
    GameObject instantiatedAStarGrid;

    bool attacking = false;
    bool landed = false;
    bool dead = false;

    public GameObject[] doorSeals;
    public GameObject instaKill;
    int numberKillLength = 0;

    public GameObject guidanceArrow;

    IEnumerator summonSkeles()
    {
        attackDuration = 18f;
        for (int k = 0; k < 3; k++)
        {
            if (dead == false)
            {
                animator.SetTrigger("Summon");
                this.GetComponents<AudioSource>()[3].Play();
                yield return new WaitForSeconds(5f / 8f);
                for (int i = 0; i < 2; i++)
                {
                    Vector3 randPos = new Vector3(Random.Range(-9f, 9f), Random.Range(-9f, 9f), 0) + new Vector3(-800, 0, 0);
                    while (Physics2D.OverlapCircle(randPos, 0.7f))
                    {
                        randPos = new Vector3(Random.Range(-9f, 9f), Random.Range(-9f, 9f), 0) + new Vector3(-800, 0, 0);
                    }
                    Instantiate(summon, randPos, Quaternion.identity);
                }
                yield return new WaitForSeconds(2f / 8f);
                animator.SetTrigger("Idle");
            }
        }
        attacking = false;
    }

    GameObject pickSpear(float angle)
    {
        if (angle >= 180 && angle <= 360)
        {
            if (angle >= 180 && angle < 200)
            {
                return spreadSpears[4];
            }
            else if (angle >= 200 && angle < 230)
            {
                return spreadSpears[5];
            }
            else if (angle >= 230 && angle < 260)
            {
                return spreadSpears[6];
            }
            else if (angle >= 260 && angle < 280)
            {
                return spreadSpears[3];
            }
            else if (angle >= 280 && angle < 310)
            {
                return spreadSpears[2];
            }
            else if (angle >= 320 && angle < 340)
            {
                return spreadSpears[1];
            }
            else
            {
                return spreadSpears[0];
            }
        }
        else
        {
            return null;
        }
    }

    IEnumerator spreadSpearAttack()
    {
        attackDuration = 6;
        for (int i = 0; i < 3; i++)
        {
            if (dead == false)
            {
                animator.SetTrigger("Spread Spear");
                this.GetComponents<AudioSource>()[2].Play();
                yield return new WaitForSeconds(3 / 8f);
                float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
                GameObject spawnSpear = pickSpear(angleToShip);
                GameObject spawnSpear2 = pickSpear(angleToShip + 30);
                GameObject spawnSpear3 = pickSpear(angleToShip - 30);
                GameObject instant = Instantiate(spawnSpear, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                instant.GetComponent<SpreadSpear>().angleTravel = angleToShip;
                instant = Instantiate(spawnSpear2, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                instant.GetComponent<SpreadSpear>().angleTravel = angleToShip + 30;
                instant = Instantiate(spawnSpear3, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                instant.GetComponent<SpreadSpear>().angleTravel = angleToShip - 30;
                yield return new WaitForSeconds(3 / 8f);
                animator.SetTrigger("Idle");
            }
        }
        spreadAttacking = false;
        attacking = false;
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

    IEnumerator downWardsSpearAttack()
    {
        attackDuration = 4;
        posArray.Clear();
        animator.SetTrigger("Throw Spear");
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(6f / 8f);
        animator.SetTrigger("Idle");
        Instantiate(spearIndicator, playerShip.transform.position, Quaternion.identity);
        posArray.Add(playerShip.transform.position);
        for(int i = 0; i < 3; i++)
        {
            if (dead == false)
            {
                Vector3 newPos = new Vector3(Random.Range(playerShip.transform.position.x - 3, playerShip.transform.position.x + 3),
                                             Random.Range(playerShip.transform.position.y - 3, playerShip.transform.position.y + 3), 0);
                while (isPositionPicked(posArray, newPos) == true)
                {
                    newPos = new Vector3(Random.Range(playerShip.transform.position.x - 3, playerShip.transform.position.x + 3),
                                         Random.Range(playerShip.transform.position.y - 3, playerShip.transform.position.y + 3), 0);
                }
                posArray.Add(newPos);
                Instantiate(spearIndicator, newPos, Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
            }
        }
        attacking = false;
    }

	void Start () {
        Camera.main.GetComponent<CameraShake>().shakeCamFunction(3f, 0.3f);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
        playerShip = GameObject.Find("PlayerShip");
        instantiatedAStarGrid = Instantiate(aStarGrid, Vector3.zero + new Vector3(-800, 0, 0), Quaternion.identity);
        for (int i = 0; i < doorSeals.Length; i++)
        {
            doorSeals[i].SetActive(true);
        }
        boxCol.enabled = false;
    }

    void Update() {
        if (landed == true)
        {
            if (dead == false)
            {
                if (attackDuration > 0)
                {
                    attackDuration -= Time.deltaTime;
                }
                else
                {
                    int whatAttack;
                    if (attacking == false)
                    {
                        whatAttack = Random.Range(1, 7);
                        attacking = true;
                    }
                    else
                    {
                        whatAttack = 5;
                    }

                    if (whatAttack <= 3)
                    {
                        StartCoroutine(downWardsSpearAttack());
                    }
                    else if (whatAttack >= 4 && whatAttack < 6)
                    {
                        spreadAttacking = true;
                        if (transform.position == topPos)
                        {
                            StartCoroutine(spreadSpearAttack());
                        }
                    }
                    else if (whatAttack == 6)
                    {
                        StartCoroutine(summonSkeles());
                    }
                }

                if (spreadAttacking == false)
                {
                    if (transform.position != middlePos)
                    {
                        if (Vector2.Distance(transform.position, middlePos) < 0.1f)
                        {
                            transform.position = middlePos;
                        }
                        speed = Vector2.Distance(transform.position, middlePos) * 2f;
                        transform.position += Vector3.down * Time.deltaTime * speed;
                    }
                }
                else
                {
                    if (transform.position != topPos)
                    {
                        if (Vector2.Distance(transform.position, topPos) < 0.1f)
                        {
                            transform.position = topPos;
                        }
                        speed = Vector2.Distance(transform.position, topPos) * 2f;
                        transform.position += Vector3.up * Time.deltaTime * speed;
                    }
                }
            }
            else
            {
                transform.position += Vector3.up * Time.deltaTime * 5;
                if(transform.position.y > 18 || Camera.main.transform.position.y > 10)
                {
                    Destroy(instantiatedAStarGrid);
                    Instantiate(guidanceArrow, new Vector3(0, 5.6f, 0) + new Vector3(-800, 0, 0), Quaternion.Euler(0, 0, 90));
                    Destroy(this.gameObject);
                }

                GameObject[] spearMen = GameObject.FindGameObjectsWithTag("MeleeEnemy");
                if (numberKillLength != spearMen.Length)
                {
                    numberKillLength = spearMen.Length;
                    foreach (GameObject skele in spearMen)
                    {
                        Instantiate(instaKill, skele.transform.position, Quaternion.identity);
                    }
                }
            }
        }
        else
        {
            speed = Vector2.Distance(transform.position, middlePos);
            transform.position += Vector3.down * Time.deltaTime * speed;
            if (Vector2.Distance(transform.position, middlePos) < 0.1)
            {
                transform.position = middlePos;
                boxCol.enabled = true;
                landed = true;
                this.GetComponents<AudioSource>()[0].Stop();
            }
        }
    }

    IEnumerator fadeOut(AudioSource source, float speed)
    {
        while(source.volume > 0)
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
            this.GetComponents<AudioSource>()[4].Play();
            if (health <= 0)
            {
                StartCoroutine(hitFrame());
                this.gameObject.GetComponent<Collider2D>().enabled = false;
                animator.SetTrigger("Death");
                this.GetComponents<AudioSource>()[5].Play();
                StartCoroutine(fadeOut(this.GetComponents<AudioSource>()[5], 0.05f));
                GameObject[] spearMen = GameObject.FindGameObjectsWithTag("MeleeEnemy");
                foreach(GameObject skele in spearMen)
                {
                    Instantiate(instaKill, skele.transform.position, Quaternion.identity);
                }
                for(int i = 0; i < doorSeals.Length; i++)
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
