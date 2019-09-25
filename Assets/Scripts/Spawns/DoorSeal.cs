using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSeal : MonoBehaviour {
    BoxCollider2D boxCol;
    Animator animator;
    public bool open = false;
    private bool hasAnimated = false;
    public GameObject waterSplash;
    Camera camera;

    void spawnSplash()
    {
        if (transform.position.y > camera.transform.position.y + 5)
        {
            Instantiate(waterSplash, transform.position + new Vector3(0.4f, -1.2f, 0), Quaternion.Euler(0, 0, 270));
        }
        else if (transform.position.y < camera.transform.position.y - 5)
        {
            Instantiate(waterSplash, transform.position + new Vector3(0.4f, 1.2f, 0), Quaternion.Euler(0, 0, 90));
        }
        else if (transform.position.x > camera.transform.position.x + 5)
        {
            Instantiate(waterSplash, transform.position + new Vector3(-1.2f, 0.4f, 0), Quaternion.Euler(0, 0, 180));
        }
        else
        {
            Instantiate(waterSplash, transform.position + new Vector3(1.2f, 0.4f, 0), Quaternion.Euler(0, 0, 0));
        }
    }

    void disableWall()
    {
        boxCol.enabled = false;
    }

	void Start () {
        camera = Camera.main;
        animator = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        Invoke("spawnSplash", 1f / 12f);
    }

	void Update () {
		if(hasAnimated == false && open == true)
        {
            animator.SetTrigger("PhaseOut");
            Invoke("disableWall", 5f / 12f);
            Invoke("spawnSplash", 4f / 12f);
            Destroy(this.gameObject, 5f / 12f);
            hasAnimated = true;
        }
	}
}
