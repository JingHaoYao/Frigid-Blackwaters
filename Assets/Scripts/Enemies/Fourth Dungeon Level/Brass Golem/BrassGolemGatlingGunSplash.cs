using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrassGolemGatlingGunSplash : MonoBehaviour
{
    [SerializeField] Collider2D collider;

    private void Start()
    {
        StartCoroutine(splashRoutine());
    }

    IEnumerator splashRoutine()
    {
        collider.enabled = false;
        yield return new WaitForSeconds(2 / 12f);
        collider.enabled = true;
        yield return new WaitForSeconds(2 / 12f);
        collider.enabled = true;
        yield return new WaitForSeconds(6 / 12f);
        Destroy(this.gameObject);
    }
}
