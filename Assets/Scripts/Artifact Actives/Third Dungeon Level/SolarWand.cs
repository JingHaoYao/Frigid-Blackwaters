using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarWand : MonoBehaviour
{
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;
    Camera mainCamera;
    public GameObject sunBall;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void spawnSunBall()
    {
        Vector3 spawnPosition = new Vector3(Mathf.Clamp(PlayerProperties.cursorPosition.x, -8.5f, 8.5f), Mathf.Clamp(PlayerProperties.cursorPosition.y, -8.5f, 8.5f));
        Instantiate(sunBall, spawnPosition, Quaternion.Euler(0, 0, Random.Range(0, 360)));
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    spawnSunBall();
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    spawnSunBall();
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    spawnSunBall();
                    PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
                }
            }
        }
    }
}
