using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGiantsHeartFist : MonoBehaviour {
    PolygonCollider2D[] colliderList;
    Animator animator;
    GameObject playerShip;
    SpriteRenderer spriteRenderer;

    IEnumerator fallFist()
    {
        FindObjectOfType<AudioManager>().PlaySound("Stone Giant Fist Splash");
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.enabled = true;
        animator.enabled = true;
        yield return new WaitForSeconds(1 / 12f);
        colliderList[0].enabled = true;
        yield return new WaitForSeconds(3f / 12f);
        colliderList[0].enabled = false;
        yield return new WaitForSeconds(4f / 12f);
        animator.SetTrigger("Fall");
        FindObjectOfType<AudioManager>().PlaySound("Stone Giant Fist Splash");
        Destroy(this.gameObject, 0.583f);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliderList = GetComponents<PolygonCollider2D>();
        spriteRenderer.enabled = false;
        animator.enabled = false;
        StartCoroutine(fallFist());
        playerShip = GameObject.Find("PlayerShip");
    }

    void Update()
    {

    }
}
