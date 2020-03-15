using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellbombsV2 : ArtifactEffect
{
    public GameObject spellBomb;

    public override void playerDashed()
    {
        if (PlayerProperties.playerScript.enemiesDefeated == false)
        {
            Instantiate(spellBomb, PlayerProperties.playerShipPosition, Quaternion.identity);
        }
    }
}
