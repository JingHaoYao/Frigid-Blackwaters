using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullTower : Enemy
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidBody2D;
    public Sprite[] closedViews;
    public Sprite[] openViews;
    int mirror = 1, whatView = 0;
    GameObject playerShip;
    public GameObject invulnerableHitBox;
    bool invulnerable = true;
    public GameObject deadTower;
    public GameObject summonHeadless;
    GameObject[] headlessSpearmen = new GameObject[3];
    public float attackPeriod = 0;
    bool isAttacking = false;
    public GameObject invulnerableIcon;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerShip = FindObjectOfType<PlayerScript>().gameObject;
        animator.enabled = false;
        attackPeriod = Random.Range(6.0f, 8.0f);
    }

    IEnumerator attack()
    {
        invulnerable = false;
        yield return new WaitForSeconds(3f);
        isAttacking = true;
        animator.enabled = true;
        animator.SetTrigger("Attack" + whatView.ToString());
        yield return new WaitForSeconds(3f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(4f / 12f);
        float offset = Random.Range(0, 45);
        for(int i = 0; i < 3; i++)
        {
            float randAngle = 120f * i;
            randAngle += offset;
            Vector3 randPos = playerShip.transform.position + new Vector3(Mathf.Cos(randAngle * Mathf.Deg2Rad), Mathf.Sin(randAngle * Mathf.Deg2Rad)) * Random.Range(1.0f, 2.0f);
            Vector3 summonPos = new Vector3(Mathf.Clamp(randPos.x, Camera.main.transform.position.x - 8, Camera.main.transform.position.x + 8), Mathf.Clamp(randPos.y, Camera.main.transform.position.y - 8, Camera.main.transform.position.y + 8));
            /*while(Physics2D.OverlapCircle(randPos, 0.4f)y
            {
                randPos = playerShip.transform.position + new Vector3(Mathf.Cos(randAngle * Mathf.Deg2Rad), Mathf.Sin(randAngle * Mathf.Deg2Rad)) * Random.Range(3.0f, 5.0f);
            }*/
            GameObject instant = Instantiate(summonHeadless, summonPos, Quaternion.identity);
            if(Random.Range(0,2) == 1)
            {
                Vector3 scale = instant.transform.localScale;
                scale.x *= -1;
                instant.transform.localScale = scale;
            }
        }
        animator.enabled = false;
        invulnerable = true;
        isAttacking = false;
    }

    void Update()
    {
        float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        pickView(angleToShip);
        
        if(invulnerable == true)
        {
            spriteRenderer.sprite = closedViews[whatView - 1];
            invulnerableHitBox.SetActive(true);
        }
        else
        {
            spriteRenderer.sprite = openViews[whatView - 1];
            invulnerableHitBox.SetActive(false);
        }
        invulnerableIcon.SetActive(invulnerableHitBox.activeSelf);

        if(isAttacking == false)
        {
            transform.localScale = new Vector3(0.35f * mirror, 0.35f);
        }

        if(attackPeriod > 0)
        {
            attackPeriod -= Time.deltaTime;
        }
        else
        {
            if (stopAttacking == false)
            {
                StartCoroutine(attack());
            }
            attackPeriod = 9;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0 && invulnerable == false)
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);

        }
    }

    public override void deathProcedure()
    {
        GameObject dead = Instantiate(deadTower, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        this.GetComponents<AudioSource>()[0].Play();
        StartCoroutine(hitFrame());
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
