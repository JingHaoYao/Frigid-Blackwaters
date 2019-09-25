using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBoard : MonoBehaviour
{
    public List<GameObject> questList = new List<GameObject>();
    public QuestSlot[] questSlots;
    QuestRewarder rewarder;
    int currDungeonLevel = MiscData.dungeonLevelUnlocked;
    public Text whatDungeonText;
    public Sprite[] upcomingBossIcons;
    public int[] upcomingBossRequirements;
    public Image bossIcon;
    public Text bossMessage;

    public void Start()
    {
        rewarder = GameObject.Find("QuestRewarder").GetComponent<QuestRewarder>();
        if (MiscData.availableQuests[0] != "empty_quest")
        {
            loadPreviousQuests();
            for (int i = 0; i < 3; i++)
            {
                MiscData.availableQuests[i] = questList[i].GetComponent<QuestType>().questID;
            }
        }
        else
        {
            //need some way of generating new quests
            generateDungeonQuests(MiscData.dungeonLevelUnlocked);
            whatDungeonText.text = MiscData.dungeonLevelUnlocked.ToString();
            UpdateUI();
            for(int i = 0; i < 3; i++)
            {
                MiscData.availableQuests[i] = questList[i].GetComponent<QuestType>().questID;
            }
            MiscData.currentQuestID = "empty_quest";
        }
        this.gameObject.SetActive(false);
    }

    public void shiftDungeonQuests()
    {
        FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
        if (MiscData.currentQuestID == "empty_quest")
        {
            currDungeonLevel++;
            if (currDungeonLevel > MiscData.dungeonLevelUnlocked)
            {
                currDungeonLevel = 1;
            }
            whatDungeonText.text = currDungeonLevel.ToString();
            generateDungeonQuests(currDungeonLevel);
            UpdateUI();
            for (int i = 0; i < 3; i++)
            {
                MiscData.availableQuests[i] = questList[i].GetComponent<QuestType>().questID;
            }
            MiscData.currentQuestID = "empty_quest";
        }
    }

    bool contains(string[] array, string id)
    {
        foreach(string line in array)
        {
            if(id == line)
            {
                return true;
            }
        }
        return false;
    }

    public void loadPreviousQuests()
    {
        for(int i = 0; i < 3; i++) {
            questList.Add(rewarder.questDatabase[MiscData.availableQuests[i]]);
            questSlots[i].loadQuest(questList[i]);
            questSlots[i].loadRewards();
        }
        if (contains(MiscData.availableQuests, MiscData.currentQuestID))
        {
            foreach(QuestSlot slot in questSlots)
            {
                if (slot.targetQuest.GetComponent<QuestType>().questID == MiscData.currentQuestID)
                {
                    slot.acceptQuest();
                }
                else
                {
                    slot.disableQuest();
                }
            }
        }
        else
        {
            MiscData.currentQuestID = "empty_quest";
        }
        SaveSystem.SaveGame();
    }

    GameObject createQuest(int whatDungeonUnlocked)
    {
        if (whatDungeonUnlocked == 1)
        {
            return rewarder.allFirstLevelQuests[Random.Range(1, rewarder.allFirstLevelQuests.Length)];
        }
        else if(whatDungeonUnlocked == 2)
        {
            return rewarder.allSecondLevelQuests[Random.Range(1, rewarder.allSecondLevelQuests.Length)];
        }
        else
        {
            return rewarder.allFirstLevelQuests[Random.Range(1, rewarder.allFirstLevelQuests.Length)];
        }
    }

    public void generateDungeonQuests(int whatDungeonLevelUnlocked)
    {
        questList.Clear();
        for (int i = 0; i < 3; i++) {
            GameObject targetQuest = createQuest(whatDungeonLevelUnlocked);
            while (questList.Contains(targetQuest))
            {
                targetQuest = createQuest(whatDungeonLevelUnlocked);
            }
            questList.Add(targetQuest);
        }
    }

    void updateBossMessage()
    {
        int whatBoss;
        if (!MiscData.bossesDefeated.Contains("undead_mariner"))
        {
            whatBoss = 0;
        }
        else if (!MiscData.bossesDefeated.Contains("elder_frost_mage"))
        {
            whatBoss = 0;
        }
        else
        {
            whatBoss = -1;
        }

        if(whatBoss == -1)
        {
            bossIcon.enabled = false;
            bossMessage.text = "No more quest bosses available";
        }
        else
        {
            bossIcon.enabled = true;
            bossIcon.sprite = upcomingBossIcons[whatBoss];
            if (MiscData.numberQuestsCompleted >= upcomingBossRequirements[whatBoss])
            {
                bossMessage.text = "The next quest boss is available.";
            }
            else
            {
                bossMessage.text = "The next quest boss is available in " + (upcomingBossRequirements[whatBoss] - MiscData.numberQuestsCompleted).ToString() + " more completed quests";
            }
        }
    }

    public void UpdateUI()
    {
        for(int i = 0; i < 3; i++)
        {
            questSlots[i].loadQuest(questList[i]);
            questSlots[i].loadRewards();
        }
        updateBossMessage();
        SaveSystem.SaveGame();
    }
}
