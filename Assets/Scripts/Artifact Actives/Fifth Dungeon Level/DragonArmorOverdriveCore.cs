using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonArmorOverdriveCore : ArtifactEffect
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] GameObject fireWave;

    private float angleToCursor()
    {
        return Mathf.Atan2(PlayerProperties.cursorPosition.y - PlayerProperties.playerShipPosition.y, PlayerProperties.cursorPosition.x - PlayerProperties.playerShipPosition.x) * Mathf.Rad2Deg;
    }

    IEnumerator summonFireWave()
    {
        artifactBonus.speedBonus += 6;
        PlayerProperties.playerArtifacts.UpdateStats();
        PlayerProperties.durationUI.addTile(displayItem.displayIcon, 5f);

        GameObject fireWaveInstant = Instantiate(fireWave, PlayerProperties.playerShipPosition, Quaternion.identity);
        Vector3 positionAway = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angleToCursor() * Mathf.Deg2Rad), Mathf.Sin(angleToCursor() * Mathf.Deg2Rad)) * 60;

        fireWaveInstant.GetComponent<SkyClimberFireWave>().Initialize(angleToCursor(), this.gameObject, positionAway, 5f);

        yield return new WaitForSeconds(5f);

        artifactBonus.speedBonus -= 6;
        PlayerProperties.playerArtifacts.UpdateStats();
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    StartCoroutine(summonFireWave());
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    StartCoroutine(summonFireWave());
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                    StartCoroutine(summonFireWave());
                }
            }
        }
    }
}
