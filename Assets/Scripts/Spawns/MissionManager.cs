using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public List<string> missionIDs;
    public List<StoryMission> missions;
    public List<BossManager> bossManagers;
    public StoryMission currMission;
    public Dictionary<string, BossManager> bossDict = new Dictionary<string, BossManager>();
    public Dictionary<string, StoryMission> missionDict = new Dictionary<string, StoryMission>();

    // If the player has moved to the boss room or not
    public bool bossInitiated = false;

    void Awake()
    {
        for(int i = 0; i < missionIDs.Count; i++)
        {
            if (i < bossManagers.Count && bossManagers[i] != null)
            {
                bossDict.Add(missionIDs[i], bossManagers[i]);
            }

            if (missions[i] != null)
            {
                missionDict.Add(missionIDs[i], missions[i]);
            }
        }

        currMission = missionDict[MiscData.missionID];
        FindObjectOfType<RoomTemplates>().setMaxRoomCount();
    }

    public void finishedMission()
    {
        MiscData.finishedMission = true;
        SaveSystem.SaveGame();
    }

    public void activateBossManager(int whichSide)
    {
        bossInitiated = true;
        bossDict[MiscData.missionID].startBossSequence(whichSide);
    }
}
