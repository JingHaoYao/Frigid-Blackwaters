using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodExplosion : PlayerProjectile
{
    [SerializeField] CircleCollider2D circCol;

    IEnumerator turnOnCollider()
    {
        yield return new WaitForSeconds(2f / 12f);
        circCol.enabled = true;
        yield return new WaitForSeconds(1f / 12f);
        circCol.enabled = false;
    }

    void Start()
    {
        circCol.enabled = false;
        Destroy(this.gameObject, 0.667f);
        StartCoroutine(turnOnCollider());
    }
}
