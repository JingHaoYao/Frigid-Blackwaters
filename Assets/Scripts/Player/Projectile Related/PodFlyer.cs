using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodFlyer : PlayerProjectile
{
    [SerializeField] Animator animator;
    [SerializeField] float timeUntilExplode = 2;
    [SerializeField] float speed;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject smallerPod;
    [SerializeField] int numberSmallerPods;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        triggerWeaponFireFlag(transform.position, (angletoCursor() * Mathf.Rad2Deg + 360) % 360);
        StartCoroutine(podExplosionSequence());
    }

    IEnumerator podExplosionSequence()
    {
        yield return new WaitForSeconds(timeUntilExplode * 0.8f);
        animator.SetTrigger("AboutToExplode");
        yield return new WaitForSeconds(timeUntilExplode * 0.2f);
        Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        if (numberSmallerPods > 0)
        {
            if (numberSmallerPods > 1)
            {
                float angleIncrement = 360 / numberSmallerPods;
                for (int i = 0; i < numberSmallerPods; i++)
                {
                     Instantiate(smallerPod, transform.position + new Vector3(Mathf.Cos(angleIncrement * i * Mathf.Deg2Rad), Mathf.Sin(angleIncrement * i * Mathf.Deg2Rad)) * 0.75f, Quaternion.identity);
                }
            }
            else
            {
               Instantiate(smallerPod, transform.position, Quaternion.identity);
            }
        }
        Destroy(this.gameObject);
    }

    float angletoCursor()
    {
        return Mathf.Atan2(PlayerProperties.cursorPosition.y - transform.position.y, PlayerProperties.cursorPosition.x - transform.position.x);
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, PlayerProperties.cursorPosition) > 0.5f)
        {
            transform.position += new Vector3(Mathf.Cos(angletoCursor()), Mathf.Sin(angletoCursor())) * speed * Time.deltaTime;
        }
    }
}
