using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCallerHead : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject lavaBallProjectile;
    [SerializeField] AudioSource deathAudio;
    [SerializeField] AudioSource loopAudio;
    SpriteRenderer baseRenderer;
    private bool isAttacking = false;
    Camera mainCamera;
    GameObject enemyBase;
    float idleDistance = 1.5f;

    public void StartLavaPull(int cycles)
    {
        StartCoroutine(lavaPull(cycles));
    }

    IEnumerator moveLoop()
    {
        LeanTween.value(1.25f, 1.75f, 1).setLoopPingPong().setOnUpdate((float val) => { idleDistance = val; });
        while (true)
        {
            transform.position = enemyBase.transform.position + Vector3.up * idleDistance;
            spriteRenderer.sortingOrder = baseRenderer.sortingOrder;
            yield return null;
        }
    }

    public void Initialize(GameObject baseObject)
    {
        baseRenderer = baseObject.GetComponent<SpriteRenderer>();
        this.enemyBase = baseObject;
        mainCamera = Camera.main;
        StartCoroutine(moveLoop());
    }

    public bool IsAttacking
    {
        get
        {
            return isAttacking;
        }
    }

    IEnumerator lavaPull(int cycles)
    {
        animator.SetTrigger("Open");
        isAttacking = true;
        yield return new WaitForSeconds(5 / 12f);
        loopAudio.Play();
        for(int i = 0; i < cycles; i++)
        {
            GameObject projectileInstant = Instantiate(lavaBallProjectile, new Vector3(mainCamera.transform.position.x - 8, transform.position.y), Quaternion.identity);
            projectileInstant.GetComponent<LavaCallerLavaBall>().Initialize(this.gameObject, transform.position + Vector3.up);
            projectileInstant = Instantiate(lavaBallProjectile, new Vector3(mainCamera.transform.position.x + 8, transform.position.y), Quaternion.identity);
            projectileInstant.GetComponent<LavaCallerLavaBall>().Initialize(this.gameObject, transform.position + Vector3.up);
            projectileInstant = Instantiate(lavaBallProjectile, new Vector3(transform.position.x, mainCamera.transform.position.y + 8), Quaternion.identity);
            projectileInstant.GetComponent<LavaCallerLavaBall>().Initialize(this.gameObject, transform.position + Vector3.up);
            projectileInstant = Instantiate(lavaBallProjectile, new Vector3(transform.position.x, mainCamera.transform.position.y - 8), Quaternion.identity);
            projectileInstant.GetComponent<LavaCallerLavaBall>().Initialize(this.gameObject, transform.position + Vector3.up);
            yield return new WaitForSeconds(1f);
        }
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(5 / 12f);
        loopAudio.Stop();

        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    public void Death()
    {
        StopAllCoroutines();
        StartCoroutine(deathProcedure());
    }

    IEnumerator flashRedDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    public void DamageHit()
    {
        StartCoroutine(flashRedDamage());
    }

    IEnumerator deathProcedure()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(5 / 12f);
        deathAudio.Play();
        Destroy(this.gameObject, 4 / 12f);
    }

}
