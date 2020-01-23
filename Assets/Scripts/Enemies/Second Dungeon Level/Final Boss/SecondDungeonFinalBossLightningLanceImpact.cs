using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDungeonFinalBossLightningLanceImpact : MonoBehaviour
{
    [SerializeField] PolygonCollider2D damageCollider;

    private void Start()
    {
        StartCoroutine(damageTick());
    }

    IEnumerator damageTick()
    {
        yield return new WaitForSeconds(2 / 12f);
        damageCollider.enabled = true;
        yield return new WaitForSeconds(2 / 12f);
        damageCollider.enabled = false;
        yield return new WaitForSeconds(4 / 12f);
        Destroy(this.gameObject);
    }
}
