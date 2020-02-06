using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatteredRound : PlayerProjectile {
    public float speed = 50;
    public GameObject bulletSparks, secondImpactEffect;
    public float angleTravel;
    public GameObject bulletTrail;
    Vector3 initCameraPos;
    public GameObject impactEffect;

    void Start () {
        initCameraPos = Camera.main.transform.position;
        GetComponent<DamageAmount>().damage = Mathf.CeilToInt(GetComponent<DamageAmount>().damage / 2f);
    }

	void Update () {
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
            Instantiate(impactEffect, transform.position, Quaternion.Euler(0, 0, angleTravel + 90));
        }
    }
}
