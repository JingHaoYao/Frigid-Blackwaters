using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunicIdol : ArtifactBonus
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (displayItem.isEquipped == true && addedKill == true)
        {
            addedKill = false;
            healthBonus += 25;
            artifacts.UpdateUI();
        }
    }
}
