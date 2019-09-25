using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridCrab1Attack : MonoBehaviour
{
    Animator animator;
    public GameObject damageHitBox;
    public float waitDuration = 2;
    public int numberDownSprites = 3;

    IEnumerator attack()
    {
        yield return new WaitForSeconds(1f / 12f);
        this.GetComponent<AudioSource>().Play();
        damageHitBox.SetActive(true);
        yield return new WaitForSeconds(2 / 12f);
        damageHitBox.SetActive(false);
        yield return new WaitForSeconds(3f / 12f + waitDuration);
        animator.SetTrigger("Down");
        yield return new WaitForSeconds(numberDownSprites / 12f);
        Destroy(this.gameObject);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        damageHitBox.SetActive(false);
        StartCoroutine(attack());
    }
}
