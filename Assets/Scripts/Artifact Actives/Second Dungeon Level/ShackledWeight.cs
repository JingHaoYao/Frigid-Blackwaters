using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShackledWeight : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    float speedDecreasePeriod = 0;
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
        if(speedDecreasePeriod > 0)
        {
            speedDecreasePeriod -= Time.deltaTime;
            if(artifactBonus.speedBonus != -2)
            {
                artifactBonus.speedBonus = -2;
                artifacts.UpdateUI();
            }
        }
        else
        {
            if(artifactBonus.speedBonus != 0)
            {
                artifactBonus.speedBonus = 0;
                artifacts.UpdateUI();
            }
        }
    }

    // Whenever the player fires the left weapon, and so on
    public override void firedLeftWeapon(GameObject[] bullet, Vector3 whichPositionFiredFrom, float angleTravel)
    {
        speedDecreasePeriod = 0.5f;
    }
    public override void firedFrontWeapon(GameObject[] bullet, Vector3 whichPositionFiredFrom, float angleTravel)
    {
        speedDecreasePeriod = 0.5f;
    }
    public override void firedRightWeapon(GameObject[] bullet, Vector3 whichPositionFiredFrom, float angleTravel)
    {
        speedDecreasePeriod = 0.5f;
    }
}
