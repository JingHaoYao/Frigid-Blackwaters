using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSummoningEffect : MonoBehaviour {
    public GameObject[] skeleList;

    IEnumerator spawnSkele()
    {
        yield return new WaitForSeconds(9 / 12f);
        GameObject instant = Instantiate(skeleList[Random.Range(0, skeleList.Length)], transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
        EnemyPool.addEnemy(instant.GetComponent<Enemy>());
    }

    void Start () {
        StartCoroutine(spawnSkele());
	}

}
