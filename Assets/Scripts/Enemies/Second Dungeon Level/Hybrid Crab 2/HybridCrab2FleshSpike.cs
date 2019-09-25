using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridCrab2FleshSpike : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    public Sprite regularSpike;
    public GameObject bloodShot;
    float attackPeriod = 1f;
    int numberAttacks = 0;

    IEnumerator attackRise()
    {
        this.GetComponents<AudioSource>()[0].Play();
        yield return new WaitForSeconds(6f / 12f);
        animator.enabled = false;
        spriteRenderer.sprite = regularSpike;
    }

    IEnumerator attack()
    {
        animator.enabled = true;
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(4f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        for(int i = 0; i < 8; i++)
        {
            float angle = i * 45;
            GameObject instantShot = Instantiate(bloodShot, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            instantShot.GetComponent<AnemoneShot>().angleTravel = angle * Mathf.Deg2Rad;
            instantShot.GetComponent<ProjectileParent>().instantiater = GetComponent<ProjectileParent>().instantiater;
        }
        yield return new WaitForSeconds(5f / 12f);
        animator.enabled = false;
        spriteRenderer.sprite = regularSpike;
        numberAttacks++;
    }

    IEnumerator sinkSpike()
    {
        this.GetComponents<AudioSource>()[0].Play();
        animator.enabled = true;
        animator.SetTrigger("Sink");
        yield return new WaitForSeconds(6f / 12f);
        foreach(Collider2D col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        StartCoroutine(attackRise());
    }

    void Update()
    {
        if (numberAttacks < 3)
        {
            if (attackPeriod > 0)
            {
                attackPeriod -= Time.deltaTime;
            }
            else
            {
                attackPeriod = 1;
                StartCoroutine(attack());
            }
        }
        else
        {
            if(animator.enabled == false)
            {
                StartCoroutine(sinkSpike());
            }
        }
    }
}
