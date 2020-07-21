using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastKnightBeam : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCol;
    [SerializeField] ProjectileParent projectileParent;
    Transform enemy;

    public void Initialize(GameObject bossEnemy, float angleAttackInDeg)
    {
        transform.rotation = Quaternion.Euler(0, 0, angleAttackInDeg);
        projectileParent.instantiater = bossEnemy;
        enemy = bossEnemy.transform;
        StartCoroutine(beamProcedure());
    }

    private void Update()
    {
        transform.position = enemy.position;
    }

    IEnumerator beamProcedure()
    {
        yield return new WaitForSeconds(3 / 12f);
        boxCol.enabled = true;
        yield return new WaitForSeconds(4 / 12f);
        boxCol.enabled = false;
        yield return new WaitForSeconds(4 / 12f);
        Destroy(this.gameObject);
    }
}
