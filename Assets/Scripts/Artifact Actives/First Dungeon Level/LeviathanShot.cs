using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeviathanShot : MonoBehaviour {
    public float speed = 100;
    public float angleTravel;
    public GameObject bulletTrail;
    GameObject playerShip;
    Animator animator;
    Vector3 initCameraPos;

    float pickDirectionTravel()
    {
        GameObject cursor = FindObjectOfType<CursorTarget>().gameObject;
        return (360 + Mathf.Atan2(cursor.transform.position.y - transform.position.y, cursor.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        animator = GetComponent<Animator>();
        angleTravel = pickDirectionTravel();
        transform.rotation = Quaternion.Euler(0, 0, angleTravel);
        initCameraPos = Camera.main.transform.position;
    }

    void Update()
    {
        transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0);
        if(speed > 0)
            Instantiate(bulletTrail, transform.position, Quaternion.Euler(0, 0, angleTravel + 90));

        if (Mathf.Abs(transform.position.x - initCameraPos.x) > 10 || Mathf.Abs(transform.position.y - initCameraPos.y) > 10)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RoomHitbox" || collision.gameObject.tag == "RoomWall" || collision.gameObject.tag == "EnemyShield")
        {
            FindObjectOfType<AudioManager>().PlaySound("Leviathan Cannon Impact");
            animator.SetTrigger("FadeOut");
            speed = 0;
            Destroy(this.gameObject, 0.333f);
        }
    }
}
