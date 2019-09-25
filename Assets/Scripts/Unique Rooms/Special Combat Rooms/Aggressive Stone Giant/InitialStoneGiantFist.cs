using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialStoneGiantFist : MonoBehaviour {
    UnactivatedGiant unactivatedGiant;

	void Start () {
        unactivatedGiant = GetComponentInParent<UnactivatedGiant>();	
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (unactivatedGiant.engaged == false && collision.gameObject.layer == 16 && Vector2.Distance(Camera.main.transform.position, transform.parent.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer.gameObject.transform.position) < 3)
        {
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = false;
            unactivatedGiant.engaged = true;
            unactivatedGiant.spawnEnemy();
            //unactivatedGiant.gameObject.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer.spawnDoorSeals();
        }
    }
}
