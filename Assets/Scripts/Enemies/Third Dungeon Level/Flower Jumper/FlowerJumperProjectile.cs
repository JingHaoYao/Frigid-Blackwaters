using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerJumperProjectile : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] Collider2D collider;
    [SerializeField] AudioSource endAudio;
    public float speed = 7;
    public float angleTravel;
    Camera mainCamera;
    private bool wallCol;
    public int amountDamage = 250;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        Invoke("destroyBall", 0.6f);
        transform.rotation = Quaternion.Euler(0, 0, angleTravel);
    }

    void Update()
    {
        if (wallCol == false)
        {
            transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0);
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
            PlayerProperties.playerScript.dealDamageToShip(amountDamage, this.gameObject);
        }

        if (wallCol == false)
        {
            destroyBall();
        }
    }

    void destroyBall()
    {
        this.GetComponent<Collider2D>().enabled = false;
        speed = 0;
        wallCol = true;
        collider.enabled = false;
        animator.SetTrigger("Decay");
        endAudio.Play();
        Destroy(this.gameObject, 0.75f);
    }
}
