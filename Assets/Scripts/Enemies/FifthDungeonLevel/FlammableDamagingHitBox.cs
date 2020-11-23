using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableDamagingHitBox : MonoBehaviour
{
    [SerializeField] private int damageAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(damageAmount, this.gameObject);
            PlayerProperties.flammableController.AddFlammableStack(this.gameObject);
        }
    }
}
