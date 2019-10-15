using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpentEmergeAttack : MonoBehaviour {
    public AntiSpawnSpaceDetailer anti;
    public SeaSerpentEnemy seaSerpentEnemy;
    SpriteRenderer spriteRenderer;
    GameObject playerShip;
    PolygonCollider2D polyCol;
    public Collider2D damageHitBox, obstacleHitBox;

	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShip = GameObject.Find("PlayerShip");
        damageHitBox.enabled = false;
        StartCoroutine(activateHitBox());
        Destroy(this.gameObject, 1.7f);
        Invoke("turnOffObstacleHitBox", 1.75f / 1.5f);
        Invoke("playSplash", (14f / 12f) / 1.5f);
	}

    void turnOffObstacleHitBox()
    {
        obstacleHitBox.enabled = false;
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    void playSplash()
    {
        this.GetComponents<AudioSource>()[0].Play();
    }

    IEnumerator activateHitBox()
    {
        yield return new WaitForSeconds((3f / 12f) / 1.5f);
        damageHitBox.enabled = true;
        yield return new WaitForSeconds((7f / 12f) / 1.5f);
        damageHitBox.enabled = false;
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 16)
        {
            this.GetComponents<AudioSource>()[2].Play();
            seaSerpentEnemy.GetComponent<Enemy>().dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            StartCoroutine(hitFrame());
        }
    }
}
