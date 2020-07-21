using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthenCoreOrb : MonoBehaviour
{
    [SerializeField] AudioSource explodeAudio;
    [SerializeField] GameObject earthenProjectile;
    [SerializeField] Animator animator;
    Coroutine mainLoopInstant;
    public EarthenCore earthenCore;

    private void Start()
    {
        mainLoopInstant = StartCoroutine(mainLoop());
    }

    void spawnProjectiles() {
        for(int i = 0; i < 4; i++)
        {
            GameObject projectileInstant = Instantiate(earthenProjectile, transform.position, Quaternion.identity);
            projectileInstant.GetComponent<BasicProjectile>().angleTravel = i * 90 + 45;
        }
    }

    public void explode()
    {
        StopCoroutine(mainLoopInstant);
        animator.SetTrigger("Explode");
        earthenCore.removeOrb(this.gameObject);
        Destroy(this.gameObject, 10 / 12f);
    }

    IEnumerator mainLoop()
    {
        yield return new WaitForSeconds(4f);

        explodeAudio.Play();
        animator.SetTrigger("Explode");

        yield return new WaitForSeconds(6 / 12f);

        spawnProjectiles();

        yield return new WaitForSeconds(4 / 12f);
        earthenCore.removeOrb(this.gameObject);
        Destroy(this.gameObject);
    }

}
