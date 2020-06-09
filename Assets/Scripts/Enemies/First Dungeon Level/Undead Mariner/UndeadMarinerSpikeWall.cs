using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadMarinerSpikeWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(150, this.gameObject);
        }
    }
}
