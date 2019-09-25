using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkRocket : PlayerProjectile {
    public float speed = 50;
    public float angleTravel;
    public GameObject bulletTrail;
    GameObject playerShip;
    Vector3 initCameraPos;
    public GameObject explosion;
    Animator animator;
    int whatColor;

    float pickDirectionTravel()
    {
        GameObject cursor = FindObjectOfType<CursorTarget>().gameObject;
        return (360 + Mathf.Atan2(cursor.transform.position.y - transform.position.y, cursor.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void pickColor()
    {
        whatColor = Random.Range(0, 3);
        if(whatColor == 0)
        {
            animator.SetTrigger("Red Rocket");
        }
        else if(whatColor == 1)
        {
            animator.SetTrigger("Blue Rocket");
        }
        else
        {
            animator.SetTrigger("Green Rocket");
        }
    }

    void pickExplosionColor(GameObject explosion)
    {
        if (whatColor == 0)
        {
            explosion.GetComponent<Animator>().SetTrigger("Red Explosion");
        }
        else if (whatColor == 1)
        {
            explosion.GetComponent<Animator>().SetTrigger("Blue Explosion");
        }
        else
        {
            explosion.GetComponent<Animator>().SetTrigger("Green Explosion");
        }
    }

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        initCameraPos = Camera.main.transform.position;
        animator = GetComponent<Animator>();
        pickColor();
        angleTravel = pickDirectionTravel();
        transform.rotation = Quaternion.Euler(0, 0, angleTravel);
    }

    void Update()
    {
        transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0);
        Instantiate(bulletTrail, transform.position, Quaternion.Euler(0, 0, angleTravel + 90));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject instant = Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, Random.Range(1, 360)));
        pickExplosionColor(instant);
        Destroy(this.gameObject);
    }
}
