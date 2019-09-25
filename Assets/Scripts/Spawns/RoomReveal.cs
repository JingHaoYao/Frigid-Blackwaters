using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomReveal : MonoBehaviour {
    Animator animator;

    void setTrigger()
    {
        animator.SetTrigger("animate");
    }

	void Start () {
        animator = GetComponent<Animator>();
        Invoke("setTrigger", 0.2f);
	}
}
