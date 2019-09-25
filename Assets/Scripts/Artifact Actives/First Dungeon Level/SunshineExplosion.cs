using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunshineExplosion : MonoBehaviour {
	void Start () {
        Destroy(this.gameObject, 0.833f);
	}

	void Update () {
        transform.position = GameObject.Find("PlayerShip").transform.position;
	}
}
