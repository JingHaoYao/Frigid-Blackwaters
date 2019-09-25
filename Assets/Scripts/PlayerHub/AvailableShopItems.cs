using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableShopItems : MonoBehaviour
{
    public string[] firstLevelConsumables;
    public string[] firstLevelArtifacts;

    public string[] secondLevelConsumables;
    public string[] secondLevelArtifacts;

    public DialogueSet[] shopDialogues;
    NotificationBell shopNotifications;
    string shopNotification = "New Dialogue Available at the Shops";

    HubShop[] hubShops;
    private void Awake()
    {
        shopNotifications = GameObject.Find("Dialogue Notifications").GetComponent<NotificationBell>();
        hubShops = FindObjectsOfType<HubShop>();

        /*firstLevelConsumables = new string[firstDungeonLevelConsumables.Length];
        firstLevelArtifacts = new string[firstDungeonLevelArtifacts.Length];

        for(int i = 0; i < firstLevelConsumables.Length; i++)
        {
            firstLevelConsumables[i] = firstDungeonLevelConsumables[i].name;
        }

        for(int i = 0; i < firstLevelArtifacts.Length; i++)
        {
            firstLevelArtifacts[i] = firstDungeonLevelArtifacts[i].name;
        }*/

        loadDialogue(); 
    }

    public void loadDialogue()
    {
        /*if(MiscData.completedTavernDialogues.Count >= 6 && MiscData.completedShopDialogues.Count == 0)
        {
            shopNotifications.dialoguesAvailable.Add("Shop");
            foreach (HubShop shop in hubShops)
            {
                shop.loadedDialogue = shopDialogues[0];
                shop.dialogueIndicator.SetActive(true);
            }
        }
        else if(MiscData.completedTavernDialogues.Count >= 6 && MiscData.completedShopDialogues.Count == 1 && MiscData.numberDungeonRuns >= 15)
        {
            shopNotifications.dialoguesAvailable.Add("Shop");
            foreach (HubShop shop in hubShops)
            {
                shop.loadedDialogue = shopDialogues[1];
                shop.dialogueIndicator.SetActive(true);
            }
        }
        else if(MiscData.completedTavernDialogues.Count >= 6 && MiscData.completedShopDialogues.Count == 1 && MiscData.numberDungeonRuns >= 17)
        {
            shopNotifications.dialoguesAvailable.Add("Shop");
            foreach (HubShop shop in hubShops)
            {
                shop.loadedDialogue = shopDialogues[2];
                shop.dialogueIndicator.SetActive(true);
            }
        }
        else
        {
            foreach (HubShop shop in hubShops)
            {
                shop.dialogueIndicator.SetActive(false);
            }
        }*/
        foreach (HubShop shop in hubShops)
        {
            shop.dialogueIndicator.SetActive(false);
        }
    }
}
