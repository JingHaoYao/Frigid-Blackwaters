using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivePod : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ProjectileParent projectileParent;
    public GameObject explosion;
    private float speed = 8;
    private bool inExplosion = false;

    IEnumerator explodeSequence()
    {
        inExplosion = true;
        animator.SetTrigger("AboutToExplode");
        yield return new WaitForSeconds(0.75f);
        Destroy(this.gameObject);
        GameObject explosionInstant = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionInstant.GetComponent<ProjectileParent>().instantiater = projectileParent.instantiater;
    }

    void Update()
    {
        transform.position += Time.deltaTime * Mathf.Clamp(speed, 0, 9999) * (PlayerProperties.playerShipPosition - transform.position).normalized;
        speed -= Time.deltaTime * 3;

        if((speed < 3 || Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) < 2) && inExplosion == false)
        {
            StartCoroutine(explodeSequence());
        }
    }
}
