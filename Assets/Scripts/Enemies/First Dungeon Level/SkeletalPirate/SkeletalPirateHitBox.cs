using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalPirateHitBox : MonoBehaviour {
    GameObject playerShip;

	void Start () {
        playerShip = GameObject.Find("PlayerShip");
	}

	void Update () {
        
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == GameObject.Find("PlayerShip"))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            playerShip.GetComponent<PlayerScript>().amountDamage += 300;
        }
    }
}
