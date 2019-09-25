using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserFrostMageMissile : MonoBehaviour
{
    Animator animator;
    Collider2D col;
    Rigidbody2D rigidBody2D;
    float attackPeriod = 0;
    public bool cw = false;
    float angleTravel = 0;
    public float travelSpeed = 6;
    Vector3 center;
    public GameObject frostParticles;
    bool collided = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        center = transform.position;
    }

    void circle()
    {
        float circleAngle;
        float angleAway = Mathf.Atan2(transform.position.y - center.y, transform.position.x - center.x);
        if (cw == true)
        {
            circleAngle = (270 + Mathf.Atan2(center.y - transform.position.y, center.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
        else
        {
            circleAngle = (450 + Mathf.Atan2(center.y - transform.position.y, center.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
        rigidBody2D.velocity = new Vector3(Mathf.Cos(circleAngle * Mathf.Deg2Rad) + Mathf.Cos(angleAway) * 0.25f, Mathf.Sin(circleAngle * Mathf.Deg2Rad) + Mathf.Sin(angleAway) * 0.25f, 0).normalized * travelSpeed;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg);
        Instantiate(frostParticles, transform.position, Quaternion.Euler(0, 0, angleTravel + 90));
    }
    
    void Update()
    {
        if(attackPeriod < 4.5f)
        {
            if (collided == false)
            {
                attackPeriod += Time.deltaTime;
                if (attackPeriod > 0.5f)
                {
                    circle();
                }
            }
        }
        else
        {
            if (collided == false)
            {
                collided = true;
                animator.SetTrigger("Explode");
                this.GetComponent<AudioSource>().Play();
                Destroy(this.gameObject, 0.5f);
                col.enabled = false;
                rigidBody2D.velocity = Vector3.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collided == false)
        {
            this.GetComponent<AudioSource>().Play();
            if (collision.gameObject.tag == "playerHitBox")
            {
                FindObjectOfType<PlayerScript>().amountDamage += 250;
            }
            this.GetComponent<AudioSource>().Play();
            collided = true;
            animator.SetTrigger("Explode");
            Destroy(this.gameObject, 0.5f);
            rigidBody2D.velocity = Vector3.zero;
        }
    }
}
