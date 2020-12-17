using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSkeleton : Enemy
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource deathAudio;

    public override void damageProcedure(int damage)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<DamageAmount>())
        {
            dealDamage(collision.GetComponent<DamageAmount>().damage);
        }
    }

    public override void deathProcedure()
    {
        animator.SetTrigger("Death");
        deathAudio.Play();
        Destroy(this.gameObject, 8 / 12f);
    }
}
