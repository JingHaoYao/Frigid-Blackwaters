using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSeeker : MonoBehaviour
{
    Animator animator;
    float speed = 5;
    public float angleTravel;
    GameObject playerShip;
    Camera mainCamera;
    private bool wallCol;
    public int damageDealing;
    bool entryWaited = false;
    public GameObject particles;
    float followPeriod = 0;

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
    }

    void Update()
    {
        float angleToShip = Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x);
        if (wallCol == false && entryWaited == true)
        {
            if (followPeriod < 1.5f)
            {
                angleTravel = angleToShip;
            }
            transform.rotation = Quaternion.Euler(0, 0, angleTravel * Mathf.Rad2Deg);
            transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel), 0);
            Instantiate(particles, transform.position, Quaternion.Euler(0, 0, (angleTravel * Mathf.Rad2Deg) + 90));
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
                    playerShip.GetComponent<PlayerScript>().amountDamage += damageDealing;
                }
                wallCol = true;
                animator.SetTrigger("Dissipate");
                Destroy(this.gameObject, 0.417f);
            }
        }
    }
}
