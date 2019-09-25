using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTower : MonoBehaviour {
    GambleTowers gambleTowers;
    Animator animator;

	void Start () {
        gambleTowers = transform.parent.GetComponent<GambleTowers>();
        animator = GetComponent<Animator>();
	}

    public void activateWin()
    {
        animator.SetTrigger("Win");
    }

    public void activateLose()
    {
        animator.SetTrigger("Lose");
    }

    public void activateDormant()
    {
        animator.SetTrigger("Completed");
    }

	void Update () {
		
	}
}
