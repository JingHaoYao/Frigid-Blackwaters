using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSummoningEffect : MonoBehaviour {
    public GameObject[] skeleList;

    IEnumerator spawnSkele()
    {
        yield return new WaitForSeconds(9 / 12f);
        GameObject chosenEnemy = skeleList[Random.Range(0, skeleList.Length)];
        if(chosenEnemy == null)
        {
            Debug.Log("null enemy");
        }
        GameObject instant = Instantiate(chosenEnemy, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);

        EnemyPool.addEnemy(instant.GetComponent<Enemy>());

        Destroy(this.gameObject);
    }

    void Start () {
        StartCoroutine(spawnSkele());
	}

}
