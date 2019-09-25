using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestIndicator : MonoBehaviour {
    public GameObject parentChest;
    Vector3 startPos, thisStartPos;

	void Start () {
        startPos = parentChest.transform.position;
        thisStartPos = transform.position;
	}

	void Update () {
        transform.position = thisStartPos + (parentChest.transform.position - startPos);
	}
}
