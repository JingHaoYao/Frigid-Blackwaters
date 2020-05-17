using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalEngineerMineExplosion : MonoBehaviour
{
    [SerializeField] CircleCollider2D circCol;

    private void Start()
    {
        StartCoroutine(explode());
    }

    IEnumerator explode()
    {
        yield return new WaitForSeconds(2 / 12f);
        circCol.enabled = true;
        yield return new WaitForSeconds(2 / 12f);
        circCol.enabled = false;
        yield return new WaitForSeconds(3 / 12f);
        Destroy(this.gameObject);
    }
}
