using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHitBox : MonoBehaviour
{
    public int damageAmount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().amountDamage += damageAmount;
        }
    }
}
