using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterShot : PlayerProjectile {
    public float speed = 50;
    public GameObject bulletSparks, secondImpactEffect;
    public float angleTravel;
    public GameObject bulletTrail;
    GameObject playerShip;
    Vector3 initCameraPos;
    public bool fourScatters = false;
    public GameObject scatter;
    GameObject enemyHit;

    float pickDirectionTravel()
    {
        GameObject cursor = FindObjectOfType<CursorTarget>().gameObject;
        return (360 + Mathf.Atan2(cursor.transform.position.y - transform.position.y, cursor.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        angleTravel = pickDirectionTravel();
        initCameraPos = Camera.main.transform.position;
        if (PlayerUpgrades.musketUpgrades.Count >= 2)
        {
            this.GetComponent<DamageAmount>().damage += 1;
        }
    }

    void summonScatters()
    {
        if (fourScatters == false)
        {
            for(int i = 0; i < 2; i++)
            {
                GameObject instantScatter = Instantiate(scatter, transform.position, Quaternion.identity);
                instantScatter.GetComponent<ScatteredRound>().angleTravel = angleTravel - 10 + 20 * i;
                if(enemyHit != null)
                    Physics2D.IgnoreCollision(instantScatter.GetComponent<Collider2D>(), enemyHit.GetComponent<Collider2D>());
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject instantScatter = Instantiate(scatter, transform.position, Quaternion.identity);
                instantScatter.GetComponent<ScatteredRound>().angleTravel = angleTravel - 7.5f + 5 * i;
                if(enemyHit != null)
                    Physics2D.IgnoreCollision(instantScatter.GetComponent<Collider2D>(), enemyHit.GetComponent<Collider2D>());
            }
        }
    }

    void Update()
    {
        transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0);
        Instantiate(bulletTrail, transform.position, Quaternion.Euler(0, 0, angleTravel + 90));
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
        }
        else
        {
            enemyHit = collision.gameObject;
        }
        summonScatters();
        Destroy(this.gameObject);
    }
}
