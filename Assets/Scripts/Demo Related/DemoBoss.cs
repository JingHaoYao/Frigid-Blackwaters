using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBoss : Enemy
{
    public Sprite[] headSpriteList;
    public Sprite[] symbolList;
    Rigidbody2D rigidBody2D;
    SpriteRenderer bodySpriteRenderer;
    SpriteRenderer headSpriteRenderer;
    SpriteRenderer symbolSpriteRenderer;
    public DemoBossCrystal[] demoCrystals;

    float rotatePeriod = 0;
    List<int> attackSequences = new List<int>();

    float rechargePeriod = 7;

    int pickView(float angle)
    {
        if (angle > 255 && angle <= 285)
        {
            return 0;
        }
        else if (angle > 285 && angle <= 360)
        {
            return 5;
        }
        else if (angle > 180 && angle <= 255)
        {
            return 1;
        }
        else if (angle > 75 && angle <= 105)
        {
            return 3;
        }
        else if (angle > 0 && angle <= 75)
        {
            return 6;
        }
        else
        {
            return 2;
        }
    }

    IEnumerator pickAttack()
    {
        if((float)health/maxHealth > 0.5f)
        {
            for(int i = 0; i < 2; i++)
            {
                attackSequences.Add(Random.Range(0, symbolList.Length));
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                attackSequences.Add(Random.Range(0, symbolList.Length));
            }
        }

        foreach(int whatAttack in attackSequences)
        {
            symbolSpriteRenderer.sprite = symbolList[whatAttack];
            symbolSpriteRenderer.GetComponent<Animator>().SetTrigger("FadeInFadeOut");
            yield return new WaitForSeconds(1f);
        }

        foreach(int whatAttack in attackSequences)
        {
            demoCrystals[0].attack(whatAttack);
            demoCrystals[1].attack(whatAttack);
            yield return new WaitForSeconds(1.5f);
        }

        attackSequences.Clear();
    }

    private void Start()
    {
        headSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
        bodySpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[2];
        symbolSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[5];
        rigidBody2D = GetComponent<Rigidbody2D>();
        FindObjectOfType<BossHealthBar>().bossStartUp("The Watcher");
        FindObjectOfType<BossHealthBar>().targetEnemy = this;
    }

    private void Update()
    {
        if (rotatePeriod < Mathf.PI * 2)
        {
            rotatePeriod += Time.deltaTime;
        }
        else
        {
            rotatePeriod = 0;
        }

        for(int i = 0; i < demoCrystals.Length; i++)
        {
            demoCrystals[i].transform.position = transform.position + new Vector3(Mathf.Cos(i * Mathf.PI + rotatePeriod), Mathf.Sin(i * Mathf.PI + rotatePeriod) * 0.8f + 0.6f) * 3.5f;
            demoCrystals[i].currAngle = (i * Mathf.PI + rotatePeriod) * Mathf.Rad2Deg;
        }
        symbolSpriteRenderer.sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder + 1;
        bodySpriteRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;

        rechargePeriod += Time.deltaTime;
        if(rechargePeriod > 10)
        {
            rechargePeriod = 0;
            StartCoroutine(pickAttack());
        }
    }

    IEnumerator bossDefeated()
    {
        bodySpriteRenderer.GetComponent<Animator>().SetTrigger("Explode");
        headSpriteRenderer.GetComponent<Animator>().enabled = true;
        headSpriteRenderer.GetComponent<Animator>().SetTrigger("Explode");
        foreach(DemoBossCrystal crystal in demoCrystals)
        {
            crystal.destroyCrystal();
        }
        GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(0.75f);
        FindObjectOfType<DemoBossManager>().loadTitleScreen();
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            GetComponents<AudioSource>()[0].Play();
            if (health <= 0)
            {
                rigidBody2D.velocity = Vector3.zero;
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                addKills();
                FindObjectOfType<BossHealthBar>().bossEnd();
                FindObjectOfType<PlayerScript>().playerDead = true;
                StartCoroutine(bossDefeated());
            }
            else
            {
                StartCoroutine(hitFrame());
            }
        }
    }

    IEnumerator hitFrame()
    {
        bodySpriteRenderer.color = Color.red;
        headSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        bodySpriteRenderer.color = Color.white;
        headSpriteRenderer.color = Color.white;
    }
}
