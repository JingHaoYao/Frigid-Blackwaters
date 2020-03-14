using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtherealSconce : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;
    public GameObject spiritOrb;

    public override void addedKill(string tag, Vector3 deathPos, Enemy enemy)
    {
        if (PlayerProperties.playerScript.enemiesDefeated == true)
        {
            GameObject orb = Instantiate(spiritOrb, deathPos + new Vector3(0, 0.5f, 0), Quaternion.identity);
            orb.GetComponent<SpiritOrb>().sconce = this;
        }
    }

    public void addBonus()
    {
        artifactBonus.healthBonus += 50;
        PlayerProperties.playerScript.healPlayer(100);
        PlayerProperties.playerArtifacts.UpdateUI();
    }
}
