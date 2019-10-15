﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalScatterMageShot : MonoBehaviour
{
    Animator animator;
    float speed = 6;
    public float angleTravel;
    GameObject playerShip;
    Camera mainCamera;
    private bool wallCol;
    float dissipatePeriod = 0;
    public int damageDealing;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        playerShip = GameObject.Find("PlayerShip");
    }

    void Update()
    {
        dissipatePeriod += Time.deltaTime;
        if (wallCol == false)
        {
            transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel), 0);
            speed -= Time.deltaTime * 2f;
            if (speed < 0)
            {
                speed = 0;
            }
        }

        if (dissipatePeriod > 2.5f && wallCol == false)
        {
            this.GetComponent<AudioSource>().Play();
            wallCol = true;
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
            this.GetComponent<Collider2D>().enabled = false;
            this.GetComponent<AudioSource>().Play();
            if (collision.gameObject.tag == "playerHitBox")
            {
                playerShip.GetComponent<PlayerScript>().amountDamage += damageDealing;
            }
            wallCol = true;
            animator.SetTrigger("Dissipate");
            Destroy(this.gameObject, 0.5f);
        }
    }
}