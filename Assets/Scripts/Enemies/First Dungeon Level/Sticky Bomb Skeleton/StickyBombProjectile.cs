using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBombProjectile : MonoBehaviour
{
    Animator animator;
    public float speed = 7;
    public float angleTravel;
    GameObject playerShip;
    Camera mainCamera;
    private bool wallCol;
    public GameObject explosion;
    GameObject hitObject;
    Vector3 hitPos, objectHitPos;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        playerShip = GameObject.Find("PlayerShip");
    }

    void Update()
    {
        if (wallCol == false)
        {
            transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel), 0);
        }
        else
        {
            if (hitObject != null)
            {
                transform.position = hitPos + (hitObject.transform.position - objectHitPos);
            }
        }

        if (transform.position.x < mainCamera.transform.position.x - 11f || transform.position.x > mainCamera.transform.position.x + 11f || transform.position.y > mainCamera.transform.position.y + 11f || transform.position.y < mainCamera.transform.position.y - 11f)
        {
            if (wallCol == false)
            {
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator spawnExplosion()
    {
        yield return new WaitForSeconds(4);
        Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.GetComponent<Collider2D>().enabled = false;
        if (collision.gameObject.tag == "playerHitBox")
        {
            playerShip.GetComponent<PlayerScript>().amountDamage += 50;
        }

        if (wallCol == false)
        {
            this.GetComponent<AudioSource>().Play();
            hitObject = collision.gameObject;
            hitPos = transform.position;
            objectHitPos = collision.transform.position;
            StartCoroutine(spawnExplosion());
            wallCol = true;
        }
    }
}
