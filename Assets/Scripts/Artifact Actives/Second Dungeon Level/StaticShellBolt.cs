using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticShellBolt : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    Animator animator;
    CircleCollider2D circCol;
    public float angleTravel;
    bool collided = false;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        circCol = GetComponent<CircleCollider2D>();
        transform.rotation = Quaternion.Euler(0, 0, angleTravel);
        rigidbody2D.velocity = new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * 6;
    }

    private void Update()
    {
        if (transform.position.x < Camera.main.transform.position.x - 11f || transform.position.x > Camera.main.transform.position.x + 11f || transform.position.y > Camera.main.transform.position.y + 11f || transform.position.y < Camera.main.transform.position.y - 11f)
        {
            if (collided == false)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RoomHitbox" || collision.gameObject.tag == "RoomWall" || collision.gameObject.tag == "EnemyShield")
        {
            if (collided == false)
            {
                rigidbody2D.velocity = Vector3.zero;
                transform.position += new Vector3(0, Mathf.Sin((angleTravel + 180) * Mathf.Deg2Rad)) * 0.7f; 
                animator.SetTrigger("Explode");
                Destroy(this.gameObject, 4f / 2.5f);
                collided = true;
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
