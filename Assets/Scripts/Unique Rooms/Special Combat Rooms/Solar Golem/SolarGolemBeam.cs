using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarGolemBeam : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource pulseAudio;
    [SerializeField] private BoxCollider2D damageCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    int baseSortingOrder;

    public void Initialize(float originalAngle, float toAngle, float time, int baseSortingLayer)
    {
        transform.rotation = Quaternion.Euler(0, 0, originalAngle);
        StartCoroutine(solarBeamProcess(originalAngle, toAngle, time));
        baseSortingOrder = baseSortingLayer;
    }

    IEnumerator solarBeamProcess(float originalAngle, float toAngle, float time)
    {
        pulseAudio.Play();
        LeanTween.value(0, 0.5f, 0.583f).setOnUpdate((float val) => { pulseAudio.volume = val; });
        yield return new WaitForSeconds(0.583f);
        damageCollider.enabled = true;
        LeanTween.value(originalAngle, toAngle, time).setEaseInOutCirc().setOnUpdate((float val) => { transform.rotation = Quaternion.Euler(0, 0, val); });
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Winddown");
        damageCollider.enabled = false;
        LeanTween.value(0.5f, 0f, 0.583f).setOnUpdate((float val) => { pulseAudio.volume = val; });
        yield return new WaitForSeconds(0.583f);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if(transform.rotation.eulerAngles.z % 360 > 180)
        {
            spriteRenderer.sortingOrder = baseSortingOrder + 2;
        }
        else
        {
            spriteRenderer.sortingOrder = baseSortingOrder - 2;
        }
    }

    public void forceShutDown()
    {
        StopAllCoroutines();
        damageCollider.enabled = false;
        animator.SetTrigger("Winddown");
        LeanTween.value(0.5f, 0f, 0.583f).setOnUpdate((float val) => { pulseAudio.volume = val; }).setOnComplete(() => Destroy(this.gameObject));
    }
}
