using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMinorDamageSplash : MonoBehaviour
{
    CircleCollider2D damageCollider;
    [SerializeField] int damageAmount = 300;

    IEnumerator damageTick()
    {
        yield return new WaitForSeconds(1f / 12f);
        damageCollider.enabled = true;
        yield return new WaitForSeconds(2f / 12f);
        damageCollider.enabled = false;
    }

    void Start()
    {
        damageCollider = GetComponent<CircleCollider2D>();
        damageCollider.enabled = false;
        StartCoroutine(damageTick());
        Destroy(this.gameObject, 0.667f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(damageAmount, this.gameObject);
        }
    }
}
