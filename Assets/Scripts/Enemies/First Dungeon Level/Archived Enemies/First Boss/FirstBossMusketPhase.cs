using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossMusketPhase : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCol;

    public int health = 15;
    GameObject playerShip;

    float attackDuration = 1f;

    List<Vector3> posArray = new List<Vector3>();

    Vector3 middlePos = new Vector3(-800, 42.5f, 0);
    Vector3 topPos = new Vector3(-800, 48.5f, 0);
    Vector3[] sidePositionList = new Vector3[8] {new Vector3(-800, 48.5f), new Vector3(-800, 48.5f), new Vector3(-792, 48.5f), new Vector3(-792, 40), new Vector3(-792, 40), new Vector3(-808, 35), new Vector3(-792, 35), new Vector3(-800, 35)};
    Vector3 targetPos;
    float speed = 4f;
    public GameObject summon, aStarGrid;
    GameObject instantiatedAStarGrid;

    bool landed = false;
    bool dead = false;
    bool entryAnimPlayed = false;

    public GameObject[] doorSeals;
    public GameObject instaKill;
    int numberKillLength = 0;
    bool movedToTopPos;
    bool isAttacking = false;

    float angleToShip = 0;
    float directionIndex = 0;

    public GameObject musketFlare, laser, ball;

    int numberLasers = 0;
    int numberAttacks = 0;
    int numberMoves = 0;

    public GameObject deadBoss;
    public GameObject[] lightRays;
    public GameObject smallWaterSplash;

    IEnumerator summonSkeles()
    {
        isAttacking = true;
        attackDuration = 18f;
        for (int k = 0; k < 3; k++)
        {
            if (dead == false)
            {
                animator.SetTrigger("Summon");
                this.GetComponents<AudioSource>()[1].Play();
                yield return new WaitForSeconds(5f / 8f);
                for (int i = 0; i < 2; i++)
                {
                    Vector3 randPos = new Vector3(Random.Range(-9f, 9f), Random.Range(-9f, 9f), 0) + new Vector3(-800, 40, 0);
                    while (Physics2D.OverlapCircle(randPos, 0.7f))
                    {
                        randPos = new Vector3(Random.Range(-9f, 9f), Random.Range(-9f, 9f), 0) + new Vector3(-800, 40, 0);
                    }
                    Instantiate(summon, randPos, Quaternion.identity);
                }
                yield return new WaitForSeconds(2f / 8f);
            }
        }
        directionIndex = 0;
        pickAnim(angleToShip);
        isAttacking = false;
    }

    bool isPositionPicked(List<Vector3> posArray, Vector3 pos)
    {
        for (int i = 0; i < posArray.Count; i++)
        {
            if (Vector2.Distance(posArray[i], pos) < 0.6f || Physics2D.OverlapCircle(pos, 0.2f))
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator fireMusket(float angleOrientation, GameObject bullet)
    {
        pickAnim(angleOrientation);
        attackDuration = 1.5f;
        isAttacking = true;
        GameObject instantMusketFlare;
        if (dead == false)
        {
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                instantMusketFlare = Instantiate(musketFlare, transform.position + new Vector3(2, 3.91f, 0), Quaternion.Euler(0, 0, 135));
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                instantMusketFlare = Instantiate(musketFlare, transform.position + new Vector3(0, 4.44f, 0), Quaternion.Euler(0, 0, 135));
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                instantMusketFlare = Instantiate(musketFlare, transform.position + new Vector3(-2, 3.91f, 0), Quaternion.Euler(0, 0, -135));
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                instantMusketFlare = Instantiate(musketFlare, transform.position + new Vector3(4.07f, 1.19f, 0), Quaternion.Euler(0, 0, 90));
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                instantMusketFlare = Instantiate(musketFlare, transform.position + new Vector3(-2.04f, -0.74f, 0), Quaternion.Euler(0, 0, -45));
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                instantMusketFlare = Instantiate(musketFlare, transform.position + new Vector3(0, -1.95f, 0), Quaternion.Euler(0, 0, 0));
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                instantMusketFlare = Instantiate(musketFlare, transform.position + new Vector3(2.04f, -0.74f, 0), Quaternion.Euler(0, 0, 45));
            }
            else
            {
                instantMusketFlare = Instantiate(musketFlare, transform.position + new Vector3(-4.07f, 1.19f, 0), Quaternion.Euler(0, 0, -90));
            }
        }
        else
        {
            instantMusketFlare = this.gameObject;
        }

        this.GetComponents<AudioSource>()[2].Play();
        yield return new WaitForSeconds(3f / 12f);
        if (dead == false)
        {
            GameObject bulletInstance = Instantiate(bullet, instantMusketFlare.transform.position, Quaternion.identity);
            if (bullet == ball)
            {
                bulletInstance.GetComponent<FirstBossEnergyBall>().angleTravel = angleOrientation;
            }
            else
            {
                bulletInstance.GetComponent<FirstBossLaser>().angleTravel = angleOrientation;
            }
        }
        yield return new WaitForSeconds(4f / 12f);
        isAttacking = false;
    }

    void pickAnim(float angleOrientation)
    {
        if (dead == false)
        {
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                if(directionIndex != 1)
                {
                    directionIndex = 1;
                    animator.SetTrigger("Idle4");
                    transform.localScale = new Vector3(0.26f, 0.26f, 0);
                }
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                if (directionIndex != 2)
                {
                    directionIndex = 2;
                    animator.SetTrigger("Idle5");
                    transform.localScale = new Vector3(0.26f, 0.26f, 0);
                }
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                if (directionIndex != 3)
                {
                    directionIndex = 3;
                    animator.SetTrigger("Idle4");
                    transform.localScale = new Vector3(-0.26f, 0.26f, 0);
                }
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                if (directionIndex != 4)
                {
                    directionIndex = 4;
                    animator.SetTrigger("Idle3");
                    transform.localScale = new Vector3(-0.26f, 0.26f, 0);
                }
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                if (directionIndex != 5)
                {
                    directionIndex = 5;
                    animator.SetTrigger("Idle2");
                    transform.localScale = new Vector3(-0.26f, 0.26f, 0);
                }
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                if (directionIndex != 6)
                {
                    directionIndex = 6;
                    animator.SetTrigger("Idle1");
                    transform.localScale = new Vector3(0.26f, 0.26f, 0);
                }
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                if (directionIndex != 7)
                {
                    directionIndex = 7;
                    animator.SetTrigger("Idle2");
                    transform.localScale = new Vector3(0.26f, 0.26f, 0);
                }
            }
            else
            {
                if (directionIndex != 8)
                {
                    directionIndex = 8;
                    animator.SetTrigger("Idle3");
                    transform.localScale = new Vector3(0.26f, 0.26f, 0);
                }
            }
        }
    }

    void Start()
    {
        Camera.main.GetComponent<CameraShake>().shakeCamFunction(3f, 0.3f);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
        playerShip = GameObject.Find("PlayerShip");
        instantiatedAStarGrid = Instantiate(aStarGrid, new Vector3(-800, 40, 0), Quaternion.identity);
        for (int i = 0; i < doorSeals.Length; i++)
        {
            doorSeals[i].SetActive(true);
        }
        boxCol.enabled = false;
        targetPos = topPos;
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
                    pickAnim(angleToShip);
                    speed = Vector2.Distance(transform.position, topPos) * 4;
                    transform.position += Vector3.up * Time.deltaTime * speed;
                    if (Vector2.Distance(topPos, transform.position) < 0.1f)
                    {
                        transform.position = topPos;
                        movedToTopPos = true;
                    }
                }
                else
                {
                    if (attackDuration > 0)
                    {
                        if(Vector2.Distance(transform.position, targetPos) > 0.2f)
                        {
                            if (isAttacking == false)
                            {
                                speed = Vector2.Distance(transform.position, targetPos) * 3;
                                float angleTravel = (360 + Mathf.Atan2(targetPos.y - transform.position.y, targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360;
                                transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;
                            }
                        }
                        else
                        {
                            transform.position = targetPos;
                        }

                        if (isAttacking == false)
                        {
                            pickAnim(angleToShip);
                        }
                        attackDuration -= Time.deltaTime;
                    }
                    else
                    {
                        if(numberAttacks < 6)
                        {
                            if(numberLasers < 3)
                            {
                                StartCoroutine(fireMusket(angleToShip, laser));
                                numberLasers++;
                            }
                            else
                            {
                                StartCoroutine(fireMusket(angleToShip, ball));
                                numberLasers = 0;
                            }
                            numberMoves++;
                            numberAttacks++;
                            if (numberMoves > 2)
                            {
                                targetPos = sidePositionList[Random.Range(0, sidePositionList.Length)];
                                while(Vector2.Distance(targetPos, transform.position) < 0.2f)
                                {
                                    targetPos = sidePositionList[Random.Range(0, sidePositionList.Length)];
                                }
                                numberMoves = 0;
                            }
                        }
                        else
                        {
                            StartCoroutine(summonSkeles());
                            numberAttacks = 0;
                        }
                    }
                }
            }
            else
            {
                GameObject[] swordMen = GameObject.FindGameObjectsWithTag("RangedEnemy");
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
                    this.GetComponents<AudioSource>()[3].Stop();
                    StartCoroutine(introAnim());
                }
            }
        }
    }

    IEnumerator introAnim()
    {
        animator.SetTrigger("Entry");
        this.GetComponents<AudioSource>()[4].Play();
        yield return new WaitForSeconds(1.083f / 0.667f);
        landed = true;
        boxCol.enabled = true;
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
                StartCoroutine(hitFrame());
                Instantiate(deadBoss, transform.position, Quaternion.identity);
                this.gameObject.GetComponent<Collider2D>().enabled = false;
                playerShip.GetComponent<PlayerScript>().playerDead = true;
                GameObject[] swordMen = GameObject.FindGameObjectsWithTag("RangedEnemy");
                foreach(GameObject enemy in swordMen)
                {
                    Instantiate(smallWaterSplash, enemy.transform.position, Quaternion.identity);
                    Destroy(enemy);
                }
                foreach(GameObject ray in lightRays)
                {
                    ray.SetActive(true);
                }
                Destroy(this.gameObject);
                FindObjectOfType<AudioManager>().FadeOut("First Boss Background Music", 0.1f);
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
