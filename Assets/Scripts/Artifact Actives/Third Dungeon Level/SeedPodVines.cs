using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPodVines : MonoBehaviour
{
    [SerializeField] Collider2D damageCollider;
    void Start()
    {
        damageCollider.enabled = false;
        StartCoroutine(damageProcedure());
    }

    IEnumerator damageProcedure()
    {
        yield return new WaitForSeconds(3 / 12f);
        damageCollider.enabled = true;
        yield return new WaitForSeconds(4 / 12f);
        damageCollider.enabled = false;
        yield return new WaitForSeconds(3f);
        destroyVines();
    }

    void destroyVines()
    {
        LeanTween.alpha(this.gameObject, 0, 1f).setOnComplete(() => Destroy(this.gameObject));
    }
}
