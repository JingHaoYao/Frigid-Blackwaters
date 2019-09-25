using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearWaterCrash : MonoBehaviour {
    BoxCollider2D boxCol;

    private void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        Destroy(this.gameObject, 0.583f);
        Invoke("turnOffBoxCol", 2f / 12f);
    }

    void turnOffBoxCol()
    {
        boxCol.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().amountDamage += 250;
        }
    }
}
