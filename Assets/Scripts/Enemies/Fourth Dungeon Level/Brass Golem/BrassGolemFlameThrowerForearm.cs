using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrassGolemFlameThrowerForearm : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> particles;
    [SerializeField] Collider2D damageCollider;
    [SerializeField] AudioSource flameAudio;

    IEnumerator flameThrowerRoutine(float duration)
    {
        float currentPeriod = 0;

        flameAudio.Play();

        foreach(ParticleSystem particle in particles)
        {
            particle.Stop();
            var mainModule = particle.main;
            mainModule.duration = duration + 0.5f;
            particle.Play();
        }

        yield return new WaitForSeconds(0.5f);

        while (currentPeriod < duration)
        {
            damageCollider.enabled = true;
            yield return new WaitForSeconds(0.1f);
            damageCollider.enabled = false;
            yield return new WaitForSeconds(0.1f);

            currentPeriod += 0.2f;
        }

        flameAudio.Stop();

        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
        }
    }

    public void stopProcedures()
    {
        StopAllCoroutines();
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
        }
    }

    public void StartFlameThrowerSequence(float duration)
    {
        StartCoroutine(flameThrowerRoutine(duration));
    }
}
