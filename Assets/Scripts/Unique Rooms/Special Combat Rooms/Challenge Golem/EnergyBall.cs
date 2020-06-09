using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour {
    public float angleTravel;
    Animator animator;
    bool impacted = false;
    GameObject playerShip;
    public int travelSpeed = 10;
    float particleTimer = 0;
    public GameObject blastParticles;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        transform.rotation = Quaternion.Euler(0, 0, angleTravel);
    }

    void Update()
    {
        if (impacted == false)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0) * Time.deltaTime * travelSpeed;
        }

        particleTimer += Time.deltaTime;
        if(particleTimer >= 0.05 && impacted == false)
        {
            GameObject particles = Instantiate(blastParticles, transform.position, Quaternion.Euler(0, 0, angleTravel + 90));
            particleTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(400, this.gameObject);
        }

        if (impacted == false && collision.gameObject.layer != 15)
        {
            impacted = true;
            animator.SetTrigger("Impact");
            this.GetComponent<AudioSource>().Play();
            Destroy(this.gameObject, 0.5f / 2f);
            this.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
