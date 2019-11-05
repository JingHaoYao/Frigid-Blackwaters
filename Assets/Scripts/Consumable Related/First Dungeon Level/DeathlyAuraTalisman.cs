using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathlyAuraTalisman : MonoBehaviour {
    ConsumableBonus consumableBonus;
    bool activated = false;
    public GameObject deathlyAuraBall;

    void spawnBall()
    {
        Instantiate(deathlyAuraBall, GameObject.Find("PlayerShip").transform.position + new Vector3(0, 2, 0), Quaternion.identity);
    }

	void Start () {
        consumableBonus = GetComponent<ConsumableBonus>();
	}

	void Update () {
		if(consumableBonus.consumableActivated == true && activated == false)
        {
            spawnBall();
            activated = true;
        }
	}
}
