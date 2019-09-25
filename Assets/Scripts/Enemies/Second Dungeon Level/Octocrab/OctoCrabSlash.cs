using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoCrabSlash : MonoBehaviour
{
    float travelSpeed = 10;
    public float angleTravel = 0;
    float maxDuration = 0.8f, currDuration = 0;
    Animator animator;
    Collider2D collider;
    bool endTravel = false;
    float endingSize = 2f, startingSize = 0.5f;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        currDuration += Time.deltaTime;
        transform.localScale = new Vector3(Mathf.Lerp(endingSize, startingSize, currDuration / maxDuration), Mathf.Lerp(startingSize, endingSize, currDuration / maxDuration));
        travelSpeed = Mathf.Lerp(10, 1, currDuration / maxDuration);
        if(currDuration >= maxDuration && endTravel == false)
        {
            endTravel = true;
            collider.enabled = false;
            animator.SetTrigger("Dissipate");
            Destroy(this.gameObject, 0.5f);
        }

        if(currDuration < maxDuration && endTravel == false)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0) * Time.deltaTime * travelSpeed;
        }

        if((Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > 8 || Mathf.Abs(transform.position.y - Camera.main.transform.position.y) > 8) && endTravel == false)
        {
            endTravel = true;
            collider.enabled = false;
            animator.SetTrigger("Dissipate");
            Destroy(this.gameObject, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            FindObjectOfType<PlayerScript>().amountDamage += Mathf.RoundToInt((currDuration/maxDuration) * 500);
        }
    }
}
