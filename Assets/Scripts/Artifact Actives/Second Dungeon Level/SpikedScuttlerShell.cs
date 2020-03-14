using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedScuttlerShell : ArtifactEffect
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
    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if(Vector2.Distance(playerScript.transform.position, enemy.transform.position) < 3.5f)
        {
            enemy.dealDamage(Mathf.FloorToInt(amountDamage / 100f));
        }
    }
}
