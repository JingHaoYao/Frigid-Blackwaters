using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBombExplosion : MonoBehaviour
{
    CircleCollider2D circCol;
    void Start()
    {
        circCol = GetComponent<CircleCollider2D>();
        circCol.enabled = false;
        Destroy(this.gameObject, 6.833f / 5f);
        StartCoroutine(explode());
    }

    IEnumerator explode()
    {
        yield return new WaitForSeconds(4f / 12f);
        circCol.enabled = true;
        yield return new WaitForSeconds(2f / 12f);
    }
}
