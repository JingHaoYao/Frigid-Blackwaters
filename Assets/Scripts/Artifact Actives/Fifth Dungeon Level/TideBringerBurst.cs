using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TideBringerBurst : MonoBehaviour
{
    [SerializeField] CircleCollider2D circCol;

    private void Start()
    {
        StartCoroutine(tideSplashRoutine());
    }

    IEnumerator tideSplashRoutine()
    {
        circCol.enabled = false;
        yield return new WaitForSeconds(4 / 12f);
        circCol.enabled = true;
        yield return new WaitForSeconds(3 / 12f);
        circCol.enabled = false;
        yield return new WaitForSeconds(7 / 12f);
        Destroy(this.gameObject);
    }
}
