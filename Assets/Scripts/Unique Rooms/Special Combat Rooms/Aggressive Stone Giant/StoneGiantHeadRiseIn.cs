using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGiantHeadRiseIn : MonoBehaviour {
    public GameObject stoneGiantEnemy;
    public AntiSpawnSpaceDetailer anti;
    
    void spawnGiantEnemy()
    {
        GameObject instant = Instantiate(stoneGiantEnemy, transform.position, Quaternion.identity);
        instant.GetComponent<StoneGiant>().anti = anti;
    }

	void Start () {
        Invoke("spawnGiantEnemy", 0.5f);
        Invoke("turnOffRenderer", 0.5f);
        Destroy(this.gameObject, 2f);
	}

    void turnOffRenderer()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

	void Update () {
		
	}
}
