using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonRound : PlayerProjectile
{
    public float speed = 50;
    public GameObject bulletSparks, secondImpactEffect;
    public float angleTravel;
    public GameObject bulletTrail;
    GameObject playerShip;
    Vector3 initCameraPos;
    public bool isRedHot = false;
    public GameObject burn;
    public GameObject bulletImpact;

    public float pickDirectionTravel()
    {
        float angleOrientation = GameObject.Find("PlayerShip").GetComponent<PlayerScript>().angleOrientation;
        float tempAngle = 0;
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            tempAngle = 45;
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            tempAngle = 90;
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            tempAngle = 135;
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            tempAngle = 180;
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            tempAngle = 225;
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {
            tempAngle = 270;
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            tempAngle = 315;
        }
        else
        {
            tempAngle = 0;
        }
        return tempAngle;
    }

    public void adjust(int whichWeapon)
    {
        if (whichWeapon == 2)
        {
            angleTravel -= 90;
        }
        else if (whichWeapon == 3)
        {
            angleTravel += 90;
        }
    }

    float pickDirectionTravel2()
    {
        GameObject cursor = FindObjectOfType<CursorTarget>().gameObject;
        return (360 + Mathf.Atan2(cursor.transform.position.y - transform.position.y, cursor.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        initCameraPos = Camera.main.transform.position;
        angleTravel = pickDirectionTravel();
        adjust(whichWeaponFrom);

        if (forceShot == false)
        {
            angleTravel = pickDirectionTravel2();
        }

        if (PlayerUpgrades.cannonUpgrades.Count >= 2)
        {
            this.GetComponent<DamageAmount>().damage += 1;
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
            Destroy(this.gameObject);
        }
        else
        {
            Instantiate(bulletImpact, transform.position, Quaternion.Euler(0, 0, angleTravel + 90));
        }

        if (isRedHot == true)
        {
            if (
                collision.gameObject.tag == "EnemyShield"
                || collision.gameObject.tag == "RangedEnemy"
                || collision.gameObject.tag == "MeleeEnemy"
                || collision.gameObject.tag == "StrongEnemy"
                )
            {
                GameObject instant = Instantiate(burn, collision.gameObject.transform.position, Quaternion.identity);
                instant.GetComponent<RedHotCannonBallBurn>().targetObject = collision.gameObject;
            }
        }
    }
}
