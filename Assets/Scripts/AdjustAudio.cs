using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustAudio : MonoBehaviour
{
    [SerializeField] AudioSource[] audioSources;
    PlayerScript playerScript;
    public bool continuous;

    void adjustVolume()
    {
        foreach(AudioSource audioSource in audioSources)
        {
            audioSource.volume = Mathf.Clamp(20 - Vector2.Distance(playerScript.transform.position, transform.position) / 20, 0, 1);
        }
    }

    IEnumerator continouslyAdjust()
    {
        while (true)
        {
            adjustVolume();
            yield return null;
        }
    }

    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        if (continuous)
        {
            StartCoroutine(continouslyAdjust());
        }
    }
}
