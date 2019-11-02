using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EverlastingGunpowderExplosion : MonoBehaviour
{
    CircleCollider2D circCol;
    void Start()
    {
        circCol = GetComponent<CircleCollider2D>();
        circCol.enabled = false;
        StartCoroutine(explode());
    }

    IEnumerator explode()
    {
        yield return new WaitForSeconds(2f / 12f);
        circCol.enabled = true;
        yield return new WaitForSeconds(1f / 12f);
        circCol.enabled = false;
        yield return new WaitForSeconds(4f / 12f);
        Destroy(this.gameObject);
    }
}
