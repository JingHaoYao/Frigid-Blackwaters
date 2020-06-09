using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBombExplosion : MonoBehaviour
{
    CircleCollider2D circCol;

    IEnumerator explosion()
    {
        yield return new WaitForSeconds(2f / 12f);
        circCol.enabled = true;
        yield return new WaitForSeconds(2f / 12f);
        circCol.enabled = false;
        yield return new WaitForSeconds(3f / 12f);
        Destroy(this.gameObject);
    }

    void Start()
    {
        circCol = GetComponent<CircleCollider2D>();
        StartCoroutine(explosion());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(300, this.gameObject);
        }
    }
}
