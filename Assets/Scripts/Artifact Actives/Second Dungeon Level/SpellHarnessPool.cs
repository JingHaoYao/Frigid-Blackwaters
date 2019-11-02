using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHarnessPool : MonoBehaviour
{
    CapsuleCollider2D capsulCol;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Start()
    {
        capsulCol = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.Play("Spell Harness Damaging Pool", -1, Random.Range(0.0f, 1.0f));
        capsulCol.enabled = false;
        StartCoroutine(damageCol());
    }
    
    IEnumerator damageCol()
    {
        for(int i = 0; i < 4; i++)
        {
            capsulCol.enabled = true;
            yield return new WaitForSeconds(0.5f);
            capsulCol.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }

        while(spriteRenderer.color.a > 0)
        {
            float alphaVal = spriteRenderer.color.a;
            spriteRenderer.color = new Color(1, 1, 1, alphaVal -= Time.deltaTime);
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
