using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalwartTalisman : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    float defensePeriod = 0;
    ArtifactBonus artifactBonus;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }

    void Update()
    {
        if(defensePeriod > 0)
        {
            defensePeriod -= Time.deltaTime;
            if(artifactBonus.defenseBonus != 0.2f)
            {
                artifactBonus.defenseBonus = 0.2f;
                artifacts.UpdateUI();
            }
        }
        else
        {
            if(artifactBonus.defenseBonus != 0)
            {
                artifactBonus.defenseBonus = 0;
                artifacts.UpdateUI();
            }
        }
    }

    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if (defensePeriod <= 0)
        {
            defensePeriod = 1;
        }
    }
}
