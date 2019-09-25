using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingOverlay : MonoBehaviour {
    Animator animator;
    bool loadIn = false;

	void Start () {
        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().playerDead = true;
        animator = GetComponent<Animator>();
	}

	void Update () {
        if (
            GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>().spawnPeriod >= 6.5f
            && loadIn == false
            )
        {
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().playerDead = false;
            loadIn = true;
            animator.SetTrigger("FadeOut");
            Destroy(this.gameObject, 1f);
        }
    }
}
