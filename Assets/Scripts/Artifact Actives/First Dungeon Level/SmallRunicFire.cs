using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallRunicFire : MonoBehaviour {
    CircleCollider2D circCol;
    public GameObject targetObject;
    Animator animator;
    bool setDisappear = false;

    IEnumerator tickDamage()
    {
        yield return new WaitForSeconds(2);
        if (targetObject != null)
        {
            circCol.enabled = true;
            animator.SetTrigger("FadeOut");
            setDisappear = true;
            Destroy(this.gameObject, 0.417f);
        }
    }

	void Start () {
        circCol = GetComponent<CircleCollider2D>();
        circCol.enabled = false;
        animator = GetComponent<Animator>();
        StartCoroutine(tickDamage());
	}

	void Update () {
		if(targetObject != null)
        {
            transform.position = targetObject.transform.position + new Vector3(0, 0.3f, 0);
            this.GetComponent<SpriteRenderer>().sortingOrder = targetObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
        else
        {
            if(setDisappear == false)
            {
                setDisappear = true;
                animator.SetTrigger("FadeOut");
                Destroy(this.gameObject, 0.417f);
            }
        }
	}
}
