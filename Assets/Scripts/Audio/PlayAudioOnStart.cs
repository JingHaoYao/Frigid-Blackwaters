using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnStart : MonoBehaviour
{
    public string nameOfAudio;

    void Start()
    {
        FindObjectOfType<AudioManager>().PlaySound(nameOfAudio);
    }
}
