using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalwartTalisman : ArtifactBonus
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    float defensePeriod = 0;

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
            if(defensePeriod <= 0)
            {
                defensePeriod = 1;
            }
        }

        if(defensePeriod > 0)
        {
            defensePeriod -= Time.deltaTime;
            if(defenseBonus != 0.2f)
            {
                defenseBonus = 0.2f;
                artifacts.UpdateUI();
            }
        }
        else
        {
            if(defenseBonus != 0)
            {
                defenseBonus = 0;
                artifacts.UpdateUI();
            }
        }
    }
}
