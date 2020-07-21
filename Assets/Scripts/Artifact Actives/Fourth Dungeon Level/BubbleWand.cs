using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleWand : ArtifactBonus
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] GameObject bubble;

    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void summonBubbleProjectile()
    {
        PlayerProperties.playerArtifacts.numKills -= killRequirement;
        GameObject bubbleInstant = Instantiate(bubble, PlayerProperties.playerShipPosition, Quaternion.identity);
        bubbleInstant.GetComponent<BubbleWandBubbleProjectile>().targetLocation = new Vector3(
            Mathf.Clamp(PlayerProperties.cursorPosition.x, mainCamera.transform.position.x - 8.5f, mainCamera.transform.position.x + 8.5f),
            Mathf.Clamp(PlayerProperties.cursorPosition.y, mainCamera.transform.position.y - 8.5f, mainCamera.transform.position.y + 8.5f));
    }

    private void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    summonBubbleProjectile();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    summonBubbleProjectile();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    summonBubbleProjectile();
                }
            }
        }
    }
}
