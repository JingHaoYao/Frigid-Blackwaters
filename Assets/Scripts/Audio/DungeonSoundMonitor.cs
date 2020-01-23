using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSoundMonitor : MonoBehaviour
{
    PlayerScript playerScript;
    AudioManager audioManager;
    public string[] ominousIDs;
    float ambiancePlayPeriod = 0;
    float ambiancePlayThreshold = 15;

    void Start()
    {
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        audioManager = FindObjectOfType<AudioManager>();
        Array.Find(audioManager.sounds, sound => sound.name == "Battle Drums").source.volume = 0;
    }

    void Update()
    {
        if(playerScript.enemiesDefeated == true && Array.Find(audioManager.sounds, sound => sound.name == "Dungeon Ambiance").source.volume == 0)
        {
            if (playerScript.playerDead == false)
            {
                //Debug.Log("audio");
                audioManager.FadeIn("Dungeon Ambiance", 0.1f, .5f);
                audioManager.FadeOut("Battle Drums", 0.1f);
            }
        }
        
        if(playerScript.enemiesDefeated == false && Array.Find(audioManager.sounds, sound => sound.name == "Battle Drums").source.volume == 0)
        {
            if (playerScript.playerDead == false)
            {
                audioManager.FadeIn("Battle Drums", 0.1f, .5f);
                audioManager.FadeOut("Dungeon Ambiance", 0.1f);
            }
        }

        if(playerScript.enemiesDefeated == true && Array.Find(audioManager.sounds, sound => sound.name == "Dungeon Ambiance").source.mute == false)
        {
            ambiancePlayPeriod += Time.deltaTime;
            if(ambiancePlayPeriod >= ambiancePlayThreshold)
            {
                audioManager.PlaySound(ominousIDs[UnityEngine.Random.Range(0, ominousIDs.Length)]);
                ambiancePlayThreshold = UnityEngine.Random.Range(15, 30);
                ambiancePlayPeriod = 0;
            }
        }
    }
}
