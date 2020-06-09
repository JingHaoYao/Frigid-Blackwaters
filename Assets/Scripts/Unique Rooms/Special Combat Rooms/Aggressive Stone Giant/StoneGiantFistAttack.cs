using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGiantFistAttack : MonoBehaviour {
    PolygonCollider2D[] colliderList;
    Animator animator;
    GameObject playerShip;

    IEnumerator fallFist()
    {
        yield return new WaitForSeconds(1 / 12f);
        colliderList[0].enabled = true;
        yield return new WaitForSeconds(3f / 12f);
        colliderList[0].enabled = false;
        yield return new WaitForSeconds(2f / 12f);
        this.GetComponent<AudioSource>().Play();
        animator.SetTrigger("Fall");
        Invoke("turnOffRenderer", 0.583f);
        Destroy(this.gameObject, 2f);
    }

	void Start () {
        animator = GetComponent<Animator>();
        colliderList = GetComponents <PolygonCollider2D>();
        StartCoroutine(fallFist());
        playerShip = GameObject.Find("PlayerShip");
	}

    void turnOffRenderer()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(800, this.gameObject);
        }
    }
}
