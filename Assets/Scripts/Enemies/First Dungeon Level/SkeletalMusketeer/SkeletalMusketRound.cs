using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalMusketRound : MonoBehaviour {
    public float speed = 50;
    public GameObject bulletSparks;
    public float angleTravel;
    public GameObject bulletTrail;
    GameObject playerShip;

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
    }

    void Update()
    {
        transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel), 0);
        Instantiate(bulletTrail, transform.position, Quaternion.Euler(0, 0, angleTravel * Mathf.Rad2Deg + 90));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            playerShip.GetComponent<PlayerScript>().amountDamage += 100;
        }

        if (collision.gameObject.tag == "RoomHitbox" || collision.gameObject.tag == "RoomWall")
        {
            Instantiate(bulletSparks, transform.position, Quaternion.Euler(0, 0, (angleTravel * Mathf.Rad2Deg) + 90));
            Destroy(this.gameObject);
        }
    }
}
