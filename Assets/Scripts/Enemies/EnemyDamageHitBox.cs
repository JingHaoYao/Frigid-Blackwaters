using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHitBox : MonoBehaviour
{
    GameObject playerShip;
    public int damageAmount;

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(damageAmount, this.gameObject);
        }
    }
}
