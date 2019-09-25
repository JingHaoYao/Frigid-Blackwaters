using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSummoningEffect : MonoBehaviour {
    public GameObject[] skeleList;

    IEnumerator spawnSkele()
    {
        yield return new WaitForSeconds(9 / 12f);
        Instantiate(skeleList[Random.Range(0, skeleList.Length)], transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
    }

    void Start () {
        StartCoroutine(spawnSkele());
	}

	void Update () {
		
	}
}
