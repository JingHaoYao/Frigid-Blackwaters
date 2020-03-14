using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunicDrill : MonoBehaviour
{
    public GameObject drillProjectile;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void spawnDrill()
    {
        Vector3 spawnPosition = new Vector3(
            Mathf.Clamp(PlayerProperties.cursorPosition.x, mainCamera.transform.position.x - 8, mainCamera.transform.position.y + 8),
            Mathf.Clamp(PlayerProperties.cursorPosition.y, mainCamera.transform.position.y - 8, mainCamera.transform.position.y + 8));
        Instantiate(drillProjectile, spawnPosition, Quaternion.identity);
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    spawnDrill();
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    spawnDrill();
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    spawnDrill();
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                }
            }
        }
    }
}
