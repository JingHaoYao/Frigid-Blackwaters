using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireReaperScytheArtifact : ArtifactBonus
{
    [SerializeField] GameObject fireSwipe;
    [SerializeField] DisplayItem displayItem;

    public void SummonFireSwipe()
    {
        PlayerProperties.playerArtifacts.numKills -= killRequirement;
        float angleToCursor = Mathf.Atan2(PlayerProperties.cursorPosition.y - PlayerProperties.playerShipPosition.y, PlayerProperties.cursorPosition.x - PlayerProperties.playerShipPosition.x);

        Instantiate(fireSwipe, PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angleToCursor), Mathf.Sin(angleToCursor)), Quaternion.Euler(0, 0, angleToCursor * Mathf.Rad2Deg + 90));
    }

    private void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    SummonFireSwipe();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    SummonFireSwipe();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    SummonFireSwipe();
                }
            }
        }
    }
}
