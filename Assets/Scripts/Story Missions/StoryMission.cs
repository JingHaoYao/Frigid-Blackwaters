using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryMission
{
    public Sprite missionIcon;
    public GameObject[] itemRewards;
    public int goldReward;
    public int skillPointReward;
    public string missionID;
    public string bossName;
    public int numberDungeonRooms;
    public int difficulty;
    public string bossInfo;
}
