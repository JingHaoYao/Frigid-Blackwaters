using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpentWave : MonoBehaviour {
    Animator animator;
    public float angleTravel = 0;
    float speed = 6;
    bool animationStarted = false;
    PlayerScript playerScript;
    public GameObject waterFoam;
    float foamTimer = 0;

    void spawnFoam()
    {
        if (speed > 0)
        {
            if (animationStarted == false)
            {
                float whatAngle = angleTravel;
                foamTimer += Time.deltaTime;
                if (foamTimer >= 0.05f * speed / 3f)
                {
                    foamTimer = 0;
                    GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
                }
            }
        }
    }

    void Start () {
        animator = GetComponent<Animator>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
	}

	void Update () {
        
		if(Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > 7 || Mathf.Abs(transform.position.y - Camera.main.transform.position.y) > 7)
        {
            if(animationStarted == false)
            {
                animationStarted = true;
                animator.SetTrigger("Submerge");
                Destroy(this.gameObject, 0.5f);
                this.GetComponent<Collider2D>().enabled = false;
            }
        }
        else
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0) * Time.deltaTime * speed;
        }
        spawnFoam();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "playerHitBox" && animationStarted == false)
        {
            playerScript.amountDamage += 200;
            animationStarted = true;
            animator.SetTrigger("Submerge");
            Destroy(this.gameObject, 0.5f);
            this.GetComponent<Collider2D>().enabled = false;
            speed = 0;
        }        
    }
}
