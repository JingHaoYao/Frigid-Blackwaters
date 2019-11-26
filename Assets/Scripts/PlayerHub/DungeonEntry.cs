using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntry : MonoBehaviour {
    public GameObject dungeonSelector;
    PlayerScript playerScript;

    private void Start()
    {
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox" || collision.gameObject.name == "PlayerShip")
        {
            MiscData.numberDungeonRuns++;
            playerScript.shipRooted = true;
            dungeonSelector.SetActive(true);
            playerScript.windowAlreadyOpen = true;
            FindObjectOfType<AudioManager>().FadeOut("Background Music", 0.2f);
            FindObjectOfType<AudioManager>().FadeOut("Background Waves", 0.2f);
        }
    }
}
