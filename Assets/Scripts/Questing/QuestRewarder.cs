using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRewarder : MonoBehaviour
{
    public GameObject[] allQuests;
    public GameObject[] allFirstLevelQuests;
    public GameObject[] allSecondLevelQuests;

    public GameObject[] bossQuests;
    public Dictionary<string, GameObject> questDatabase = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> bossQuestDatabase = new Dictionary<string, GameObject>();

    public GameObject currentQuest;

    public QuestRewards questRewardsMenu;

    void Awake()
    {
        foreach (GameObject quest in allQuests)
        {
            questDatabase.Add(quest.GetComponent<QuestType>().questID, quest);
        }
        foreach (GameObject quest in bossQuests)
        {
            bossQuestDatabase.Add(quest.GetComponent<QuestType>().questID, quest);
        }
        loadQuest(MiscData.currentQuestID);
        addRewards();
    }

    public void addRewards()
    {
        if (MiscData.finishedQuest)
        {
            QuestType questConf = currentQuest.GetComponent<QuestType>();
            questRewardsMenu.gameObject.SetActive(true);
            questRewardsMenu.loadRewardsMenu(questConf.rewardGoldAmount, questConf.GetComponent<QuestType>().rewardSkillPoints, questConf.rewardObjects);
            HubProperties.storeGold += questConf.rewardGoldAmount;
            PlayerUpgrades.numberMaxSkillPoints += questConf.rewardSkillPoints;
            PlayerUpgrades.numberSkillPoints += questConf.rewardSkillPoints;
            foreach(GameObject rewardItem in questConf.rewardObjects)
            {
                if(HubProperties.vaultItems.Count < 8)
                {
                    HubProperties.vaultItems.Add(rewardItem.name);
                }
            }
            MiscData.numberQuestsCompleted++;
        }
        else
        {
            questRewardsMenu.noQuest();
        }
        MiscData.finishedQuest = false;
        SaveSystem.SaveGame();
    }

    public void loadQuest(string questID)
    {
        currentQuest = questDatabase[questID];
    }
}
