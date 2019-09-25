using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGiantSlamAttack : MonoBehaviour {
    PolygonCollider2D[] colliderList;
    Animator animator;
    GameObject playerShip;
    bool rising = true;

    IEnumerator fallFist()
    {
        yield return new WaitForSeconds(1 / 12f);
        colliderList[0].enabled = true;
        yield return new WaitForSeconds(3f / 12f);
        colliderList[0].enabled = false;
        yield return new WaitForSeconds(2f / 12f);
        animator.SetTrigger("Slam");
        yield return new WaitForSeconds(2 / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        rising = false;
        colliderList[1].enabled = false;
        colliderList[2].enabled = true;
        colliderList[3].enabled = true;
        yield return new WaitForSeconds(1 / 12f);
        colliderList[2].enabled = false;
        Invoke("turnOffRenderer", 7f / 12f);
        Destroy(this.gameObject, 15f/12f);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        colliderList = GetComponents<PolygonCollider2D>();
        StartCoroutine(fallFist());
        playerShip = GameObject.Find("PlayerShip");
    }

    void turnOffRenderer()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            if (rising == true)
            {
                playerShip.GetComponent<PlayerScript>().amountDamage += 800;
            }
            else
            {
                playerShip.GetComponent<PlayerScript>().amountDamage += 1000;
            }
        }
    }
}
