using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroStoneFlameBurst : MonoBehaviour
{
    [SerializeField] Collider2D flameBurstCollider;
    [SerializeField] ProjectileParent projectileParent;

    public void Initialize(GameObject spawner)
    {
        this.projectileParent.instantiater = spawner;
        StartCoroutine(flameBurstLoop());
    }

    IEnumerator flameBurstLoop()
    {
        flameBurstCollider.enabled = false;
        yield return new WaitForSeconds(4 / 12f);
        flameBurstCollider.enabled = true;

        yield return new WaitForSeconds(3 / 12f);

        flameBurstCollider.enabled = false;
        yield return new WaitForSeconds(5 / 12f);

        Destroy(this.gameObject);
    }
}
