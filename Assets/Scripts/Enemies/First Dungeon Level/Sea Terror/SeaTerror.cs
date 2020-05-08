using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaTerror : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    GameObject playerShip;
    PlayerScript playerScript;
    int currView = 0;
    int mirror = 1;
    float angleToShip = 0;

    Vector3 targetTravel;

    Rigidbody2D rigidBody2D;

    public BossManager bossManager;

    public List<SeaTerrorTentacle> tentacleList = new List<SeaTerrorTentacle>();

    float summonSwipeTentacle = 0;
    float summonSlamTentacle = 0;

    public GameObject tentacle;

    public Sprite[] views;

    public GameObject waterSplash;

    [SerializeField] AudioSource waterAudio;

    int pickView(float angle)
    {
        if (angle > 22.5f && angle <= 67.5f)
        {
            mirror = 1;
            return 3;
        }
        else if (angle > 67.5f && angle <= 112.5)
        {
            mirror = 1;
            return 4;
        }
        else if (angle > 112.5 && angle <= 157.5)
        {
            mirror = -1;
            return 3;
        }
        else if (angle > 157.5 && angle <= 202.5)
        {
            mirror = -1;
            return 2;
        }
        else if (angle > 202.5f && angle <= 247.5f)
        {
            mirror = -1;
            return 1;
        }
        else if (angle > 247.5f && angle <= 292.5f)
        {
            mirror = 1;
            return 0;
        }
        else if (angle > 292.5f && angle < 337.5)
        {
            mirror = 1;
            return 1;
        }
        else
        {
            mirror = 1;
            return 2;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerShip = GameObject.Find("PlayerShip");
        playerScript = playerShip.GetComponent<PlayerScript>();
        targetTravel = Camera.main.transform.position + new Vector3(Camera.main.transform.position.x - playerShip.transform.position.x, Camera.main.transform.position.y - playerShip.transform.position.y).normalized * 4.5f;
        FindObjectOfType<BossHealthBar>().targetEnemy = GetComponent<Enemy>();
        FindObjectOfType<BossHealthBar>().bossStartUp("Sea Terror");
        StartCoroutine(mainGameLoop());
    }

    void summonSwiper()
    {
        float summonAngle = Random.Range(0, 2 * Mathf.PI);
        GameObject tentacleInstant = Instantiate(
            tentacle, 
            new Vector3(
                Mathf.Clamp((playerShip.transform.position + new Vector3(Mathf.Cos(summonAngle), Mathf.Sin(summonAngle)) * 2).x, 1400 - 7.5f, 1400 + 7.5f),
                Mathf.Clamp((playerShip.transform.position + new Vector3(Mathf.Cos(summonAngle), Mathf.Sin(summonAngle)) * 2).y, 20 - 7.5f, 20 + 7.5f)
                ), 
            Quaternion.identity);
        tentacleList.Add(tentacleInstant.GetComponent<SeaTerrorTentacle>());

        if(Random.Range(0,2) == 1)
        {
            Vector3 scale = tentacleInstant.transform.localScale;
            tentacleInstant.transform.localScale = new Vector3(scale.x * -1, scale.y);
        }

    }

    void summonSlammer()
    {
        GameObject tentacleInstant = Instantiate(
            tentacle,
            new Vector3(
                Mathf.Clamp(playerShip.transform.position.x - 4.4f, 1400 - 7.5f, 1400 + 7.5f),
                playerShip.transform.position.y
                ),
            Quaternion.identity);
        tentacleList.Add(tentacleInstant.GetComponent<SeaTerrorTentacle>());
        tentacleInstant.GetComponent<SeaTerrorTentacle>().slamTentacle = true;
        Vector3 scale = tentacleInstant.transform.localScale;
        tentacleInstant.transform.localScale = new Vector3(-scale.x, scale.y);
        tentacleInstant = Instantiate(
            tentacle,
            new Vector3(
                Mathf.Clamp(playerShip.transform.position.x + 4.4f, 1400 - 7.5f, 1400 + 7.5f),
                playerShip.transform.position.y
            ),
        Quaternion.identity);
        tentacleInstant.GetComponent<SeaTerrorTentacle>().slamTentacle = true;
        tentacleList.Add(tentacleInstant.GetComponent<SeaTerrorTentacle>());
    }

    IEnumerator mainGameLoop()
    {
        yield return new WaitForSeconds(10 / 12f);
        waterAudio.Play();
        yield return new WaitForSeconds(7 / 12f);
        animator.enabled = false;
        while (true)
        {
            if (health > 0)
            {
                float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
                spriteRenderer.sprite = views[pickView(angleToShip)];
                transform.localScale = new Vector3(6 * mirror, 6);

                summonSwipeTentacle += Time.deltaTime;
                summonSlamTentacle += Time.deltaTime;

                if (summonSwipeTentacle > 4)
                {
                    summonSwipeTentacle = 0;
                    summonSwiper();
                }

                if (summonSlamTentacle > 8)
                {
                    summonSlamTentacle = Random.Range(0.0f, 3.0f);
                    summonSlammer();
                }
            }
            yield return null;
        }
    }

    void spawnWaterSplash()
    {
        Instantiate(waterSplash, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);

        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    public override void deathProcedure()
    {
        rigidBody2D.velocity = Vector3.zero;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        bossManager.bossBeaten(nameID, 1.167f);
        Invoke("spawnWaterSplash", 1.05f);

        FindObjectOfType<BossHealthBar>().bossEnd();
        animator.enabled = true;
        animator.SetTrigger("Death");
        Destroy(this.gameObject, 1.167f);
    }

    public override void damageProcedure(int damage)
    {
        this.GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }
}
