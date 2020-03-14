using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowPlume : MonoBehaviour
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;
    float adjustedSpeedBonus = 0;
    float previousTravelAngle = -90;

    void Update()
    {
        if (displayItem.isEquipped)
        {
            if (Mathf.Abs(previousTravelAngle - PlayerProperties.currentPlayerTravelDirection) < 0.01f && PlayerProperties.shipTravellingVector.magnitude > 0.1f)
            {
 
                adjustedSpeedBonus = Mathf.Clamp(adjustedSpeedBonus += Time.deltaTime * 4, 0, 3);
                if (artifactBonus.speedBonus != adjustedSpeedBonus)
                {
                    artifactBonus.speedBonus = adjustedSpeedBonus;
                    PlayerProperties.playerArtifacts.UpdateUI();
                }
            }
            else
            {
                previousTravelAngle = PlayerProperties.currentPlayerTravelDirection;
                adjustedSpeedBonus = 0;
                if (artifactBonus.speedBonus != adjustedSpeedBonus)
                {
                    artifactBonus.speedBonus = adjustedSpeedBonus;
                    PlayerProperties.playerArtifacts.UpdateUI();
                }
            }
        }
    }
}
