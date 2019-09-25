using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour {
    SpriteRenderer spriteRenderer;
    public int health = 15;
    public bool outputHealth = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            health -= damageDealt;
            if (health <= 0)
            {
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(this.gameObject);
            }
            else
            {
                if(outputHealth == true)
                    Debug.Log(health);
                StartCoroutine(hitFrame());
            }
        }
    }
}
