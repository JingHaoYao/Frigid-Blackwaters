using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeGolemLaserBlast : MonoBehaviour {
    public float angleTravel;
    Animator animator;
    bool impacted = false;
    GameObject playerShip;
    public int travelSpeed = 8;
    float particleTimer = 0;
    public GameObject blastParticles;
    public bool checkPos = true;

    void Start () {
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        transform.rotation = Quaternion.Euler(0, 0, angleTravel);
	}

	void Update () {
		if(impacted == false)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0) * Time.deltaTime * travelSpeed;
        }

        particleTimer += Time.deltaTime;
        if (particleTimer >= 0.025 && impacted == false)
        {
            GameObject particles = Instantiate(blastParticles, transform.position, Quaternion.Euler(0, 0, angleTravel + 90));
            particleTimer = 0;
        }
    }

    bool checkPosition()
    {
        if(transform.position.y > Camera.main.transform.position.y + 8)
        {
            return true;
        }
        if(transform.position.y < Camera.main.transform.position.y - 8)
        {
            return true;
        }
        if(transform.position.x > Camera.main.transform.position.x + 8)
        {
            return true;
        }
        if(transform.position.x < Camera.main.transform.position.x - 8)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(300, this.gameObject);
        }

        if (impacted == false && collision.gameObject.layer != 15 && (checkPosition() == true || checkPos == false))
        {
            impacted = true;
            animator.SetTrigger("Impact");
            this.GetComponent<AudioSource>().Play();
            Destroy(this.gameObject, 0.667f/2f);
            this.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
