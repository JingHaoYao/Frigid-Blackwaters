using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemMeleeHitBox : MonoBehaviour {
    GameObject playerShip;

	void Start () {
        playerShip = GameObject.Find("PlayerShip");
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == playerShip)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(600, this.gameObject);
        }
    }
}
