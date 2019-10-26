using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerativeBandage : ArtifactBonus
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    float timeToHealPeriod = 0;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true && tookDamage == true)
        {
            tookDamage = false;
            if(timeToHealPeriod <= 0)
            {
                timeToHealPeriod = 3;
            }
        }

        if(timeToHealPeriod > 0)
        {
            timeToHealPeriod -= Time.deltaTime;
            if(timeToHealPeriod <= 0)
            {
                playerScript.trueDamage -= 150;
                timeToHealPeriod = 0;
            }
        }
    }
}
