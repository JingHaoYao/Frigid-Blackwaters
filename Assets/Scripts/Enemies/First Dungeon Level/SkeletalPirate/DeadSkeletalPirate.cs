using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSkeletalPirate : MonoBehaviour {
    Animator animator;
    public SpriteRenderer spriteRenderer;
    public int whatView = 0;
    public GameObject waterSplash;

    IEnumerator spawnWaterSplash()
    {
        yield return new WaitForSeconds(8 / 12f);
        GameObject splash = Instantiate(waterSplash, transform.position, Quaternion.identity);
        splash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder - 1;
    }

    void chooseAnim()
    {
        if (whatView == 1)
        {
            animator.SetTrigger("Death1");
        }
        else if (whatView == 2)
        {
            animator.SetTrigger("Death2");
        }
        else if (whatView == 3)
        {
            animator.SetTrigger("Death3");
        }
        else if (whatView == 4)
        {
            animator.SetTrigger("Death4");
        }
    }

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Destroy(this.gameObject, 10f/12f);
        StartCoroutine(spawnWaterSplash());
        chooseAnim();
    }

	void Update () {
		
	}
}
