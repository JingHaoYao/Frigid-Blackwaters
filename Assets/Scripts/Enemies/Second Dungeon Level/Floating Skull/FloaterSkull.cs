using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterSkull : Enemy
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidBody2D;
    public Sprite[] closedViews;
    int mirror = 1, whatView = 0;
    GameObject playerShip;
    public GameObject deadTower;
    public GameObject summonHeadless;
    public float attackPeriod = 0;
    bool isAttacking = false;

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
        attackPeriod = Random.Range(3.0f, 5.0f);
    }

    IEnumerator attack()
    {
        isAttacking = true;
        animator.enabled = true;
        animator.SetTrigger("Attack" + whatView.ToString());
        yield return new WaitForSeconds(4f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(4f / 12f);
        float offset = Random.Range(0, 45);
        Vector3 randPos = playerShip.transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1));
        Vector3 summonPos = new Vector3(Mathf.Clamp(randPos.x, Camera.main.transform.position.x - 8, Camera.main.transform.position.x + 8), Mathf.Clamp(randPos.y, Camera.main.transform.position.y - 8, Camera.main.transform.position.y + 8));
        GameObject instant = Instantiate(summonHeadless, summonPos, Quaternion.identity);
        if (Random.Range(0, 2) == 1)
        {
            Vector3 scale = instant.transform.localScale;
            scale.x *= -1;
            instant.transform.localScale = scale;
        }
        animator.enabled = false;
        isAttacking = false;
    }

    void Update()
    {
        float angleToShip = (360 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        pickView(angleToShip);

        spriteRenderer.sprite = closedViews[whatView - 1];

        if (isAttacking == false)
        {
            transform.localScale = new Vector3(0.35f * mirror, 0.35f);
        }

        if (attackPeriod > 0)
        {
            attackPeriod -= Time.deltaTime;
        }
        else
        {
            if (stopAttacking == false)
            {
                StartCoroutine(attack());
            }
            attackPeriod = 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            this.GetComponents<AudioSource>()[0].Play();
            StartCoroutine(hitFrame());
        }
    }

    public override void deathProcedure()
    {
        GameObject dead = Instantiate(deadTower, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {

    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
