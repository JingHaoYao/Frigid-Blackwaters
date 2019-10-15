using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalShieldHitBox : MonoBehaviour {
    BoxCollider2D shieldBoxCol;

	void Start () {
        shieldBoxCol = transform.parent.gameObject.GetComponent<BoxCollider2D>();
	}

	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
            transform.parent.gameObject.GetComponent<Enemy>().dealDamage(damageDealt);
            transform.parent.gameObject.GetComponent<SkeletalShield>().actualHit = true;
        }
    }
}
