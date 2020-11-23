using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineOfAMagmaCrawler : ArtifactEffect
{
    float previousMultiplier = 0;

    public override void artifactEquipped()
    {
        if(PlayerProperties.flammableController != null)
        {
            previousMultiplier = PlayerProperties.flammableController.GetDamageMultiplier;
            PlayerProperties.flammableController.UpdateDamageMultiplier(previousMultiplier * 0.5f);
        }
    }

    public override void artifactUnequipped()
    {
        if (PlayerProperties.flammableController != null)
        {
            PlayerProperties.flammableController.UpdateDamageMultiplier(previousMultiplier);
        }
    }
}
