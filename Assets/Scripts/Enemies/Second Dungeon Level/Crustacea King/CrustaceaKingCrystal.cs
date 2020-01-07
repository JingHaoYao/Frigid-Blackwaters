using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrustaceaKingCrystal : MonoBehaviour
{
    PolygonCollider2D polyCol;
    bool destroyed = false;
    void Start()
    {
        polyCol = GetComponent<PolygonCollider2D>();
        StartCoroutine(crystalShardRoutine());
    }

    IEnumerator crystalShardRoutine()
    {
        yield return new WaitForSeconds(0.667f);
        polyCol.enabled = true;
        yield return new WaitForSeconds(12f);
        if (destroyed == false)
        {
            GetComponent<Animator>().SetTrigger("Shatter");
            GetComponent<AudioSource>().Play();
            polyCol.enabled = false;
            destroyed = true;
            yield return new WaitForSeconds(0.417f);
            Destroy(this.gameObject);
        }
    }

    IEnumerator destroy()
    {
        GetComponent<Animator>().SetTrigger("Shatter");
        GetComponent<AudioSource>().Play();
        polyCol.enabled = false;
        yield return new WaitForSeconds(0.417f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroyed == false)
        {
            destroyed = true;
            StartCoroutine(destroy());

            if (collision.gameObject.GetComponent<CrustaceaKing>() || collision.gameObject.transform.parent.GetComponent<CrustaceaKing>())
            {
                FindObjectOfType<CrustaceaKing>().crystalDamage();
            }
        }
    }
}
