using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnemoneShot : MonoBehaviour
{
    Animator animator;
    public float speed = 7;
    public float angleTravel;
    GameObject playerShip;
    Camera mainCamera;
    private bool wallCol;
    public int amountDamage = 75;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        playerShip = GameObject.Find("PlayerShip");
        Invoke("destroyBall", 0.6f);
    }

    void Update()
    {
        if (wallCol == false)
        {
            transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel), 0);
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
        if (collision.gameObject.tag == "playerHitBox")
        {
            playerShip.GetComponent<PlayerScript>().amountDamage += amountDamage;
        }

        if (wallCol == false)
        {
            speed = 0;
            wallCol = true;
            animator.SetTrigger("Dissipate");
            this.GetComponent<AudioSource>().Play();
            Destroy(this.gameObject, (4 / 12f));
        }
    }

    void destroyBall()
    {
        this.GetComponent<Collider2D>().enabled = false;
        speed = 0;
        wallCol = true;
        animator.SetTrigger("Dissipate");
        this.GetComponent<AudioSource>().Play();
        Destroy(this.gameObject, (4 / 12f));
    }
}
