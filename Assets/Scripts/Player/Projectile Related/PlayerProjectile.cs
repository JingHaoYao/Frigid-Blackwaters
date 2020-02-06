using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int whichWeaponFrom;
    public bool forceShot;
    public List<WeaponProperties.WeaponElement> weaponElements;

    public void triggerWeaponFireFlag()
    {
        if (whichWeaponFrom == 1)
        {
            foreach (ArtifactSlot slot in PlayerProperties.playerArtifacts.artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedFrontWeapon(new GameObject[1] { this.gameObject });
            }
        }
        else if (whichWeaponFrom == 2)
        {
            foreach (ArtifactSlot slot in PlayerProperties.playerArtifacts.artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedLeftWeapon(new GameObject[1] { this.gameObject });
            }
        }
        else
        {
            foreach (ArtifactSlot slot in PlayerProperties.playerArtifacts.artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedRightWeapon(new GameObject[1] { this.gameObject });
            }
        }
    }
}
