using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShrimpShot : MonoBehaviour {
    Animator animator;
    public float speed = 7;
    public float angleTravel;
    public GameObject shotTrail;
    GameObject playerShip;
    Camera mainCamera;
    private bool wallCol;
    public GameObject impactSmoke;

    void Start() {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        playerShip = GameObject.Find("PlayerShip");
    }

    void Update() {
        transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel), 0);


        if (wallCol == false)
        {
            Instantiate(shotTrail, transform.position, Quaternion.Euler(0, 0, angleTravel * Mathf.Rad2Deg + 90));
        }

        if (transform.position.x < mainCamera.transform.position.x - 8.5f || transform.position.x > mainCamera.transform.position.x + 8.5f || transform.position.y > mainCamera.transform.position.y + 8.5f || transform.position.y < mainCamera.transform.position.y - 8.5f)
        {
            if (wallCol == false)
            {
                speed = 0;
                wallCol = true;
                this.GetComponent<AudioSource>().Play();
                animator.SetTrigger("Disperse");
                Instantiate(impactSmoke, transform.position, Quaternion.identity);
                Destroy(this.gameObject, (6f / 12f) / 1.5f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(150, this.gameObject);
        }
    }
}
