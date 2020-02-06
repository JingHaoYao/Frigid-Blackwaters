using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerSummonEffect : MonoBehaviour {
    public GameObject skelePirate1, skelePirate2, skelePirate3;
    GameObject[] skelePirateList;
    SpriteRenderer spriteRenderer;
    GameObject playerShip;

    void pickRendererLayer()
    {
        /*if (transform.position.y < playerShip.transform.position.y)
        {
            spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
        else
        {
            spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }*/
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    IEnumerator spawnSkele()
    {
        yield return new WaitForSeconds(9 / 12f);
        GameObject enemyInstant = Instantiate(skelePirateList[Random.Range(0, 3)], transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
        if (enemyInstant.GetComponent<Enemy>()) {
            EnemyPool.addEnemy(enemyInstant.GetComponent<Enemy>());
        }
    }

	void Start () {
		skelePirateList = new GameObject[3] { skelePirate1, skelePirate2, skelePirate3 };
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShip = GameObject.Find("PlayerShip");
        Destroy(this.gameObject, 14f / 12f);
        StartCoroutine(spawnSkele());
    }

	void Update () {
        pickRendererLayer();
	}
}
