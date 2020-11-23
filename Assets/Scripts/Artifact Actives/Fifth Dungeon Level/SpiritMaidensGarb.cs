using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMaidensGarb : ArtifactEffect
{
    bool withinImmunityFrame = false;
    [SerializeField] GameObject dashParticles;
    public override void playerDashed()
    {
        StartCoroutine(immunityFrame());
        Instantiate(dashParticles, PlayerProperties.playerShipPosition, Quaternion.Euler(0, 0, PlayerProperties.playerScript.whatAngleTraveled - 90));
    }

    public override void artifactUnequipped()
    {
        StopAllCoroutines();
        if(withinImmunityFrame)
        {
            PlayerProperties.playerScript.removeImmunityItem(this.gameObject);
        }
    }

    IEnumerator immunityFrame()
    {
        withinImmunityFrame = true;
        PlayerProperties.playerScript.addImmunityItem(this.gameObject);

        yield return new WaitForSeconds(0.5f);

        withinImmunityFrame = false;
        PlayerProperties.playerScript.removeImmunityItem(this.gameObject);
    }
}
