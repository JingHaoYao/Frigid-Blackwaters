using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonsBreathApplyBurn : MonoBehaviour {
    public GameObject fireBurn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14 || collision.gameObject.layer == 10)
        {
            GameObject spawnedFlame = Instantiate(fireBurn, collision.gameObject.transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);
            Enemy targetEnemy = collision.gameObject.GetComponent<Enemy>();
            spawnedFlame.GetComponent<EnemyStatusEffect>().targetEnemy = targetEnemy;
            targetEnemy.addStatus(spawnedFlame.GetComponent<EnemyStatusEffect>());
        }
    }
}
