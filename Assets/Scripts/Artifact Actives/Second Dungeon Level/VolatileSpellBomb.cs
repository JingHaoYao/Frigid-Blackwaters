using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolatileSpellBomb : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    ArtifactBonus artifactBonus;
    public GameObject spellExplosion;


    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }

    public override void addedKill(string tag, Vector3 deathPos, Enemy enemy)
    {
        Instantiate(spellExplosion, deathPos, Quaternion.identity);
    }
}
