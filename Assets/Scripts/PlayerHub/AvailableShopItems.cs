using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableShopItems : MonoBehaviour
{
    public string[] firstLevelConsumables;
    public string[] firstLevelArtifacts;

    public string[] secondLevelConsumables;
    public string[] secondLevelArtifacts;

    public string[] thirdLevelConsumables;
    public string[] thirdLevelArtifacts;

    public DialogueSet[] shopDialogues;
    NotificationBell shopNotifications;

    HubShop[] hubShops;
    private void Awake()
    {
        hubShops = FindObjectsOfType<HubShop>();

        loadDialogue(); 
    }

    public void loadDialogue()
    {
        foreach (HubShop shop in hubShops)
        {
            shop.dialogueIndicator.SetActive(false);
        }
    }
}
