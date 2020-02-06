using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : PlayerProjectile
{
    public GameObject bulletImpact;
    public float angleTravel;
    SpriteRenderer spriteRenderer;
    public bool highVelocity = false;
    public float stunDuration = 2;
    public GameObject stunStatus;

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10) + 4;
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (highVelocity == false)
        {
            Destroy(this.gameObject, 0.5f);
        }
        else
        {
            StartCoroutine(turnOffRenderer());
            Destroy(this.gameObject, stunDuration + 0.2f);
        }
        if (PlayerUpgrades.sniperUpgrades.Count >= 2)
        {
            this.GetComponent<DamageAmount>().damage += 1;
        }
    }

    IEnumerator turnOffRenderer()
    {
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.enabled = false;
    }

    void Update()
    {
        pickRendererLayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RoomHitbox" || collision.gameObject.tag == "RoomWall" || collision.gameObject.tag == "EnemyShield")
        {
            GetComponent<AudioSource>().Play();
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            Instantiate(bulletImpact, transform.position, Quaternion.Euler(0, 0, angleTravel + 90));
            if (collision.gameObject.GetComponent<Enemy>() && highVelocity == true)
            {
                Enemy targetEnemy = collision.gameObject.GetComponent<Enemy>();
                GameObject statusInstant = Instantiate(stunStatus, targetEnemy.transform.position + Vector3.up * 2, Quaternion.identity);
                targetEnemy.stunEnemy(stunDuration);
                targetEnemy.addStatus(statusInstant.GetComponent<EnemyStatusEffect>(), stunDuration);
                
            }
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
