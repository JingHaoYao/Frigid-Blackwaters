using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserFrostMage : Enemy
{
    Animator animator;
    GameObject playerShip;
    SpriteRenderer spriteRenderer;
    public Sprite[] spriteList;
    int whatView = 0;
    int mirror;
    public GameObject deadSkele;
    public GameObject iceMissile;
    float attackPeriod = 2;
    public int whatType = 0;

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
            mirror = -1;
        }
        else if (angle > 180 && angle <= 255)
        {
            whatView = 1;
            mirror = 1;
        }
        else if (angle > 75 && angle <= 105)
        {
            whatView = 4;
            mirror = 1;
        }
        else if (angle >= 0 && angle <= 75)
        {
            whatView = 3;
            mirror = 1;
        }
        else
        {
            whatView = 3;
            mirror = -1;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShip = GameObject.Find("PlayerShip");
        animator.enabled = false;
    }

    void Update()
    {
        if (attackPeriod > 0)
        {
            float angleToShip = (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 360f) % 360f;
            pickView(angleToShip);
            transform.localScale = new Vector3(0.11f * mirror, 0.11f);
            spriteRenderer.sprite = spriteList[whatView - 1];
            attackPeriod -= Time.deltaTime;
        }
        else
        {
            if (stopAttacking == false)
            {
                attackPeriod = 2.5f;
                StartCoroutine(summonMissile());
            }
        }
    }

    IEnumerator summonMissile()
    {
        animator.enabled = true;
        this.GetComponents<AudioSource>()[1].Play();
        animator.SetTrigger("Attack" + whatView.ToString());
        yield return new WaitForSeconds(4f / 12f);
        GameObject instant = Instantiate(iceMissile, transform.position + new Vector3(Mathf.Cos(Random.Range(0, Mathf.PI * 2)), 1.5f + Mathf.Sin(Random.Range(0, Mathf.PI * 2)), 0), Quaternion.identity);
        instant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        if(whatType == 1)
        {
            instant.GetComponent<LesserFrostMageMissile>().cw = true;
        }
        yield return new WaitForSeconds(8 / 12f);
        animator.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
            health -= damageDealt;
            this.GetComponents<AudioSource>()[0].Play();
            if (health <= 0)
            {
                GameObject dead = Instantiate(deadSkele, transform.position, Quaternion.identity);
                dead.GetComponent<DeadEnemyScript>().whatView = whatView;
                addKills();
                Destroy(this.gameObject);
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
