using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageNumbersUI : MonoBehaviour
{
    public GameObject enemyDamageUIs;

    public void addEnemyDamageUI(int damageAmount, GameObject enemy)
    {
        GameObject instant = Instantiate(enemyDamageUIs, Camera.main.WorldToScreenPoint(enemy.transform.position + new Vector3(0, 1.5f, 0)), Quaternion.identity);
        instant.GetComponent<EnemyDamageNumbers>().showDamage(damageAmount, enemy);
        instant.transform.SetParent(transform);
    }
}
