using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGolemPulse : MonoBehaviour {
    public GameObject playerShip;

	void Start () {
        playerShip = GameObject.Find("PlayerShip");
	}

	void Update () {
        transform.position = playerShip.transform.position;
	}
}
