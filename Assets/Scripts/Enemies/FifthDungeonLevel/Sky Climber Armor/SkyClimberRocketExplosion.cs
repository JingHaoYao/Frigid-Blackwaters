using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyClimberRocketExplosion : MonoBehaviour
{
    [SerializeField] Collider2D collider2D;
    [SerializeField] ProjectileParent projectileParent;


    public void Initialize(GameObject summoningObject)
    {
        this.projectileParent.instantiater = summoningObject;
        StartCoroutine(explosionProcedure());
    }

    IEnumerator explosionProcedure()
    {
        collider2D.enabled = false;
        yield return new WaitForSeconds(4 / 12f);
        collider2D.enabled = true;
        yield return new WaitForSeconds(3 / 12f);
        collider2D.enabled = false;
        yield return new WaitForSeconds(5 / 12f);
        Destroy(this.gameObject);
    }
}
