using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHotCannonBallBurn : MonoBehaviour {
    CircleCollider2D circCol;
    public GameObject targetObject;
    Animator animator;
    public int amountTickDamage;

    IEnumerator tickDamage()
    {
        for(int i = 0; i < amountTickDamage; i++)
        {
            yield return new WaitForSeconds(0.3f);
            circCol.enabled = true;
            if (targetObject == null)
            {
                Destroy(this.gameObject);
            }
            yield return new WaitForSeconds(0.3f);
            circCol.enabled = false;
            if (targetObject == null)
            {
                Destroy(this.gameObject);
            }
        }
        Destroy(this.gameObject);
    }

    void Start()
    {
        circCol = GetComponent<CircleCollider2D>();
        circCol.enabled = false;
        animator = GetComponent<Animator>();
        StartCoroutine(tickDamage());
    }

    void Update()
    {
        if (targetObject != null)
        {
            transform.position = targetObject.transform.position + new Vector3(0, 0.3f, 0);
            this.GetComponent<SpriteRenderer>().sortingOrder = targetObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
