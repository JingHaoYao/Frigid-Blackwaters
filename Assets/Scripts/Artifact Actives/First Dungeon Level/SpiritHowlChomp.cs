using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritHowlChomp : MonoBehaviour {
    CapsuleCollider2D capsulCol;

    IEnumerator enabledCapsulCol()
    {
        yield return new WaitForSeconds(2 / 12f);
        capsulCol.enabled = true;
    }

	void Start () {
        Destroy(this.gameObject, 0.333f);
        capsulCol = GetComponent<CapsuleCollider2D>();
        capsulCol.enabled = false;
        StartCoroutine(enabledCapsulCol());
	}

	void Update () {
		
	}
}
