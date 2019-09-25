using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpikeScript : MonoBehaviour {
    public GameObject waterSplash;
    PolygonCollider2D polyCol;
    GameObject playerShip;
    SpriteRenderer spriteRenderer;

    void pickRendererLayer()
    {
        if (Vector2.Distance(transform.position, playerShip.transform.position) <= 1.5f)
        {
            spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        else
        {
            spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
        }
    }

    IEnumerator instantiateHitBox()
    {
        polyCol.enabled = false;
        yield return new WaitForSeconds(4f / 12f);
        polyCol.enabled = true;
        yield return new WaitForSeconds(2f / 12f);
        polyCol.enabled = false;
    }

    void spawnWaterSplash()
    {
        GameObject splash = Instantiate(waterSplash, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);
        splash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder - 1;
    }

	void Start () {
        Destroy(this.gameObject, 0.833f);
        Invoke("spawnWaterSplash", 8f / 12f);
        playerShip = GameObject.Find("PlayerShip");
        polyCol = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(instantiateHitBox());
	}
	
	void Update () {
        pickRendererLayer();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            playerShip.GetComponent<PlayerScript>().amountDamage += 400;
        }
    }
}
