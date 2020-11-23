using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireReaperSwipe : MonoBehaviour
{
    [SerializeField] PolygonCollider2D collider2D;

    private void Start()
    {
        StartCoroutine(sweepRoutine());
    }

    IEnumerator sweepRoutine()
    {
        collider2D.enabled = false;

        yield return new WaitForSeconds(3 / 12f);

        collider2D.enabled = true;

        yield return new WaitForSeconds(3 / 12f);

        collider2D.enabled = false;

        yield return new WaitForSeconds(3 / 12f);
        Destroy(this.gameObject);
    }
}
