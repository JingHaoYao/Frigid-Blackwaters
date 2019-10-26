using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindDemonJade : ArtifactBonus
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
        if (displayItem.isEquipped == true && exploredNewRoom == true)
        {
            exploredNewRoom = false;
            speedBonus += 0.2f;
            artifacts.UpdateUI();
        }
    }
}
