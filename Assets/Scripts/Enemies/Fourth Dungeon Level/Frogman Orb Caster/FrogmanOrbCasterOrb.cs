using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogmanOrbCasterOrb : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    // 0 - blue
    // 1 - green
    // 2 - purple
    [SerializeField] int whatOrb = 0;
    [SerializeField] GameObject projectile;
    [SerializeField] ProjectileParent projectileParent;

    private void Start()
    {
        StartCoroutine(orbProcess());
    }

    IEnumerator orbProcess()
    {
        yield return new WaitForSeconds(7/12f);
        audioSource.Play();
        if(whatOrb == 0)
        {
            for(int i = 0; i < 8; i++)
            {
                float angle = i * 45;
                GameObject projectileInstant = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                projectileInstant.GetComponent<FrogmanOrbCasterProjectile>().angleTravel = angle;
                projectileInstant.GetComponent<ProjectileParent>().instantiater = this.projectileParent.gameObject;
            }
        }
        else if(whatOrb == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                float angle = i * 90;
                GameObject projectileInstant = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                projectileInstant.GetComponent<FrogmanOrbCasterProjectile>().angleTravel = angle - 5;
                projectileInstant.GetComponent<ProjectileParent>().instantiater = this.projectileParent.gameObject;
                projectileInstant = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                projectileInstant.GetComponent<FrogmanOrbCasterProjectile>().angleTravel = angle + 5;
                projectileInstant.GetComponent<ProjectileParent>().instantiater = this.projectileParent.gameObject;
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                float angle = i * 90 + 45;
                GameObject projectileInstant = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                projectileInstant.GetComponent<FrogmanOrbCasterProjectile>().angleTravel = angle - 5;
                projectileInstant.GetComponent<ProjectileParent>().instantiater = this.projectileParent.gameObject;
                projectileInstant = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                projectileInstant.GetComponent<FrogmanOrbCasterProjectile>().angleTravel = angle + 5;
                projectileInstant.GetComponent<ProjectileParent>().instantiater = this.projectileParent.gameObject;
            }
        }
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Dissapear");
        yield return new WaitForSeconds(8 / 12f);
        Destroy(this.gameObject);
    }
}
