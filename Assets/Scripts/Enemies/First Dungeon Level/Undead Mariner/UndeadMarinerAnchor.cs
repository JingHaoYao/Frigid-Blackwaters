using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadMarinerAnchor : MonoBehaviour
{
    public UndeadMariner mariner;
    public float attackingAngle;
    public bool returnToSender = false;
    public Vector3 returnPosition;
    float speed = 0;
    GameObject playerShip;
    bool hitShip = false;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, attackingAngle + 90);

        if (speed < 50)
        {
            speed += Time.deltaTime * 75;
        }

        if (returnToSender == false)
        {
            transform.position += new Vector3(Mathf.Cos(attackingAngle * Mathf.Deg2Rad), Mathf.Sin(attackingAngle * Mathf.Deg2Rad)) * speed * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(Mathf.Cos((attackingAngle + 180) * Mathf.Deg2Rad), Mathf.Sin((attackingAngle + 180) * Mathf.Deg2Rad)) * speed * Time.deltaTime;
            if (hitShip == true)
            {
                playerShip.transform.position += new Vector3(Mathf.Cos((attackingAngle + 180) * Mathf.Deg2Rad), Mathf.Sin((attackingAngle + 180) * Mathf.Deg2Rad)) * speed * Time.deltaTime;
            }

            if (Vector2.Distance(returnPosition, transform.position) < 0.5f || Mathf.Abs(((360 + Mathf.Atan2(transform.position.y - returnPosition.y, transform.position.x - returnPosition.x) * Mathf.Rad2Deg) % 360) - attackingAngle) > 10)
            {
                if (hitShip == true)
                {
                    playerShip.GetComponent<PlayerScript>().shipRooted = false;
                }
                Destroy(this.gameObject);
            }
        }

        if(Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > 7 || Mathf.Abs(transform.position.y - Camera.main.transform.position.y) > 7)
        {
            returnToSender = true;
            speed = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerScript playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
            this.GetComponent<AudioSource>().Play();
            playerScript.amountDamage += 100;
            playerScript.shipRooted = true;
            hitShip = true;
            returnToSender = true;
            speed = 0;
        }
    }
}
