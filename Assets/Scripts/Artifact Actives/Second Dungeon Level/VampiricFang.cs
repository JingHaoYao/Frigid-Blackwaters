using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampiricFang : ArtifactEffect
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

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
        playerScript.healPlayer(damageDealt * 50);
    }
}
