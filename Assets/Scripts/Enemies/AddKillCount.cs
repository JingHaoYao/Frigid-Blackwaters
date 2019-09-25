using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddKillCount : MonoBehaviour {
    public int killNumber = 1;

	void Awake () {
        GameObject.Find("PlayerShip").GetComponent<Artifacts>().numKills += killNumber;
	}
}
