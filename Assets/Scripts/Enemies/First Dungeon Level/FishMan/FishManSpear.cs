using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManSpear : MonoBehaviour {
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody2D;
    BoxCollider2D boxCol;
    public float spearSpeed = 8;
    public float travelAngle = 0;
    GameObject playerShip;

    void pickRendererLayer()
    {
        /*if(transform.position.y < playerShip.transform.position.y)
        {
            spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
        else
        {
            spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }*/
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void Start () {
        boxCol = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShip = GameObject.Find("PlayerShip");
        transform.rotation = Quaternion.Euler(0, 0, travelAngle);
    }
	
	void Update () {
        transform.position += new Vector3(Mathf.Cos(travelAngle * Mathf.Deg2Rad), Mathf.Sin(travelAngle * Mathf.Deg2Rad), 0) * Time.deltaTime * spearSpeed;
        pickRendererLayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        boxCol.enabled = false;

        if(collision.gameObject.tag == "playerHitBox")
        {
            playerShip.GetComponent<PlayerScript>().amountDamage += 200;
        }

        spearSpeed = 0;
        animator.SetTrigger("SpearBreak");
        Destroy(this.gameObject, 0.667f / 4f);
    }
}
