using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBomb : MonoBehaviour
{
    public GameObject explosion;
    Rigidbody2D rigidBody2D;
    public float angleTravel = 0;
    float travelSpeed = 10;
    public GameObject waterFoam;
    float foamTimer = 0;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * (travelSpeed / 3f))
            {
                foamTimer = 0;
                Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(travelSpeed > 0)
        {
            rigidBody2D.velocity = new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * travelSpeed;
            travelSpeed -= Time.deltaTime * 3.5f;
            spawnFoam();
        }
        else
        {
            travelSpeed = 0;
            Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float angleBetween = (360 + Mathf.Atan2(transform.position.y - collision.transform.position.y, transform.position.x - collision.transform.position.x) * Mathf.Rad2Deg) % 360;
        this.GetComponent<AudioSource>().Play();
        if ((angleBetween > 45 && angleBetween < 135) || (angleBetween > 225 && angleBetween < 315))
        {
            angleTravel = (360 - angleTravel) % 360;
        }
        else
        {
            angleTravel = (360 + (180 - angleTravel)) % 360;
        }
    }
}
