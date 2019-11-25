using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubMissionLoader : MonoBehaviour
{
    public StoryMission[] allStoryMissions;
    Dictionary<string, StoryMission> storyMissionDatabase = new Dictionary<string, StoryMission>();
    ReturnNotifications returnNotifications;

    private void Awake()
    {
        returnNotifications = FindObjectOfType<ReturnNotifications>();
        foreach(StoryMission mission in allStoryMissions)
        {
            storyMissionDatabase.Add(mission.missionID, mission);
        }
        addRewards();
    }

    void addRewards()
    {
        if (MiscData.missionID != null)
        {
            if (MiscData.finishedMission == false)
            {
                returnNotifications.updateRewards(0, 0, null, true, false);
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
                        }
                        else
                        {
                            if (HubProperties.vaultItems.Count < 8)
                            {
                                HubProperties.vaultItems.Add(spawnedItem.name);
                            }
                        }
                    }

                    returnNotifications.updateRewards(compMission.goldReward, compMission.skillPointReward, compMission.itemRewards, false, false);
                }
                else
                {
                    returnNotifications.updateRewards(0, 0, null, false, true);
                }
                SaveSystem.SaveGame();
            }
        }
    }

}
