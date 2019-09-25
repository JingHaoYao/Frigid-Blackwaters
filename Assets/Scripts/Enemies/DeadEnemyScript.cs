using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEnemyScript : MonoBehaviour {
    Animator animator;
    public SpriteRenderer spriteRenderer;
    public int whatView = 0;
    public GameObject waterSplash;
    public float whenSpawnSplash = 0;
    public float whenDestroy = 0;

    IEnumerator spawnWaterSplash()
    {
        yield return new WaitForSeconds(whenSpawnSplash);
        GameObject splash = Instantiate(waterSplash, transform.position, Quaternion.identity);
        splash.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder - 2;
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
        else if(whatView == 5)
        {
            animator.SetTrigger("Death5");
        }
        else if(whatView == 6)
        {
            animator.SetTrigger("Death6");
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Destroy(this.gameObject, whenDestroy);
        StartCoroutine(spawnWaterSplash());
        chooseAnim();
        if (this.GetComponent<AudioSource>())
        {
            this.GetComponent<AudioSource>().Play();
        }
    }

    void Update()
    {

    }
}
