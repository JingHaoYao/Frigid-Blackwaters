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
    public DemoBossCrystal[] demoCrystals;

    float rotatePeriod = 0;

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

    private void Start()
    {
        headSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
        bodySpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            this.GetComponents<AudioSource>()[4].Play();
            if (health <= 0)
            {
                rigidBody2D.velocity = Vector3.zero;
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                addKills();
                FindObjectOfType<BossHealthBar>().bossEnd();
                this.GetComponents<AudioSource>()[1].Play();
                Destroy(this.gameObject, 0.75f);
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
