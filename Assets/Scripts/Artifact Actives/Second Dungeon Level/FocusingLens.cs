using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusingLens : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    ArtifactBonus artifactBonus;
    public float comboTimer = 0;
    int numberBulletsFired = 0;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }

    private void Update()
    {
        if(comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
        }
        else
        {
            if(numberBulletsFired != 0)
            {
                numberBulletsFired = 0;
                artifactBonus.attackBonus = 3;
                artifacts.UpdateUI();
            }
        }
    }

    // Whenever the player fires the left weapon, and so on
    public override void firedLeftWeapon(GameObject[] bullet, Vector3 whichPositionFiredFrom, float angleTravel)
    {
        if(comboTimer <= 0)
        {
            comboTimer = 8;
            FindObjectOfType<DurationUI>().addTile(displayItem.displayIcon, 8);
        }

        if(comboTimer > 0)
        {
            artifactBonus.attackBonus -= 1;
            numberBulletsFired++;
            artifacts.UpdateUI();
        }
    }
    public override void firedFrontWeapon(GameObject[] bullet, Vector3 whichPositionFiredFrom, float angleTravel)
    {
        if (comboTimer <= 0)
        {
            comboTimer = 8;
            FindObjectOfType<DurationUI>().addTile(displayItem.displayIcon, 8);
        }

        if (comboTimer > 0)
        {
            artifactBonus.attackBonus -= 1;
            numberBulletsFired++;
            artifacts.UpdateUI();
        }
    }
    public override void firedRightWeapon(GameObject[] bullet, Vector3 whichPositionFiredFrom, float angleTravel)
    {
        if (comboTimer <= 0)
        {
            comboTimer = 8;
            FindObjectOfType<DurationUI>().addTile(displayItem.displayIcon, 8);
        }

        if (comboTimer > 0)
        {
            artifactBonus.attackBonus -= 1;
            numberBulletsFired++;
            artifacts.UpdateUI();
        }
    }
}
