using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMarker : MonoBehaviour {

	void Start () {
		
	}

	void Update () {
        transform.localScale = new Vector3(0.2f, 0.2f, 0);
        transform.localRotation = Quaternion.Euler(Vector3.zero);
	}
}
