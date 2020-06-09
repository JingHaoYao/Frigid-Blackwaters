using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPulse : MonoBehaviour {
    GameObject playerShip;
    private float damageTimer;
    CapsuleCollider2D col;

	void Start () {
        playerShip = GameObject.Find("PlayerShip");
        col = GetComponent<CapsuleCollider2D>();
	}

	void Update () {
        damageTimer += Time.deltaTime;
		if(damageTimer > 1f / 12f)
        {
            if(col.enabled == false)
            {
                col.enabled = true;
            }
            else{
                col.enabled = false;
            }
            damageTimer = 0;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(50, this.gameObject);
        }
    }
}
