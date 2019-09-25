using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderFrostMageIceMissle : MonoBehaviour
{
    public float angleTravel;
    float speed = 0;
    public float speedCap = 0;
    bool collided = false;
    Animator animator;
    PolygonCollider2D polyCol;
    GameObject playerShip;
    float angleToShip;
    float animPeriod = 0.5f;
    bool animDone = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        polyCol = GetComponent<PolygonCollider2D>();
        playerShip = GameObject.Find("PlayerShip");
    }

    void Update()
    {
        angleToShip = (360 + (Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg)) % 360;

        if(animPeriod > 0)
        {
            animPeriod -= Time.deltaTime;
        }
        else
        {
            if(animDone == false)
            {
                animDone = true;
                speedCap = 20;
                angleTravel = angleToShip;
                polyCol.enabled = true;
            }
        }

        if (speed < speedCap)
        {
            speed += Time.deltaTime * speedCap * 5;
        }

        if (animDone == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, angleToShip);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, angleTravel);
        }

        if(collided == false)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RoomHitbox" && collided == false)
        {
            this.GetComponent<AudioSource>().Play();
            collided = true;
            animator.SetTrigger("Explode");
            Destroy(this.gameObject, 0.5f);
            polyCol.enabled = false;
        }

        if (collision.gameObject.tag == "playerHitBox")
        {
            playerShip.GetComponent<PlayerScript>().amountDamage += 250;
        }
    }
}
