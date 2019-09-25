using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBombExplosion : MonoBehaviour {
    CapsuleCollider2D capCol;

    IEnumerator explode()
    {
        yield return new WaitForSeconds(1 / 12f);
        capCol.enabled = true;
        yield return new WaitForSeconds(2 / 12f);
        capCol.enabled = false;
    }

	void Start () {
        Destroy(this.gameObject, 0.417f);
        capCol = this.GetComponent<CapsuleCollider2D>();
        capCol.enabled = false;
        StartCoroutine(explode());
   	}

	void Update () {
        if (transform.parent != null)
        {
            transform.position = this.transform.parent.gameObject.transform.position;
            this.GetComponent<SpriteRenderer>().sortingOrder = transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
	}
}
