using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsidianTalisman : ArtifactBonus
{
    [SerializeField] GameObject obsidianTower;
    [SerializeField] DisplayItem displayItem;
    [SerializeField] AudioSource audioSource;
    Camera mainCamera;
    GameObject obsidianTowerInstant;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void summonObsidianTower()
    {
        if (obsidianTowerInstant == null)
        {
            audioSource.Play();
            PlayerProperties.playerArtifacts.numKills -= killRequirement;
            Vector3 spawnPosition =
                new Vector3(
                Mathf.Clamp(PlayerProperties.cursorPosition.x, mainCamera.transform.position.x - 7.5f, mainCamera.transform.position.x + 7.5f),
                Mathf.Clamp(PlayerProperties.cursorPosition.y, mainCamera.transform.position.y - 7.5f, mainCamera.transform.position.y + 6.5f));
            obsidianTowerInstant = Instantiate(obsidianTower, spawnPosition, Quaternion.identity);
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
                    summonObsidianTower();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    summonObsidianTower();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    summonObsidianTower();
                }
            }
        }
    }
}
