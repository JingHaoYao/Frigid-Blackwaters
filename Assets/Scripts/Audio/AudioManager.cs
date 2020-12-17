using UnityEngine.Audio;
using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        PlayerProperties.audioManager = this;
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            s.source.outputAudioMixerGroup = s.mixerGroup;
            s.source.enabled = false;
            s.source.enabled = true;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }

        if (s.source.isPlaying == false)
        {
            s.source.Play();
        }
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }

        if (s.source.isPlaying == true)
        {
            s.source.Stop();
        }
    }

    public void MuteSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }

        s.source.mute = true;
    }

    public void UnMuteSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }

        s.source.mute = false;
    }

    IEnumerator FadeInSound(string name, float speed, float maxVolume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s == null)
        {
            yield break;
        }

        s.source.volume = 0;
        float timeSpentFading = 0;

        while (s.source.volume < maxVolume && timeSpentFading < 3)
        {
            s.source.volume += speed;
            if(s.source.volume > maxVolume)
            {
                s.source.volume = maxVolume;
            }
            yield return new WaitForSeconds(0.1f);
            timeSpentFading += 0.1f;
        }

        s.source.volume = maxVolume;
    }

    IEnumerator FadeOutSound(string name, float speed)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        float timeSpentFading = 0;

        if (s == null)
        {
            yield break;
        }

        while (s.source.volume > 0)
        {
            s.source.volume -= speed;
            if(s.source.volume < 0)
            {
                s.source.volume = 0;
            }
            yield return new WaitForSeconds(0.1f);
            timeSpentFading += 0.1f;
            if(timeSpentFading >= 3)
            {
                break;
            }
        }

        s.source.volume = 0;
    }

    public void FadeIn(string name, float speed, float maxVolume)
    {
        StartCoroutine(FadeInSound(name, speed, maxVolume));
    }

    public void FadeOut(string name, float speed)
    {
        StartCoroutine(FadeOutSound(name, speed));
    }
}