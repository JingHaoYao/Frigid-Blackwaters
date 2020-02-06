using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : Enemy {
    SpriteRenderer spriteRenderer;
    public bool outputHealth = false;
    public bool addToPool = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (addToPool)
        {
            EnemyPool.addEnemy(this);
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
            dealDamage(damageDealt);
        }
    }

    public override void deathProcedure()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        if (outputHealth == true)
        {
            Debug.Log("Health" + health);
            Debug.Log("Damage:" + damage);
        }
        StartCoroutine(hitFrame());
    }
}
