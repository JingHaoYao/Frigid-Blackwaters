using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGolemChest : MonoBehaviour {
    public GameObject golemChest;

	void Start () {
        if (transform.position.y < Camera.main.transform.position.y)
        {
            Instantiate(golemChest, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(golemChest, transform.position + new Vector3(0, -3, 0), Quaternion.identity);
        }
	}

	void Update () {
		
	}
}
