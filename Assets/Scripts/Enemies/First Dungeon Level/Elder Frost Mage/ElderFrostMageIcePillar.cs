using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderFrostMageIcePillar : MonoBehaviour
{
    public Collider2D emergeHitBox, explodeHitBox;
    Animator animator;
    PolygonCollider2D solidHitBox;
    public bool explodeIcePillar = false;
    bool setExplode = false;

    IEnumerator emergeAttack()
    {
        yield return new WaitForSeconds(1f / 18f);
        emergeHitBox.enabled = true;
        yield return new WaitForSeconds(3f / 18f);
        emergeHitBox.enabled = false;
    }

    IEnumerator explodeAttack()
    {
        animator.SetTrigger("Explode");
        this.GetComponent<AudioSource>().Play();
        Destroy(this.gameObject, 0.583f);
        yield return new WaitForSeconds(2f / 12f);
        explodeHitBox.enabled = true;
        yield return new WaitForSeconds(2f / 12f);
        explodeHitBox.enabled = false;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        solidHitBox = GetComponent<PolygonCollider2D>();
        StartCoroutine(emergeAttack());
    }

    void Update()
    {
        if(setExplode == false && explodeIcePillar == true)
        {
            setExplode = true;
            StartCoroutine(explodeAttack());
        }
    }
}
