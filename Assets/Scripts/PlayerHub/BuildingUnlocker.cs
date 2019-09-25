using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUnlocker : MonoBehaviour
{
    ReturnNotifications notifications;
    bool appliedUnlocks;

    void Start()
    {
        notifications = FindObjectOfType<ReturnNotifications>();

        if (!MiscData.unlockedBuildings.Contains("tavern"))
        {
            MiscData.unlockedBuildings.Add("tavern");
        }

        if (!MiscData.unlockedBuildings.Contains("dungeon_entrance"))
        {
            MiscData.unlockedBuildings.Add("dungeon_entrance");
        }
    }

    DialogueSet loadDialogue(string dialogueName)
    {
        return Resources.Load<DialogueSet>("Dialogues/Building Unlock Dialogues/" + dialogueName);
    }

    private void LateUpdate()
    {
        if(appliedUnlocks == false)
        {
            appliedUnlocks = true;
            if (MiscData.numberDungeonRuns >= 1 && !MiscData.unlockedBuildings.Contains("provisions"))
            {
                notifications.dialoguesToDisplay.Add(loadDialogue("Quest and Provisions Unlocked"));
                MiscData.unlockedBuildings.Add("provisions");
                MiscData.unlockedBuildings.Add("quest_ship");
                MiscData.unlockedBuildings.Add("boss_quest_center");
            }

            if (MiscData.completedCheckPoints.Count >= 1 && !MiscData.unlockedBuildings.Contains("weapon_outfitter"))
            {
                notifications.dialoguesToDisplay.Add(loadDialogue("Weapon Outfitter Unlocked Dialogue"));
                MiscData.unlockedBuildings.Add("weapon_outfitter");
            }

            if (MiscData.completedCheckPoints.Count >= 2 && !MiscData.unlockedBuildings.Contains("shipsmith"))
            {
                notifications.dialoguesToDisplay.Add(loadDialogue("Shipsmith Unlocked Dialogue"));
                MiscData.unlockedBuildings.Add("shipsmith");
            }

            int numberNonGoldItems = 0;
            foreach (string id in PlayerItems.inventoryItemsIDs)
            {
                if (id != "GoldItem" && id != "GoldItem(Clone)")
                {
                    numberNonGoldItems++;
                }
            }

            if (numberNonGoldItems >= 3 && !MiscData.unlockedBuildings.Contains("golden_vault"))
            {
                notifications.dialoguesToDisplay.Add(loadDialogue("Golden Vault Unlocked Dialogue"));
                MiscData.unlockedBuildings.Add("golden_vault");
            }

            if (HubProperties.storeGold >= 1250 && !MiscData.unlockedBuildings.Contains("artifact_shop"))
            {
                notifications.dialoguesToDisplay.Add(loadDialogue("Artifact Shop Unlocked"));
                MiscData.unlockedBuildings.Add("artifact_shop");
            }

            this.gameObject.SetActive(false);
        }
    }
}
