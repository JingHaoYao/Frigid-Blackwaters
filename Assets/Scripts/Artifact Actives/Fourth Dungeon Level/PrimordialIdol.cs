using UnityEngine;

public class PrimordialIdol : ArtifactEffect
{
    [SerializeField] ArtifactBonus artifactBonus;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if ((float)PlayerProperties.playerScript.shipHealth / PlayerProperties.playerScript.shipHealthMAX <= 0.5f)
        {
            artifactBonus.attackBonus = 3;
            artifactBonus.speedBonus = 1;
            PlayerProperties.playerArtifacts.UpdateStats();
        }
    }

    public override void healed(int healingAmount)
    {
        if ((float)PlayerProperties.playerScript.shipHealth / PlayerProperties.playerScript.shipHealthMAX > 0.5f)
        {
            artifactBonus.attackBonus = 0;
            artifactBonus.speedBonus = 0;
            PlayerProperties.playerArtifacts.UpdateStats();
        }
    }
}
