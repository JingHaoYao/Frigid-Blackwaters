using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilentShroud : ArtifactBonus
{
    public GameObject shroud;
    [SerializeField] DisplayItem displayItem;
    private GameObject shroudInstant;

    IEnumerator invulnerableLoop(float duration)
    {
        float immunueDuration = duration;
        while (duration > 0)
        {
            PlayerProperties.playerScript.damageImmunity = true;
            duration -= Time.deltaTime;
            yield return null;
        }
        PlayerProperties.playerScript.damageImmunity = false;
        shroudInstant.GetComponent<SilentShroudShroud>().fadeOut();
    }

    void summonShroud()
    {
        if (shroudInstant == null)
        {
            PlayerProperties.durationUI.addTile(displayItem.displayIcon, 6);
            PlayerProperties.playerArtifacts.numKills -= killRequirement;
            shroudInstant = Instantiate(shroud, PlayerProperties.playerShipPosition + new Vector3(0, -1.2f, 0), Quaternion.identity);
            StartCoroutine(invulnerableLoop(6));
        }
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    summonShroud();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    summonShroud();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    summonShroud();
                }
            }
        }
    }
}
