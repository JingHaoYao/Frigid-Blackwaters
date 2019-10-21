using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunRound : MonoBehaviour {
    public float speed = 50;
    public GameObject bulletSparks, secondImpactEffect;
    public float angleTravel;
    public GameObject bulletTrail;
    GameObject playerShip;
    Vector3 initCameraPos;
    public bool piercing = false;
    public GameObject bulletImpact;


    void Start () {
        playerShip = GameObject.Find("PlayerShip");
        initCameraPos = Camera.main.transform.position;
        if (PlayerUpgrades.spreadshotUpgrades.Count >= 2)
        {
            this.GetComponent<DamageAmount>().damage += 1;
        }
        Destroy(this.gameObject, 0.3f);
    }

    void Update()
    {
        transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0);
        Instantiate(bulletTrail, transform.position, Quaternion.Euler(0, 0, angleTravel + 90));
        if (Mathf.Abs(transform.position.x - initCameraPos.x) > 10 || Mathf.Abs(transform.position.y - initCameraPos.y) > 10)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RoomHitbox" || collision.gameObject.tag == "RoomWall" || collision.gameObject.tag == "EnemyShield")
        {
            Instantiate(bulletSparks, transform.position, Quaternion.Euler(0, 0, (angleTravel * Mathf.Rad2Deg) + 90));
            if (secondImpactEffect)
            {
                Instantiate(secondImpactEffect, transform.position, Quaternion.Euler(0, 0, (angleTravel * Mathf.Rad2Deg) + 90));
            }
            Destroy(this.gameObject);
        }
        else
        {
            Instantiate(bulletImpact, transform.position, Quaternion.Euler(0, 0, angleTravel + 90));
        }

        if (piercing == false)
        {
            Destroy(this.gameObject);
        }
    }
}
