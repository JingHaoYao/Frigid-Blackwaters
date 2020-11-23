using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour {
    GameObject playerShip;
    PlayerScript playerScript;
    public Animator animator;

	void Start () {
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
	}

	void Update () {
        transform.position = playerShip.transform.position;
        this.GetComponent<SpriteRenderer>().sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
        transform.rotation = Quaternion.Euler(0, 0, playerScript.whatAngleTraveled);
	}
}
