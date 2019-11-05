using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreedSoulParticles : MonoBehaviour {
    float period = 0;
    public GameObject particles;
    GameObject playerShip;

	void Start () {
        Destroy(this.gameObject, 2f);
        playerShip = GameObject.Find("PlayerShip");
	}

    void circlePlayerShip()
    {
        float circleAngle;
        circleAngle = (450 + Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        transform.position += new Vector3(Mathf.Cos(circleAngle * Mathf.Deg2Rad), Mathf.Sin(circleAngle * Mathf.Deg2Rad), 0) * Time.deltaTime * 2; 
    }

    void Update () {
        period += Time.deltaTime;
        circlePlayerShip();
        if (period > 0.05f)
        {
            period = 0;
            Instantiate(particles, transform.position, Quaternion.Euler(0, 0, 180));
        }
    }
}
