using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkBrooch : MonoBehaviour
{
    public GameObject blinkEffect;
    [SerializeField] ArtifactBonus artifactBonus;
    [SerializeField] DisplayItem displayItem;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void teleport()
    {
        if (!Physics2D.OverlapCircle(PlayerProperties.cursorPosition, 0.5f, 12))
        {
            PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
            float angleToCursor = Mathf.Atan2(PlayerProperties.cursorPosition.y - PlayerProperties.playerShipPosition.y, PlayerProperties.cursorPosition.x - PlayerProperties.playerShipPosition.x);
            Vector3 positionToTeleport = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angleToCursor), Mathf.Sin(angleToCursor)) * 4;
            Instantiate(blinkEffect, PlayerProperties.playerShipPosition, Quaternion.identity);
            PlayerProperties.playerShip.transform.position = new Vector3(
                Mathf.Clamp(positionToTeleport.x, mainCamera.transform.position.x - 8, mainCamera.transform.position.x + 8),
                Mathf.Clamp(positionToTeleport.y, mainCamera.transform.position.y - 8, mainCamera.transform.position.y + 8));
            Instantiate(blinkEffect, new Vector3(
                Mathf.Clamp(positionToTeleport.x, mainCamera.transform.position.x - 8, mainCamera.transform.position.x + 8),
                Mathf.Clamp(positionToTeleport.y, mainCamera.transform.position.y - 8, mainCamera.transform.position.y + 8)), Quaternion.identity);
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
                    teleport();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    teleport();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    teleport();
                }
            }
        }
    }
}
