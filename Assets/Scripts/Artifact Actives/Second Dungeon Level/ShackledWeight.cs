using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShackledWeight : ArtifactBonus
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    float speedDecreasePeriod = 0;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true && (firedFrontWeapon == true || firedLeftWeapon == true || firedRightWeapon == true))
        {
            firedFrontWeapon = false;
            firedLeftWeapon = false;
            firedRightWeapon = false;
            speedDecreasePeriod = 0.5f;
            artifacts.UpdateUI();
        }

        if(speedDecreasePeriod > 0)
        {
            speedDecreasePeriod -= Time.deltaTime;
            if(speedBonus != -2)
            {
                speedBonus = -2;
                artifacts.UpdateUI();
            }
        }
        else
        {
            if(speedBonus != 0)
            {
                speedBonus = 0;
                artifacts.UpdateUI();
            }
        }
    }
}
