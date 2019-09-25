using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFoamSpawn : MonoBehaviour {
    Rigidbody2D rigidBody2D;
    private float foamTimer = 0;
    public GameObject waterFoam;

	void Start () {
        rigidBody2D = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
		if(rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if(foamTimer >= 0.05f)
            {
                foamTimer = 0;
                Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
	}
}
