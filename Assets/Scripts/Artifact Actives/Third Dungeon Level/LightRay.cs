using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRay : MonoBehaviour
{
    [SerializeField] CircleCollider2D damageCollider;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource pulseAudio;

    private void Start()
    {
        StartCoroutine(beamProcess(4));
    }

    IEnumerator beamProcess(float waitPeriod)
    {
        pulseAudio.Play();
        LeanTween.value(0, 0.5f, 5 / 12f).setOnUpdate((float val) => { pulseAudio.volume = val; });
        yield return new WaitForSeconds(5 / 12f);
        damageCollider.enabled = true;
        float period = 0;
        while(period < waitPeriod)
        {
            period += Time.deltaTime;
            if(Vector2.Distance(transform.position, PlayerProperties.cursorPosition) > 0.1f)
            {
                transform.position += Time.deltaTime * (PlayerProperties.cursorPosition - transform.position).normalized * 3;
            }
            yield return null;
        }
        animator.SetTrigger("FadeOut");
        damageCollider.enabled = false;
        LeanTween.value(0.5f, 0, 5 / 12f).setOnUpdate((float val) => { pulseAudio.volume = val; });
        yield return new WaitForSeconds(5 / 12f);
        Destroy(this.gameObject);
    }
}
