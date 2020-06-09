using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FrontierGates : MonoBehaviour
{
    public bool goBack = false;
    public Vector3 bossRoomLocation;
    public GameObject unactivatedBoss;
    public GameObject bossRoom;


    IEnumerator moveShip(PlayerScript playerScript)
    {
        Sound s = Array.Find(FindObjectOfType<AudioManager>().sounds, sound => sound.name == "Battle Drums");
        s.source.mute = true;
        FindObjectOfType<BlackOverlay>().transition();
        yield return new WaitForSeconds(1f);
        bossRoom.SetActive(true);
        playerScript.gameObject.transform.position = bossRoomLocation;
        Camera.main.transform.position = new Vector3(-780, -10, 0);
        unactivatedBoss.SetActive(true);
        yield return new WaitForSeconds(1f);
        //activate boss
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (goBack == true)
        {
            FindObjectOfType<PauseMenu>().loadHub();
            FindObjectOfType<PlayerScript>().shipRooted = true;
        }
        else
        {
            PlayerScript playerScript = FindObjectOfType<PlayerScript>();
            playerScript.enemiesDefeated = false;
            playerScript.shipRooted = true;
            StartCoroutine(moveShip(playerScript));
        }
    }
}
