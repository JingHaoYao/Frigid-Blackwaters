using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdWave : MonoBehaviour
{
    BoxCollider2D collider2D;

    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        StartCoroutine(damageTicks(4f));
    }

    IEnumerator damageTicks(float duration)
    {
        int numberTicks = Mathf.RoundToInt(duration / 0.2f) / 2;
        for(int i = 0; i < numberTicks; i++)
        {
            collider2D.enabled = true;
            yield return new WaitForSeconds(0.2f);
            collider2D.enabled = false;
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(this.gameObject, 1.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            FindObjectOfType<PlayerScript>().amountDamage += 50;
        }
    }
}
