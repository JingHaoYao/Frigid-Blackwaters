using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuietAllNoises : MonoBehaviour
{
    void Start()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>() as AudioSource[];
        foreach (AudioSource source in audioSources)
        {
            StartCoroutine(FadeOut(source, 0.1f));
        }
    }

    IEnumerator FadeOut(AudioSource source, float speed)
    {
        while (source.volume > 0)
        {
            source.volume -= speed;
            if (source.volume < 0)
            {
                source.volume = 0;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
