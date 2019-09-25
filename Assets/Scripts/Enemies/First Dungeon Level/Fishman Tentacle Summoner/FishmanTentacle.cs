using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishmanTentacle : MonoBehaviour
{
    Animator animator;
    CircleCollider2D damageCol;
    public float lastingDuration = 4;
    float currentDuration = 0;
    bool isAttacking = false;
    bool endAnimPlayed = false;
    GameObject playerShip;
    public GameObject splash;

    void spawnSplash()
    {
        GameObject splashInstant = Instantiate(splash, transform.position - new Vector3(0, -0.1f, 0), Quaternion.identity);
        splashInstant.GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder + 2;
    }

    IEnumerator attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(3f / 12f);
        damageCol.enabled = true;
        yield return new WaitForSeconds(1 / 12f);
        damageCol.enabled = false;
        yield return new WaitForSeconds(6f / 12f);
        animator.SetTrigger("Idle");
        isAttacking = false;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        damageCol = GetComponent<CircleCollider2D>();
        damageCol.enabled = false;
        playerShip = FindObjectOfType<PlayerScript>().gameObject;
    }

    void Update()
    {
        currentDuration += Time.deltaTime;
        if (currentDuration > 6f / 12f && currentDuration <= lastingDuration)
        {
            if(Vector2.Distance(playerShip.transform.position, transform.position + new Vector3(0, 1.1f, 0)) < 2f && isAttacking == false)
            {
                StartCoroutine(attack());
            }
        }
        else if(currentDuration > lastingDuration)
        {
            if (endAnimPlayed == false && isAttacking == false)
            {
                endAnimPlayed = true;
                Invoke("spawnSplash", 6f / 12f);
                animator.SetTrigger("DiveIn");
                Destroy(this.gameObject, 7f / 12f);
            }
        }
    }
}
