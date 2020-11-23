using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AshMaidenPyre : MonoBehaviour
{
    GameObject bossEnemy;
    [SerializeField] GameObject pyrotheumProjectile;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource ignitePyreAudio, buildAudio;
    [SerializeField] GameObject spear;
    AshMaiden ashMaiden;
    bool ready = false;

    public void Initialize(GameObject bossEnemy)
    {
        this.bossEnemy = bossEnemy;
        ashMaiden = bossEnemy.GetComponent<AshMaiden>();
    }

    public void IgnitePyre()
    {
        StartCoroutine(ignitePyreRoutine());
    }

    public bool IsPyreReady()
    {
        return ready;
    }

    public void RisePyre()
    {
        animator.Play("Ash Maiden Pyre Rise");
        buildAudio.Play();
        ready = true;
    }

    IEnumerator ignitePyreRoutine()
    {
        animator.Play("Ash Maiden Pyre Ignite");
        ignitePyreAudio.Play();
        ready = false;

        yield return new WaitForSeconds(5 / 12f);

        for(int i = 0; i < 8; i++)
        {
            GameObject pyrotheumProjectileInstant = Instantiate(pyrotheumProjectile, transform.position + Vector3.up * 2f, Quaternion.identity);
            pyrotheumProjectileInstant.GetComponent<PyrotheumProjectile>().angleTravel = i * 45;
            pyrotheumProjectileInstant.GetComponent<ProjectileParent>().instantiater = bossEnemy;
        }

        yield return new WaitForSeconds(3 / 12f);

        GameObject ashMaidenSpear = Instantiate(spear, transform.position + Vector3.up * 3f, Quaternion.Euler(0, 0, 90));

        yield return new WaitForSeconds(4 / 12f);
        ashMaiden.AddSpear(ashMaidenSpear.GetComponent<AshMaidenSpear>());
        // Call function to ashmaiden
    }
}
