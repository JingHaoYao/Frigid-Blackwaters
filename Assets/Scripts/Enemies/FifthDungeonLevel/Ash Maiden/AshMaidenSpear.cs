using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AshMaidenSpear : MonoBehaviour
{
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] PolygonCollider2D damageBox;
    [SerializeField] Animator animator;
    [SerializeField] float speed = 12;
    [SerializeField] AudioSource impactAudio;

    bool impacted = false;

    private void Start()
    {
        animator.Play("Ash Maiden Spear Ignite");
        damageBox.enabled = false;
    }

    public void Initialize(GameObject bossEnemy)
    {
        projectileParent.instantiater = bossEnemy;
    }

    private float angleToShip
    {
        get
        {
            return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
    }

    public void PrepAndFireSpear(float angleAttack)
    {
        StartCoroutine(prepSpearRoutine(angleAttack));
    }
    
    public void FadeOutSpear()
    {
        LeanTween.alpha(this.gameObject, 0, 0.5f);
        Destroy(this.gameObject, 0.5f);
    }

    IEnumerator prepSpearRoutine(float angleAttack)
    {
        LeanTween.rotate(this.gameObject, new Vector3(0, 0, angleAttack * Mathf.Rad2Deg), 0.2f);
        yield return new WaitForSeconds(0.2f);
        damageBox.enabled = true;

        while(!impacted)
        {
            transform.position += new Vector3(Mathf.Cos(angleAttack), Mathf.Sin(angleAttack)) * Time.deltaTime * 12;

            yield return null;
        }

        animator.Play("Ash Maiden Ash Spear");
        impactAudio.Play();

        yield return new WaitForSeconds(8 / 18f);

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 12)
        {
            impacted = true;
        }
    }
}
