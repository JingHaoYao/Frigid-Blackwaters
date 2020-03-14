using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunicIdol : ArtifactEffect
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

    public override void addedKill(string tag, Vector3 deathPos, Enemy enemy)
    {
        GetComponent<ArtifactBonus>().healthBonus += 25;
        artifacts.UpdateUI();
    }
}
