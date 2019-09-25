using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinMine : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    GameObject explosionHitBox;
    bool explode;
    float mineTimer = 0;

    void bounce()
    {
        animator.SetTrigger("Bounce");
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        explosionHitBox = transform.GetChild(0).gameObject;
        explosionHitBox.SetActive(false);
        Invoke("bounce", 8f / 12f);
    }

    IEnumerator explodeMine()
    {
        explode = true;
        for(int i = 0; i < 4; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        this.GetComponent<AudioSource>().Play();
        animator.SetTrigger("Explode");
        yield return new WaitForSeconds(3f / 12f);
        explosionHitBox.SetActive(true);
        yield return new WaitForSeconds(1 / 12f);
        explosionHitBox.SetActive(false);
        yield return new WaitForSeconds(4f / 12f);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        mineTimer += Time.deltaTime;
        if(mineTimer >= 4 && explode == false)
        {
            StartCoroutine(explodeMine());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (explode == false && collision.gameObject.layer != 15)
        {
            StartCoroutine(explodeMine());
        }
    }
}
