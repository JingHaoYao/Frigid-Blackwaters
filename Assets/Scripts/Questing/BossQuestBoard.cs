using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossQuestBoard : MonoBehaviour
{
    public QuestSlot bossQuestSlot;
    public GameObject bossQuest;
    QuestRewarder rewarder;
    public Shipsmith bossQuestCenter;
    public GameObject bossIcon;
    string bossNotification = "Boss Quest Available";
    NotificationBell notifications;

    void turnOnBossCenter()
    {
        notifications.startNotification(bossNotification);
        bossIcon.SetActive(true);
        bossQuestCenter.enabled = true;
    }

    private void Start()
    {
        if (GameObject.Find("Boss Quest Notifier"))
        {
            notifications = GameObject.Find("Boss Quest Notifier").GetComponent<NotificationBell>();
        }

        rewarder = GameObject.Find("QuestRewarder").GetComponent<QuestRewarder>();
        if (MiscData.availableBossQuest != "empty_quest")
        {
            bossQuest = rewarder.questDatabase[MiscData.availableBossQuest];
            MiscData.availableBossQuest = bossQuest.GetComponent<QuestType>().questID;
        }
        else
        {
            string whichQuest = pickBossToGenerate();
            MiscData.availableBossQuest = whichQuest;
            generateBossQuest(whichQuest);
        }
        UpdateUI();
        
        if(bossQuest.GetComponent<QuestType>().questID != "empty_quest")
        {
            turnOnBossCenter();
        }
        else
        {
            if (notifications != null)
            {
                notifications.gameObject.SetActive(false);
            }
        }

        this.gameObject.SetActive(false);
        SaveSystem.SaveGame();
    }

    public void UpdateUI()
    {
        bossQuestSlot.loadQuest(bossQuest);
        bossQuestSlot.loadRewards();
    }

    public void generateBossQuest(string bossQuestID)
    {
        bossQuest = rewarder.bossQuestDatabase[bossQuestID];
    }

    string pickBossToGenerate()
    {
        if (!MiscData.bossesDefeated.Contains("undead_mariner") && MiscData.numberQuestsCompleted >= 5){
            return "defeat_the_undead_mariner";
        }
        else if(!MiscData.bossesDefeated.Contains("elder_frost_mage") && MiscData.bossesDefeated.Count >= 1 && MiscData.numberQuestsCompleted >= 10)
        {
            return "defeat_the_elder_frost_mage";
        }
        else
        {
            return "empty_quest";
        }
    }
}
