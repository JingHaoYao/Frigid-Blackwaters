using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomeGuardiansMaceInstant : MonoBehaviour
{
    CircleCollider2D circCol;
    Animator animator;


    bool isDestroying = false;

    void Start()
    {
        circCol = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        circCol.enabled = false;
    }

    void Update()
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Tome Guardian's Mace Form") && isDestroying == false)
        {
            circCol.enabled = true;
        }
    }

    public void unequipArtifact()
    {
        isDestroying = true;
        circCol.enabled = false;
        animator.SetTrigger("Burst");
        GetComponent<AudioSource>().Play();
        Destroy(this.gameObject, 0.333f);
    }
}
