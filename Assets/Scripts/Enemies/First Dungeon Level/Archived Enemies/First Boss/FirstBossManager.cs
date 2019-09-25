using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossManager : MonoBehaviour
{
    public GameObject spearPhase, swordPhase, musketPhase;
    bool activatedFirstBoss = false, activatedSecondBoss = false, activatedThirdBoss = false;
    public bool startBosses = false;
    GameObject playerShip;
    public GameObject serenityBlackWindow;
    public GameObject serenitysDialogue;
    public GameObject sceneTransitioner;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
    }

    private void Update()
    {
        if (startBosses == true)
        {
            if (activatedFirstBoss == false)
            {
                spearPhase.SetActive(true);
                activatedFirstBoss = true;
                playerShip.GetComponent<PlayerScript>().enemiesDefeated = false;
            }

            if (Vector2.Distance(Camera.main.transform.position, new Vector3(-800, 20, 0)) < 4f && activatedSecondBoss == false)
            {
                swordPhase.SetActive(true);
                activatedSecondBoss = true;
                playerShip.transform.position += Vector3.up * 3;
            }

            if (Vector2.Distance(Camera.main.transform.position, new Vector3(-800, 40, 0)) < 4f && activatedThirdBoss == false)
            {
                musketPhase.SetActive(true);
                activatedThirdBoss = true;
                playerShip.transform.position += Vector3.up * 3;
            }
        }
    }

}
