using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalMeteor : MonoBehaviour
{
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] Animator animator;
    [SerializeField] Collider2D collider2D;
    [SerializeField] AudioSource splashAudio;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] GameObject shadow;

    private void Start()
    {
        collider2D.enabled = false;
    }

    public void Initialize(Vector3 targetPosition, float time, GameObject instantiater)
    {
        transform.position = targetPosition + Vector3.up * 20;
        this.projectileParent.instantiater = instantiater;
        LeanTween.move(this.gameObject, targetPosition, time).setEaseInQuad().setOnComplete(Impact);
        GameObject shadowInstant = Instantiate(shadow, targetPosition, Quaternion.identity);
        shadowInstant.transform.localScale = Vector3.zero;
        LeanTween.scale(shadowInstant, new Vector3(0.3f, 0.3f), time).setOnComplete(() => Destroy(shadowInstant));
    }

    void Impact()
    {
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.loop = false;
        StartCoroutine(ImpactProcedure());
        splashAudio.Play();
    }

    IEnumerator ImpactProcedure()
    {
        animator.SetTrigger("Splash");
        collider2D.enabled = true;
        yield return new WaitForSeconds(3 / 12f);
        collider2D.enabled = false;
        yield return new WaitForSeconds(4 / 12f);
        Destroy(this.gameObject);
    }

}
