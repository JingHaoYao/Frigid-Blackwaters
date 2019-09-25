using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBomb : MonoBehaviour {
    bool detonated = false;
    Animator animator;
    public GameObject explosion, waterSplash;
    CapsuleCollider2D capsulCol;
    GameObject spawnedExplosion;

    IEnumerator detonateIn()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(detonate());
    }

    IEnumerator detonate()
    {
        animator.SetTrigger("Detonate");
        yield return new WaitForSeconds(3 / 12f);
        spawnedExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        spawnedExplosion.transform.parent = this.transform;
        yield return new WaitForSeconds(0.417f);
        animator.SetTrigger("Sink");
        yield return new WaitForSeconds(4 / 12f);
        Instantiate(waterSplash, transform.position, Quaternion.identity);
        capsulCol.enabled = false;
        yield return new WaitForSeconds(2 / 12f);
        this.transform.parent.GetComponent<SpellBombs>().numSpellBombs--;
        Destroy(this.gameObject);
    }

	void Start () {
        animator = GetComponent<Animator>();
        capsulCol = GetComponent<CapsuleCollider2D>();
        StartCoroutine(detonateIn());
	}

	void Update () {
        this.GetComponent<SpriteRenderer>().sortingOrder = 200 - (int)(transform.position.y * 10) - 1;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.gameObject.layer == 15 || collision.gameObject.tag == "MeleeEnemy" || collision.gameObject.tag == "RangedEnemy" || collision.gameObject.tag == "EnemyShield") && detonated == false)
        {
            detonated = true;
            StartCoroutine(detonate());
        }       
    }
}
