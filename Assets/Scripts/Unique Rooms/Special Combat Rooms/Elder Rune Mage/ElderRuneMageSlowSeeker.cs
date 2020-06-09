using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderRuneMageSlowSeeker : MonoBehaviour
{
    Animator animator;
    float speed = 6.5f;
    public float angleTravel;
    public float circleAngle;
    GameObject playerShip;
    Camera mainCamera;
    private bool wallCol;
    public int damageDealing;
    bool entryWaited = false;
    public GameObject particles;
    float followPeriod = 0;
    int clockWise = 0;

    void setEntryWaitedTrue()
    {
        entryWaited = true;
        animator.SetTrigger("Slow Seeker");
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        playerShip = GameObject.Find("PlayerShip");
        Invoke("setEntryWaitedTrue", 0.417f);
        clockWise = Random.Range(0, 2);
    }

    void Update()
    {
        float angleToShip = Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x);
        if (wallCol == false && entryWaited == true)
        {
            if (followPeriod < 4f)
            {
                if (clockWise == 0)
                {
                    circleAngle = angleToShip + Mathf.PI / 2;
                }
                else
                {
                    circleAngle = angleToShip - Mathf.PI / 2;
                }
                angleTravel = angleToShip;
            }
            float angleRotation = Mathf.Atan2((new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel), 0) + new Vector3(Mathf.Cos(circleAngle), Mathf.Sin(circleAngle))).y, (new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel), 0) + new Vector3(Mathf.Cos(circleAngle), Mathf.Sin(circleAngle))).x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angleRotation);
            transform.position += Time.deltaTime * speed * (new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel), 0) + new Vector3(Mathf.Cos(circleAngle), Mathf.Sin(circleAngle))).normalized;
            Instantiate(particles, transform.position, Quaternion.Euler(0, 0, angleRotation + 90));
            followPeriod += Time.deltaTime;
        }

        if (transform.position.x < mainCamera.transform.position.x - 11f || transform.position.x > mainCamera.transform.position.x + 11f || transform.position.y > mainCamera.transform.position.y + 11f || transform.position.y < mainCamera.transform.position.y - 11f)
        {
            if (wallCol == false)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (wallCol == false)
        {
            if (collision.gameObject.tag == "RoomHitbox" || collision.gameObject.tag == "playerHitBox")
            {
                this.GetComponent<AudioSource>().Play();
                if (collision.gameObject.tag == "playerHitBox")
                {
                    PlayerProperties.playerScript.dealDamageToShip(damageDealing, this.gameObject);
                }
                wallCol = true;
                animator.SetTrigger("Dissipate");
                Destroy(this.gameObject, 0.417f);
            }
        }
    }
}
