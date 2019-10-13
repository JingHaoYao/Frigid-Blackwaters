using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabSpitterShot : MonoBehaviour
{
    Animator animator;
    float speed = 4;
    public float angleTravel;
    GameObject playerShip;
    Camera mainCamera;
    private bool wallCol;
    float dissipatePeriod = 0;
    public int damageDealing;
    public GameObject smallSpitterShot;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        playerShip = GameObject.Find("PlayerShip");
    }

    void summonSmallSpitterShot()
    {
        for(int i = 0; i < 8; i++)
        {
            GameObject smallShot = Instantiate(smallSpitterShot, transform.position, Quaternion.identity);
            smallShot.GetComponent<ProjectileParent>().instantiater = this.GetComponent<ProjectileParent>().instantiater;
            smallShot.GetComponent<AnemoneShot>().angleTravel = (i * 45) * Mathf.Deg2Rad;
        }
    }

    void Update()
    {
        dissipatePeriod += Time.deltaTime;
        if (wallCol == false)
        {
            transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel), 0);
            speed -= Time.deltaTime * 0.8f;
            if (speed < 0)
            {
                speed = 0;
            }
        }

        if (dissipatePeriod > 5f && wallCol == false)
        {
            this.GetComponent<AudioSource>().Play();
            wallCol = true;
            summonSmallSpitterShot();
            animator.SetTrigger("Dissipate");
            Destroy(this.gameObject, 0.333f);
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
            this.GetComponent<AudioSource>().Play();
            if (collision.gameObject.tag == "playerHitBox")
            {
                playerShip.GetComponent<PlayerScript>().amountDamage += damageDealing;
            }
            wallCol = true;
            animator.SetTrigger("Dissipate");
            Destroy(this.gameObject, 0.333f);
        }
    }
}
