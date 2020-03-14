using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindDemonJade : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    ArtifactBonus artifactBonus;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }
   
    // Whenever the player enters a previously unentered room
    public override void exploredNewRoom(int whatRoomType)
    {
        artifactBonus.speedBonus += 0.2f;
        artifacts.UpdateUI();
    }
}
