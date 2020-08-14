using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrassGolemHead : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject brassBomb;
    [SerializeField] AudioSource steamSound;
    [SerializeField] AudioSource steamLaunchSound;
    [SerializeField] AudioSource crackSound;
    [SerializeField] AudioSource energyExplosion;
    [SerializeField] SpriteRenderer spriteRenderer;


    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    public void startHitFrame()
    {
        StartCoroutine(hitFrame());
    }

    public void SetToUnactive()
    {
        this.gameObject.SetActive(false);
    }

    public void StartLoadOut()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(loadOut());
    }

    IEnumerator loadOut()
    {
        this.gameObject.SetActive(true);
        animator.SetTrigger("Steam");
        transform.localPosition = new Vector3(0, 0.5f, 0);
        LeanTween.moveLocal(this.gameObject, new Vector3(0, 1.06f), 1.5f).setEaseOutCirc();
        steamSound.Play();

        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator spawnBomb()
    {
        steamSound.Play();
        animator.SetTrigger("SummonBomb");
    
        yield return new WaitForSeconds(10 / 12f);

        steamLaunchSound.Play();
        GameObject bombInstant = Instantiate(brassBomb, transform.position + Vector3.up * 7, Quaternion.identity);
        bombInstant.GetComponent<BrassGolemBomb>().Initialize(transform.parent.gameObject, PlayerProperties.playerShipPosition);

        yield return new WaitForSeconds(5 / 12f);

        steamSound.Play();

        yield return new WaitForSeconds(5 / 12f);

        animator.SetTrigger("Steam");
    }

    public void SpawnBomb()
    {
        StartCoroutine(spawnBomb());
    }

    IEnumerator headDeath()
    {
        StopAllCoroutines();
        spriteRenderer.color = Color.white;

        animator.SetTrigger("Death");

        yield return new WaitForSeconds(3 / 12f);

        crackSound.Play();
        steamSound.Play();

        yield return new WaitForSeconds(4 / 12f);
        energyExplosion.Play();
    }

    public void StartHeadDeath()
    {
        StartCoroutine(headDeath());
    }

}
