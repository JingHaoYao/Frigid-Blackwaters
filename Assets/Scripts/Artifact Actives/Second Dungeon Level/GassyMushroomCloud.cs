using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GassyMushroomCloud : MonoBehaviour
{
    PolygonCollider2D polygonCol;
    void Start()
    {
        polygonCol = GetComponent<PolygonCollider2D>();
        polygonCol.enabled = false;
        StartCoroutine(gasCloud());
    }

    IEnumerator gasCloud()
    {
        yield return new WaitForSeconds(16f / 60f);
        polygonCol.enabled = true;
        yield return new WaitForSeconds(8f / 60f);
        polygonCol.enabled = false;
    }
}
