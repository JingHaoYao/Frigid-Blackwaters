using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrustaceaKingCrystal : MonoBehaviour
{
    [SerializeField] PolygonCollider2D polyCol;
    [SerializeField] Animator animator;
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] GameObject damageHitbox;
    bool destroyed = false;
    private CrustaceaKing boss;

    public void initializeCrystal(CrustaceaKing boss)
    {
        this.boss = boss;
        projectileParent.instantiater = boss.gameObject;
    }

    void Start()
    {
        polyCol = GetComponent<PolygonCollider2D>();
        StartCoroutine(crystalShardRoutine());
        transform.localScale = new Vector3(Random.Range(0, 2) * -10 + 5, 5);
    }

    IEnumerator crystalShardRoutine()
    {
        yield return new WaitForSeconds(2 / 12f);
        damageHitbox.SetActive(true);
        yield return new WaitForSeconds(2 / 12f);
        damageHitbox.SetActive(false);
        yield return new WaitForSeconds(3/12);
        polyCol.enabled = true;
        yield return new WaitForSeconds(10f);

        if (destroyed == false)
        {
            StartCoroutine(destroy());
        }
    }

    IEnumerator destroy()
    {
        destroyed = true;
        animator.SetTrigger("Sink");
        polyCol.enabled = false;
        yield return new WaitForSeconds(6/12f);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (destroyed == false)
        {
            StartCoroutine(destroy());

            if (collision.gameObject == this.projectileParent.instantiater)
            {
                this.boss.crystalDamage();
            }
        }
    }
}
