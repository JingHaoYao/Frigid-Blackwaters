using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkmanDamageHitBox : MonoBehaviour {
    GameObject playerShip;

	void Start () {
        playerShip = GameObject.Find("PlayerShip");
	}

	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(200, this.gameObject);
        }
    }
}
