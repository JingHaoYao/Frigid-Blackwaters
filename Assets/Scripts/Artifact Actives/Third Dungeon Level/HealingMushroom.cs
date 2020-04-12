using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingMushroom : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D collider;

    IEnumerator heal()
    {
        collider.enabled = false;
        animator.SetTrigger("GiveHealing");
        yield return new WaitForSeconds(8 / 12f);
        PlayerProperties.playerScript.healPlayer(500);
        LeanTween.alpha(this.gameObject, 0, 1f).setOnComplete(() => Destroy(this.gameObject));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9 && collision.gameObject.tag == "playerHitBox")
        {
            StartCoroutine(heal());
        }
    }
}
