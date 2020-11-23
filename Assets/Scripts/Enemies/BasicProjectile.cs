using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    public float destroyTime;
    public string breakString;
    public float speed;

    [SerializeField] bool rotateProjectile = true;
    [SerializeField] bool playSound = true;

    Animator animator;
    bool impacted = false;
    GameObject playerShip;
    // In degrees
    public float angleTravel;
    [SerializeField] float rotationOffset;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerShip = PlayerProperties.playerShip;
    }

    void Update()
    {
        if (impacted == false)
        {
            if (rotateProjectile)
            {
                transform.rotation = Quaternion.Euler(0, 0, angleTravel + rotationOffset);
            }
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && collision.gameObject.layer != 15)
        {
            impacted = true;
            animator.SetTrigger(breakString);
            if (playSound)
            {
                this.GetComponent<AudioSource>().Play();
            }
            Destroy(this.gameObject, destroyTime);
            this.GetComponent<Collider2D>().enabled = false;
        }
    }
}
