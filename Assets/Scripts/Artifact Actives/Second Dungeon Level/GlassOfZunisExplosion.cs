using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassOfZunisExplosion : MonoBehaviour
{
    CircleCollider2D circCol;
    public GameObject targetObject;

    void Start()
    {
        circCol = GetComponent<CircleCollider2D>();
        circCol.enabled = false;
        StartCoroutine(explode());
        Destroy(gameObject, 2.667f / 3);
    }

    private void Update()
    {
        transform.position = targetObject.transform.position + new Vector3(0, 0.7f, 0);
    }

    IEnumerator explode()
    {
        yield return new WaitForSeconds(6f / (12 * 3));
        circCol.enabled = true;
        yield return new WaitForSeconds(1f / 12f);
        circCol.enabled = false;
    }
}
