using System.Collections;
using UnityEngine;

public class ConjuringWand : ArtifactBonus
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] GameObject missileProjectile;
    bool activated = false;

    IEnumerator summonProjectiles()
    {
        activated = true;
        PlayerProperties.playerArtifacts.numKills -= killRequirement;

        for (int i = 0; i < 3; i++)
        {
            for (int k = 0; k < 3; k++) {
                float angle = k * (2 * Mathf.PI / 3);
                Instantiate(missileProjectile, PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 1.5f, Quaternion.identity);
            }
            yield return new WaitForSeconds(1);
        }

        activated = false;
    }

    void activateProjectiles()
    {
        if (!activated)
        {
            StartCoroutine(summonProjectiles());
        }
    }

    private void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    activateProjectiles();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    activateProjectiles();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    activateProjectiles();
                }
            }
        }
    }
}
