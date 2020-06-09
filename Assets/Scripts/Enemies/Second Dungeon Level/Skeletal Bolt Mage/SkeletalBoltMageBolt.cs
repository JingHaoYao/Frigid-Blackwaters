using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalBoltMageBolt : MonoBehaviour
{
    Animator animator;
    float speed = 10;
    public float angleTravel;
    GameObject playerShip;
    Camera mainCamera;
    private bool wallCol;
    float dissipatePeriod = 0;
    public int damageDealing;
    bool entryWaited = false;
    public GameObject particles;

    void setEntryWaitedTrue()
    {
        entryWaited = true;
        animator.SetTrigger("Bolt");
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        playerShip = GameObject.Find("PlayerShip");
        Invoke("setEntryWaitedTrue", 0.5f);
    }

    void Update()
    {
        dissipatePeriod += Time.deltaTime;
        if (wallCol == false && entryWaited == true)
        {
            transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel), 0);
            Instantiate(particles, transform.position, Quaternion.Euler(0, 0, (angleTravel * Mathf.Rad2Deg) + 90));
            speed -= Time.deltaTime * 5.7f;
            if (speed < 0)
            {
                speed = 0;
            }
        }

        if (dissipatePeriod > 1.75f && wallCol == false)
        {
            this.GetComponent<AudioSource>().Play();
            wallCol = true;
            animator.SetTrigger("Dissipate");
            Destroy(this.gameObject, 0.417f);
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
