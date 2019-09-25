using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockRubble : MonoBehaviour
{
    Animator animator;
    bool broken = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 16)
        {
            if(collision.gameObject.GetComponent<DamageAmount>().damage > 4 && broken == false)
            {
                broken = true;
                FindObjectOfType<AudioManager>().PlaySound("Rock Rubble Break");
                animator.SetTrigger("Explode");
                Destroy(this.gameObject, 0.417f);
            }
        }
    }
}
