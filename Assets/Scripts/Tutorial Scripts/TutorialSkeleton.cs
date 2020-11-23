using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSkeleton : Enemy
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource deathAudio;
    private TutorialManager tutorialManager;

    public override void damageProcedure(int damage)
    {
        
    }

    public void Initialize(TutorialManager manager)
    {
        this.tutorialManager = manager;
    }

    private void Start()
    {
        if (Random.Range(0, 2) == 1)
        {
            transform.localScale = new Vector3(-5, 5);
        }
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
        tutorialManager.spawnedSkeletons.Remove(this.gameObject);
        animator.SetTrigger("Death");
        deathAudio.Play();
        Destroy(this.gameObject, 8 / 12f);
    }
}
