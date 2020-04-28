using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour {
    Inventory inventory;
    PlayerScript playerScript;

	void Start () {
        inventory = GameObject.Find("PlayerShip").GetComponent<Inventory>();
        playerScript = FindObjectOfType<PlayerScript>();
	}

	void Update () {
        /*if(inventory.inventory.activeSelf == false)
        {
            this.gameObject.SetActive(false);
        }*/
        if (Input.GetKeyDown(KeyCode.Escape) || playerScript.windowAlreadyOpen == false)
        {
            this.gameObject.SetActive(false);
        }
	}
}
