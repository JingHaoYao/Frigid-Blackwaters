using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandOfTheNorthernWind : ArtifactEffect
{
    [SerializeField] private ArtifactBonus artifactBonus;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        updateSpeed();
    }

    public override void updatedInventory()
    {
        updateSpeed();
    }

    public override void healed(int healingAmount)
    {
        updateSpeed();
    }

    void updateSpeed()
    {
        int numberTicks = Mathf.Clamp(((2000 - PlayerProperties.playerScript.shipHealth) / 100), 0, int.MaxValue);
        artifactBonus.speedBonus = numberTicks * 0.2f;
        PlayerProperties.playerArtifacts.UpdateUI();
    }
}
