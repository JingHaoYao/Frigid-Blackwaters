using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomGoliathMushroom : MonoBehaviour
{
    public GameObject sprayProjectile;
    [SerializeField] Animator animator;
    [SerializeField] ProjectileParent projectileParent;

    void Start()
    {
        StartCoroutine(mushroomCycle());
    }

    IEnumerator mushroomCycle()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(3 / 12f);

        for (int k = 0; k < 8; k++)
        {
            float angle = k * 45;
            GameObject projectileInstant = Instantiate(sprayProjectile, transform.position + Vector3.up, Quaternion.identity);
            projectileInstant.GetComponent<BasicProjectile>().angleTravel = angle;
            projectileInstant.GetComponent<ProjectileParent>().instantiater = projectileParent.instantiater;

        }

        animator.SetTrigger("Sink");
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
