using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDungeonFinalBossPortal : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource closingAudio;
    [SerializeField] private AudioSource firingAudio;
    [SerializeField] private AudioSource openingAudio;
    public Enemy boss;
    public SpriteRenderer bossRenderer;

    public GameObject portalShot;

    private void Update()
    {
        spriteRenderer.sortingOrder = -bossRenderer.sortingOrder - 3;
    }

    public void initialWarp()
    {
        StartCoroutine(warp(false));
    }

    public void endWarp()
    {
        StartCoroutine(warp(true));
    }

    IEnumerator warp(bool fireShots)
    {
        spriteRenderer.enabled = true;
        animator.SetTrigger("Appear");
        openingAudio.Play();
        yield return new WaitForSeconds(4 / 12f);
        if (fireShots)
        {
            firingAudio.Play();
            for(int i = 0; i < 6; i++)
            {
                GameObject portalShotInstant = Instantiate(portalShot, transform.position, Quaternion.identity);
                portalShotInstant.GetComponent<BasicProjectile>().angleTravel = i * 60;
                portalShotInstant.GetComponent<ProjectileParent>().instantiater = boss.gameObject;
            }
        }
        yield return new WaitForSeconds(4 / 12 + 1.2f);
        animator.SetTrigger("Dissapear");
        closingAudio.Play();
        yield return new WaitForSeconds(8 / 12f);
        spriteRenderer.enabled = false;
    }
}
