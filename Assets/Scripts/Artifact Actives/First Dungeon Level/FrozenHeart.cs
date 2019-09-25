using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenHeart : MonoBehaviour
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    GameObject playerShip;
    float coolDownPeriod = 0;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        playerShip = GameObject.Find("PlayerShip");
    }

    void Update()
    {
        if (displayItem.isEquipped == true)
        {
            if (artifacts.numKills < 8 && playerScript.enemiesDefeated == false)
            {
                if (coolDownPeriod < 1.5f)
                {
                    coolDownPeriod += Time.deltaTime;
                }
                else
                {
                    coolDownPeriod = 0;
                    artifacts.numKills++;
                }
            }
            else
            {
                coolDownPeriod = 0;
            }
        }
    }
}
