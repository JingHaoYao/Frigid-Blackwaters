using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeMageProjectile : MonoBehaviour
{
    public GameObject target;

    Animator animator;
    public float speed = 8;

    bool collided = false;
    float initialAngle = 0;
    float radius = 0;


    float timer = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        initialAngle = Mathf.Atan2(transform.position.y - target.transform.position.y, transform.position.x - target.transform.position.x);
        radius = Vector2.Distance(target.transform.position, transform.position);
        this.GetComponent<CircleCollider2D>().enabled = false;
    }

    void Update()
    {

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Skeletal Charge Idle"))
        {
            if (this.GetComponent<CircleCollider2D>().enabled == false)
            {
                this.GetComponent<CircleCollider2D>().enabled = true;
            }
        }

        if(collided == false)
        {
            initialAngle += Time.deltaTime * (speed / (2 * Mathf.PI * radius));

            transform.position = target.transform.position + new Vector3(Mathf.Cos(initialAngle), Mathf.Sin(initialAngle)) * radius;
        }

        if (timer > 8)
        {
            if (collided == false)
            {
                collided = true;
                animator.SetTrigger("Impact");
                Destroy(this.gameObject, 0.417f);
                this.GetComponent<AudioSource>().Play();
                this.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            collided = true;
            animator.SetTrigger("Impact");
            Destroy(this.gameObject, 0.417f);
            this.GetComponent<AudioSource>().Play();
            this.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
