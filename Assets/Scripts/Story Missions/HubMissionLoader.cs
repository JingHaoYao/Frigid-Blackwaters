using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubMissionLoader : MonoBehaviour
{
    public StoryMission[] allStoryMissions;
    Dictionary<string, StoryMission> storyMissionDatabase = new Dictionary<string, StoryMission>();
    ReturnNotifications returnNotifications;
    public BuildingUnlocker buildingUnlocker;

    // Array of how many bosses are in each dungeon level, used by other scripts to filter the ordered mission list
    public int[] dungeonLevelThresholds;

    private void Awake()
    {
        returnNotifications = FindObjectOfType<ReturnNotifications>();
        foreach (StoryMission mission in allStoryMissions)
        {
            storyMissionDatabase.Add(mission.missionID, mission);
        }

        StartCoroutine(updateAfterEndOfFrame());

        if (MiscData.missionID != null && (MiscData.finishedTutorial == true || MiscData.dungeonLevelUnlocked > 1))
        {
            returnNotifications.activateNotifications();
        }
        else
        {
            returnNotifications.closeNotifications();
        }
    }

    IEnumerator updateAfterEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        addRewards();

        MiscData.finishedMission = false;
        MiscData.missionID = null;
    }

    void addRewards()
    {
        PlayerProperties.playerScript.loadPrevItems();
        buildingUnlocker?.unlockDialogues();
        if (MiscData.missionID != null) 
        {
            if (MiscData.finishedMission == false)
            {
                StoryMission mission = storyMissionDatabase[MiscData.missionID];
                returnNotifications.updateRewards(0, 0, null, mission.missionIcon, true, false);
            }
            else
            {
                if (!MiscData.completedMissions.Contains(MiscData.missionID))
                {
                    MiscData.completedMissions.Add(MiscData.missionID);
                    StoryMission compMission = storyMissionDatabase[MiscData.missionID];
                    HubProperties.storeGold += compMission.goldReward;
                    PlayerUpgrades.numberMaxSkillPoints += compMission.skillPointReward;
                    PlayerUpgrades.numberSkillPoints += compMission.skillPointReward;

                    foreach (GameObject item in compMission.itemRewards)
                    {
                        GameObject spawnedItem = Instantiate(item);
                        spawnedItem.transform.SetParent(GameObject.Find("PresentItems").transform);

                        if (PlayerItems.inventoryItemsIDs.Count < PlayerItems.maxInventorySize)
                        {
                            PlayerItems.inventoryItemsIDs.Add(spawnedItem.name);
                            PlayerProperties.playerInventory.itemList.Add(spawnedItem);
                        }
                        else
                        {
                            HubProperties.vaultItems.Add(spawnedItem.name);
                            FindObjectOfType<GoldenVault>().vaultItems.Add(spawnedItem);
                        }
                    }

                    returnNotifications.updateRewards(compMission.goldReward, compMission.skillPointReward, compMission.itemRewards, compMission.missionIcon, false, false);
                }
                else
                {
                    StoryMission mission = storyMissionDatabase[MiscData.missionID];
                    returnNotifications.updateRewards(0, 0, null, mission.missionIcon, false, true);
                }
                SaveSystem.SaveGame();
            }
        }
    }

}
