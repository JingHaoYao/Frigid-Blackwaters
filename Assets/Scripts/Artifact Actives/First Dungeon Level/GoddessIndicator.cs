using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoddessIndicator : MonoBehaviour {
    public Sprite[] fillIcons;
    public IdoloftheGoddess idolScript;
    Animator animator;
    SpriteRenderer spriteRenderer;
    bool animationEnded = false;
    bool setAnimate = false;

    IEnumerator endStuff()
    {
        animator.enabled = true;
        animator.SetTrigger("Completed");
        yield return new WaitForSeconds(1.5f);
        animator.SetTrigger("FadeOut");
        Destroy(this.gameObject, 0.667f);
    }

    IEnumerator startCounter()
    {
        yield return new WaitForSeconds(1.167f);
        animationEnded = true;
        animator.enabled = false;
    } 

	void Start () {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        FindObjectOfType<AudioManager>().PlaySound("Goddess Statue Started");
        StartCoroutine(startCounter());
	}

	void Update () {
        transform.position = GameObject.Find("PlayerShip").transform.position + new Vector3(0, 2.4f, 0);

		if(animationEnded == true)
        {
            if (GameObject.Find("PlayerShip").GetComponent<Artifacts>().numKills - idolScript.origKills <= 10)
            {
                spriteRenderer.sprite = fillIcons[GameObject.Find("PlayerShip").GetComponent<Artifacts>().numKills - idolScript.origKills];
            }
        }

        if(GameObject.Find("PlayerShip").GetComponent<Artifacts>().numKills - idolScript.origKills >= 10)
        {
            if(setAnimate == false)
            {
                StartCoroutine(endStuff());
                setAnimate = true;
                FindObjectOfType<AudioManager>().PlaySound("Goddess Statue Completed");
            }
        }
	}
}
