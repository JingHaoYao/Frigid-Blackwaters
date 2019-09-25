using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumBlast : MonoBehaviour {
    CannonRound cannonRound;
    PlayerScript playerScript;
    float momentumPeriod;
    public float mag, duration;

	void Start () {
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        cannonRound = GetComponent<CannonRound>();
        float momentumAngle = cannonRound.angleTravel + 180;

        playerScript.momentumVector = new Vector3(Mathf.Cos(momentumAngle * Mathf.Deg2Rad), Mathf.Sin(momentumAngle * Mathf.Deg2Rad), 0) * mag;
        playerScript.momentumMagnitude = mag;
        playerScript.momentumDuration = duration;
	}
}
