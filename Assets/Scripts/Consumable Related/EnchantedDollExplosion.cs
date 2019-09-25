using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantedDollExplosion : MonoBehaviour {
    CircleCollider2D circCol;

    IEnumerator explosion()
    {
        yield return new WaitForSeconds(3f / 12f);
        circCol.enabled = true;
        yield return new WaitForSeconds(4f / 12f);
        circCol.enabled = false;
        yield return new WaitForSeconds(5f / 12f);
        Destroy(this.gameObject);
    }

	void Start () {
        circCol = GetComponent<CircleCollider2D>();
        StartCoroutine(explosion());
	}
}
