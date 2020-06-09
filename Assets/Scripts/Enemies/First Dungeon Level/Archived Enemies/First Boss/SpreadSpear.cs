using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadSpear : MonoBehaviour {
    public float angleTravel = 0, speed = 20;
    public GameObject particles;

	void Update () {
		if(Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > 18f 
           || Mathf.Abs(transform.position.y - Camera.main.transform.position.y) > 18f)
        {
            Destroy(this.gameObject);
        }
        transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0) * Time.deltaTime * speed;
        Instantiate(particles, transform.position, Quaternion.Euler(0, 0, angleTravel + 90));
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(400, this.gameObject);
        }
    }
}
