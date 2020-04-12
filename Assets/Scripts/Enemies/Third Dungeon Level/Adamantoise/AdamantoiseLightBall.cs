using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamantoiseLightBall : MonoBehaviour
{
    public float destroyTime;
    public string breakString;
    public float speed;

    Animator animator;
    bool impacted = false;
    GameObject playerShip;
    // In degrees
    public float angleTravel;
    Camera mainCamera;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerShip = PlayerProperties.playerShip;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (impacted == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, angleTravel);
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;

            if (Mathf.Abs(transform.position.x - mainCamera.transform.position.x) > 9 || Mathf.Abs(transform.position.y - mainCamera.transform.position.y) > 9)
            {
                impacted = true;
                animator.SetTrigger(breakString);
                this.GetComponent<AudioSource>().Play();
                Destroy(this.gameObject, destroyTime);
                this.GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
