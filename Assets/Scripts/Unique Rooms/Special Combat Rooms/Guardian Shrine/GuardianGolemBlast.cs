using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianGolemBlast : MonoBehaviour {
    public float speed = 12;
    public float angleTravel = 0;
    Animator animator;
    bool collided = false;

	void Start () {
        animator = GetComponent<Animator>();
	}

	void Update () {
        if(collided == false)
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0) * Time.deltaTime * speed;
        if ((Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > 8.5f || Mathf.Abs(transform.position.y - Camera.main.transform.position.y) > 8.5f) && collided == false)
        {
            this.GetComponent<AudioSource>().Play();
            collided = true;
            animator.SetTrigger("Dissipate");
            Destroy(this.gameObject, .5f);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collided == false && collision.gameObject.tag == "playerHitBox")
        {
            collided = true;
            PlayerProperties.playerScript.dealDamageToShip(200, this.gameObject);
            this.GetComponent<AudioSource>().Play();
            animator.SetTrigger("Dissipate");
            Destroy(this.gameObject, .5f);
        }      
    }
}
