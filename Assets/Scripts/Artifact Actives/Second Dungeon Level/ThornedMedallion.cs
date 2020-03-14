using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornedMedallion : ArtifactEffect
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
        int damageReflected = Mathf.RoundToInt(amountDamage / 100f);
        if(enemy.health <= damageReflected)
        {
            enemy.dealDamage(enemy.health - 1);
        }
        else
        {
            enemy.dealDamage(damageReflected);
        }
    }
}
