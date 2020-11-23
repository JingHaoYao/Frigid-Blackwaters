using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisShrimpHitbox : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    public Transform mantisTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(damageAmount, this.gameObject);

            float angleAway = Mathf.Atan2(PlayerProperties.playerShipPosition.y - mantisTransform.position.y, PlayerProperties.playerShipPosition.x - mantisTransform.position.x);
            PlayerProperties.playerScript.setPlayerEnemyMomentum(new Vector3(Mathf.Cos(angleAway), Mathf.Sin(angleAway)) * 12, 1f);
        }
    }
}
