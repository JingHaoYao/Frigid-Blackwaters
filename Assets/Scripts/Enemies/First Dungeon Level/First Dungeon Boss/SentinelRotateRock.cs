using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelRotateRock : MonoBehaviour
{
    Animator animator;
    Animator circleAnimator;
    Rigidbody2D rigidBody2D;
    Collider2D hitCol;
    bool animated;
    public GameObject target;
    public float currSpeed = 0;
    public float targetSpeed = 10;
    int cw = 0;
    float rotationAngle = 0;
    float radius;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        hitCol = GetComponent<Collider2D>();
        circleAnimator = transform.GetChild(0).GetComponent<Animator>();
        hitCol.enabled = false;
        cw = Random.Range(0, 2);
        radius = Vector2.Distance(target.transform.position + new Vector3(0, 6, 0), transform.position);
        rotationAngle = Mathf.Atan2(transform.position.y - (target.transform.position.y + 6), transform.position.x - target.transform.position.x);
    }

    IEnumerator riseAnimation()
    {
        animator.SetTrigger("Rise");
        circleAnimator.SetTrigger("Rise");
        yield return new WaitForSeconds(0.833f);
        animated = true;
        hitCol.enabled = true;
    }

    public void fall()
    {
        animated = false;
        hitCol.enabled = false;
        animator.SetTrigger("Fall");
        circleAnimator.SetTrigger("Fall");
    }

    public void rise()
    {
        StartCoroutine(riseAnimation());
    }

    void Update()
    {
        if (animated)
        {
            if(currSpeed < targetSpeed)
            {
                currSpeed += Time.deltaTime * 20;
            }
            else
            {
                currSpeed = targetSpeed;
            }

            rotationAngle += Time.deltaTime * (currSpeed/(2 * Mathf.PI * radius));
            float directionAngle = (360 + (rotationAngle + Mathf.PI / 2) * Mathf.Rad2Deg) % 360;
            if (rotationAngle >= Mathf.PI * 2)
            {
                rotationAngle = 0;
            }

            if (target != null)
            {
                transform.position = target.transform.position + new Vector3(0, 6, 0) + new Vector3(Mathf.Cos(rotationAngle), Mathf.Sin(rotationAngle), 0) * radius;
            }
        }
    }
}
