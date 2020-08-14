using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastKnightsHelmet : ArtifactBonus
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject beam;
    [SerializeField] GameObject chargeBall;
    bool isBeaming = false;

    void fireBeams()
    {
        if (isBeaming == false)
        {
            Vector3 positionToSpawn = PlayerProperties.playerShipPosition;
            PlayerProperties.playerArtifacts.numKills -= killRequirement;

            Instantiate(chargeBall, positionToSpawn, Quaternion.identity);

            StartCoroutine(spawnBeamAttack(positionToSpawn));
        }
    }

    IEnumerator spawnBeamAttack(Vector3 spawnPos)
    {
        isBeaming = true;
        yield return new WaitForSeconds(3 / 12f);

        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90;
            GameObject beamInstant = Instantiate(beam, spawnPos, Quaternion.Euler(0, 0, angle));
        }

        yield return new WaitForSeconds(4 / 12f);

        isBeaming = false;
    }

    private void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    fireBeams();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    fireBeams();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    fireBeams();
                }
            }
        }
    }
}
