using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadFishMan : MonoBehaviour {
    Animator animator;
    public SpriteRenderer spriteRenderer;
    public int whatView = 0;
    public GameObject waterSplash;

    IEnumerator spawnWaterSplash()
    {
        yield return new WaitForSeconds(5 / 12f);
        GameObject splash = Instantiate(waterSplash, transform.position, Quaternion.identity);
        splash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder - 1;
    }

    void chooseAnim()
    {
        if(whatView == 1)
        {
            animator.SetTrigger("1Death");
        }
        else if(whatView == 2)
        {
            animator.SetTrigger("2Death");
        }
        else if(whatView == 3)
        {
            animator.SetTrigger("3Death");
        }
        else if(whatView == 4)
        {
            animator.SetTrigger("4Death");
        }
    }

	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Destroy(this.gameObject, 0.5f);
        StartCoroutine(spawnWaterSplash());
        chooseAnim();
	}

	void Update () {
		
	}
}
