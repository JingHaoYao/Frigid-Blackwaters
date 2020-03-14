using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenSwordOfTheHero : MonoBehaviour
{
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;
    Camera mainCamera;
    public GameObject heroSlash;
    GameObject heroSlashInstant;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void summonHeroSlash()
    {
        if (heroSlashInstant == null)
        {
            PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
            heroSlashInstant = Instantiate(
                heroSlash,
                new Vector3(
                    Mathf.Clamp(PlayerProperties.cursorPosition.x, mainCamera.transform.position.x - 8, mainCamera.transform.position.x + 8),
                    Mathf.Clamp(PlayerProperties.cursorPosition.y, mainCamera.transform.position.y - 8, mainCamera.transform.position.y + 8)),
                Quaternion.identity);
        }
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    summonHeroSlash();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    summonHeroSlash();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    summonHeroSlash();
                }
            }
        }
    }
}
