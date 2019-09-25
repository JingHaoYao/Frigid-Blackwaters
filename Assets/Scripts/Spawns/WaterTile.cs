using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : MonoBehaviour {

	void Start () {
		transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 90);
	}
}
