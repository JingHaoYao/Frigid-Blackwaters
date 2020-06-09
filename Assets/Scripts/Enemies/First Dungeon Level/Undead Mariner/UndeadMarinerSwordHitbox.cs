using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadMarinerSwordHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(800, this.gameObject);
        }
    }
}
