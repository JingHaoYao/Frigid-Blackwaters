using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunicFireCircle : MonoBehaviour {
    GameObject playerShip;
    CircleCollider2D circCol;
    public GameObject smallRunicFire;

    IEnumerator turnOffCol()
    {
        yield return new WaitForSeconds((15 / 12) / 3);
        circCol.enabled = false;
    }

	void Start () {
        Destroy(this.gameObject, 1.75f / 3);
        StartCoroutine(turnOffCol());
        playerShip = GameObject.Find("PlayerShip");
        circCol = GetComponent<CircleCollider2D>();
	}

	void Update () {
        transform.position = playerShip.transform.position;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 15 || collision.gameObject.tag == "MeleeEnemy" || collision.gameObject.tag == "RangedEnemy" || collision.gameObject.tag == "EnemyShield"))
        {
            GameObject spawnedFlame = Instantiate(smallRunicFire, collision.gameObject.transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);
            spawnedFlame.GetComponent<SmallRunicFire>().targetObject = collision.gameObject;
        }
    }
}
