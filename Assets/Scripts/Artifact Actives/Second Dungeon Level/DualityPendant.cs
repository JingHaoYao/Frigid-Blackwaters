using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualityPendant : ArtifactEffect
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

    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        int damageReflected = Mathf.FloorToInt(amountDamage / 100f);
        if(enemy.health > damageReflected)
        {
            enemy.dealDamage(damageReflected);
        }
        else
        {
            enemy.dealDamage(enemy.health - 1);
        }
    }

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
        if (playerScript.shipHealth > 50)
        {
            playerScript.dealDamageToShip(50, this.gameObject);
        }
        else
        {
            playerScript.dealDamageToShip(playerScript.shipHealth - 1, this.gameObject);
        }
    }
}
