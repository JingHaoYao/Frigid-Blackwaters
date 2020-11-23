using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheTideBringer : ArtifactEffect
{
    [SerializeField] GameObject waterSplash;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;

    bool onCooldown = false;

    IEnumerator coolDown()
    {
        PlayerProperties.durationUI.addTile(displayItem.displayIcon, 6f);
        yield return new WaitForSeconds(6f);
        onCooldown = false;
    }

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
        if(((float)enemy.health / enemy.maxHealth) <= 0.25f)
        {
            if(onCooldown == false)
            {
                onCooldown = true;
                enemy.dealDamage(enemy.health, true);
                Instantiate(waterSplash, enemy.transform.position, Quaternion.identity);
                StartCoroutine(coolDown());
                artifactBonus.healthBonus += 300;
                artifactBonus.defenseBonus = Mathf.Clamp(artifactBonus.defenseBonus + 0.05f, 0, 0.25f);
                PlayerProperties.playerArtifacts.UpdateStats();
                PlayerProperties.playerScript.healPlayer(300);
            }
        }
    }
}
