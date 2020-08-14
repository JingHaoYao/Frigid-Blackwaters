using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSteelArrow : ArtifactBonus
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject chargeBall;

    void fireArrow()
    {
        Vector3 positionToSpawn = PlayerProperties.playerShipPosition;
        PlayerProperties.playerArtifacts.numKills -= killRequirement;

        Instantiate(chargeBall, positionToSpawn, Quaternion.identity);

        float angle = Mathf.Atan2(PlayerProperties.cursorPosition.y - positionToSpawn.y, PlayerProperties.cursorPosition.x - positionToSpawn.x) * Mathf.Rad2Deg;

        for(int i = 0; i < 3; i++)
        {
            float attackAngle = angle - 15 + 15 * i;
            GameObject arrowInstant = Instantiate(arrow, positionToSpawn, Quaternion.identity);
            arrowInstant.GetComponent<BlueSteelArrowProjectile>().Initialize(attackAngle * Mathf.Deg2Rad);
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
                    fireArrow();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    fireArrow();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    fireArrow();
                }
            }
        }
    }
}
