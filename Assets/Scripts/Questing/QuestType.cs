using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestType: MonoBehaviour
{
    public string description; //quest description
    public int requirementAmount; //required amount for goal
    public int currentAmount; //current progress towards goal
    public bool completed;
    public string questID;
    public int whatDungeonLevel = 1;

    public int rewardGoldAmount;
    public GameObject[] rewardObjects;
    public int rewardSkillPoints;

    public void Evaluate()
    {
        if (currentAmount >= requirementAmount && completed == false)
        {
            Complete();
        }
    }

    public void Complete()
    {
        completed = true;
        MiscData.finishedQuest = true;
    }
}
