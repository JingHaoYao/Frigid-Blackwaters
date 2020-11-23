using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingOverlay : MonoBehaviour {
    Animator animator;
    bool loadIn = false;
    RoomTemplates templates;

	void Start () {
        PlayerProperties.playerScript.playerDead = true;
        animator = GetComponent<Animator>();
        templates = GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>();
    }



	void Update () {
        if (
            templates.areRoomsSpawned()
            && loadIn == false
            )
        {
            PlayerProperties.playerScript.playerDead = false;
            loadIn = true;
            animator.SetTrigger("FadeOut");
            Destroy(this.gameObject, 1f);
        }
    }
}
