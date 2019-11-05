using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathlyAuraParticles : MonoBehaviour {
    public GameObject target;
    float speed = 2.5f;
    public GameObject particles;
    float period = 0;

	void Start () {
		
	}

	void Update () {
        if (target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            float angleToTarget = (360 + (Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x) * Mathf.Rad2Deg)) % 360;
            transform.position += new Vector3(Mathf.Cos(angleToTarget * Mathf.Deg2Rad), Mathf.Sin(angleToTarget * Mathf.Deg2Rad), 0) * speed * Time.deltaTime;
            period += Time.deltaTime;
            if(period > 0.05f)
            {
                period = 0;
                Instantiate(particles, transform.position, Quaternion.Euler(0, 0, angleToTarget + 90));
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == target)
        {
            Destroy(this.gameObject);
        }
    }
}
