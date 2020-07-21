using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsidianTower : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject projectile;
    [SerializeField] AudioSource firingAudio;

    private void Start()
    {
        StartCoroutine(mainCycle());
    }

    IEnumerator mainCycle()
    {
        yield return new WaitForSeconds(7 / 12f);

        for(int i = 0; i < 5; i++)
        {
            animator.SetTrigger("Charge");
            yield return new WaitForSeconds(8 / 12f);

            firingAudio.Play();

            for (int k = 0; k < 4; k++)
            {
                GameObject projectileInstant = Instantiate(projectile, transform.position + Vector3.up * 2.2f, Quaternion.identity);
                projectileInstant.GetComponent<BasicProjectile>().angleTravel = k * 90;
            }

            yield return new WaitForSeconds(5 / 12f);
        }

        animator.SetTrigger("Sink");
        yield return new WaitForSeconds(7 / 12f);
        Destroy(this.gameObject);
    }

}
