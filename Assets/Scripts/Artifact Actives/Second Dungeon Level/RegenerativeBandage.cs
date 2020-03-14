using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerativeBandage : ArtifactEffect
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
        if(timeToHealPeriod > 0)
        {
            timeToHealPeriod -= Time.deltaTime;
            if(timeToHealPeriod <= 0)
            {
                playerScript.healPlayer(150);
                timeToHealPeriod = 0;
            }
        }
    }

    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if (timeToHealPeriod <= 0)
        {
            timeToHealPeriod = 3;
        }
    }
}
