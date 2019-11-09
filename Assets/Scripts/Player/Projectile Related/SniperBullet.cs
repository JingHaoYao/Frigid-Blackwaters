using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    public GameObject bulletImpact;
    public float angleTravel;
    SpriteRenderer spriteRenderer;
    public bool highVelocity = false;
    public float stunDuration = 2;

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

    IEnumerator stunEnemy(GameObject target)
    {
        float duration = 0;
        while(duration < stunDuration && target != null)
        {
            target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            target.GetComponent<Enemy>().stopAttacking = true;
            duration += Time.deltaTime;
            yield return null;
        }

        if (target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            target.GetComponent<Enemy>().stopAttacking = false;
        }
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
                StartCoroutine(stunEnemy(collision.gameObject));
            }
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
