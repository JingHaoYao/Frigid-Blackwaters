using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBetaSlash : MonoBehaviour
{
    public float travelSpeed = 60;
    public float angleTravel = 0;
    public float duration = 0.1f;
    Animator animator;
    Collider2D collider;
    bool endTravel = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if(duration > 0)
        {
            duration -= Time.deltaTime;
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0) * Time.deltaTime * travelSpeed;
        }
        else
        {
            if(endTravel == false)
            {
                endTravel = true;
                collider.enabled = false;
                animator.SetTrigger("Dissipate");
                Destroy(this.gameObject, 0.5f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(1100, this.gameObject);
        }
    }
}
