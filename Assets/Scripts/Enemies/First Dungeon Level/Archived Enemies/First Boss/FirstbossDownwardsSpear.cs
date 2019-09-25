using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstbossDownwardsSpear : MonoBehaviour {
    public float speed = 8;
    public Vector3 target = Vector3.zero;
    public GameObject particles;
    public GameObject waterCrash;

	void Update () {
        Instantiate(particles, transform.position, Quaternion.identity);
        transform.position += Vector3.down * speed * Time.deltaTime;
        if(Vector2.Distance(transform.position + new Vector3(0, -5.4f, 0),target) < 0.3f || transform.position.y < target.y)
        {
            Destroy(this.gameObject);
            Instantiate(waterCrash, target, Quaternion.identity);
        }
	}
}
