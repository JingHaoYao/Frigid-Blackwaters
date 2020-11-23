using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossSwordSlash : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider2D boxCol;
    [SerializeField] SpriteRenderer spriteRenderer;
    public float angleAttack;

    IEnumerator enableHitBox()
    {
        yield return new WaitForSeconds(0.4f);
        animator.SetTrigger("Slash");
        spriteRenderer.enabled = true;
        boxCol.enabled = true;
        yield return new WaitForSeconds(0.584f / 2f);
        boxCol.enabled = false;
        spriteRenderer.enabled = false;
    }

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, angleAttack);
        spriteRenderer.enabled = false;
        boxCol.enabled = false;
        Destroy(this.gameObject, 1.167f);
        StartCoroutine(enableHitBox());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerProperties.playerScript.dealDamageToShip(1100, this.gameObject);
        }
    }
}
