using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnemoneEnemy : Enemy
{
    Animator animator;
    BoxCollider2D boxCol;
    SpriteRenderer spriteRenderer;
    public GameObject obstacleHitBox;
    public GameObject deadAnemone;
    public GameObject anemoneShot;
    float divePeriod = 4;
    public GameObject splash;
    public GameObject enemyIndicator;
    public int whatAnemoneType = 1;

    Vector3 pickRandPos()
    {
        Vector3 randPos = new Vector3(Camera.main.transform.position.x + Random.Range(-8.0f, 8.0f), Camera.main.transform.position.y + Random.Range(-8.0f, 6.5f));
        while (Physics2D.OverlapCircle(randPos, .5f))
        {
            randPos = new Vector3(Camera.main.transform.position.x + Random.Range(-8.0f, 8.0f), Camera.main.transform.position.y + Random.Range(-8.0f, 6.5f));
        }
        return randPos;
    }

    void blueAnemoneAttack()
    {
        for(int i = 0; i < 8; i++)
        {
            float angle = i * 45;
            GameObject shot = Instantiate(anemoneShot, transform.position + new Vector3(0, 1.3f, 0), Quaternion.Euler(0, 0, angle));
            shot.GetComponent<AnemoneShot>().angleTravel = angle * Mathf.Deg2Rad;
        }
    }

    void greenAnemoneAttack()
    {
        for(int i = 0; i < 4; i++)
        {
            float angle = i * 90;
            GameObject shot = Instantiate(anemoneShot, transform.position + new Vector3(0, 1.3f, 0), Quaternion.Euler(0, 0, angle - 5f));
            shot.GetComponent<AnemoneShot>().angleTravel = (angle - 5f) * Mathf.Deg2Rad;
            shot = Instantiate(anemoneShot, transform.position + new Vector3(0, 1.3f, 0), Quaternion.Euler(0, 0, angle + 5f));
            shot.GetComponent<AnemoneShot>().angleTravel = (angle + 5f) * Mathf.Deg2Rad;
        }
    }
    
    void purpleAnemoneAttack()
    {
        for (int i = 0; i < 4; i++)
        {
            float angle = (i * 90) + 45;
            GameObject shot = Instantiate(anemoneShot, transform.position + new Vector3(0, 1.3f, 0), Quaternion.Euler(0, 0, angle - 5f));
            shot.GetComponent<AnemoneShot>().angleTravel = (angle - 5f) * Mathf.Deg2Rad;
            shot = Instantiate(anemoneShot, transform.position + new Vector3(0, 1.3f, 0), Quaternion.Euler(0, 0, angle + 5f));
            shot.GetComponent<AnemoneShot>().angleTravel = (angle + 5f) * Mathf.Deg2Rad;
        }
    }

    void Start()
    {
        divePeriod = Random.Range(2f, 5f);
        boxCol = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        boxCol.enabled = false;
        obstacleHitBox.SetActive(false);
        enemyIndicator.SetActive(false);
    }

    IEnumerator attackAndWait()
    {
        spriteRenderer.enabled = true;
        boxCol.enabled = true;
        obstacleHitBox.SetActive(true);
        enemyIndicator.SetActive(true);
        animator.SetTrigger("Dive Out");
        GameObject waterSplash = Instantiate(splash, transform.position, Quaternion.identity);
        waterSplash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(5f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        //attack
        if (stopAttacking == false)
        {
            if (whatAnemoneType == 1)
            {
                blueAnemoneAttack();
            }
            else if(whatAnemoneType == 2)
            {
                greenAnemoneAttack();
            }
            else if(whatAnemoneType == 3)
            {
                purpleAnemoneAttack();
            }
        }
        yield return new WaitForSeconds(4f / 12f);
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("Dive In");
        waterSplash = Instantiate(splash, transform.position, Quaternion.identity);
        waterSplash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.enabled = false;
        boxCol.enabled = false;
        obstacleHitBox.SetActive(false);
        enemyIndicator.SetActive(false);
    }

    void Update()
    {
        if(divePeriod > 0)
        {
            divePeriod -= Time.deltaTime;
        }
        else
        {
            divePeriod = 4.5f;
            transform.position = pickRandPos();
            StartCoroutine(attackAndWait());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            this.GetComponents<AudioSource>()[0].Play();
            if (health <= 0)
            {
                Instantiate(deadAnemone, transform.position, Quaternion.identity);
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
