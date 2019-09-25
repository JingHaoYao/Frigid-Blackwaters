using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    public QuestBoard questBoard;
    public BossQuestBoard bossQuestBoard;
    public GameObject targetQuest;
    bool acceptable = true;
    public GameObject acceptButton;
    public GameObject noLongerAccept;
    public Text questDescription;
    public Color acceptedColor;
    public GameObject goldReward;
    public GameObject skillPointReward;
    public GameObject[] itemRewards;
    public bool bossQuest = false;

    public void acceptQuest()
    {
        if (acceptable)
        {
            MiscData.currentQuestID = targetQuest.GetComponent<QuestType>().questID;
            acceptable = false;
            questDescription.color = acceptedColor;
            acceptButton.SetActive(false);
            foreach (QuestSlot slot in questBoard.questSlots)
            {
                if(slot != this)
                {
                    slot.disableQuest();
                }
            }

            if (bossQuest == false)
            {
                bossQuestBoard.bossQuestSlot.disableQuest();
                FindObjectOfType<AudioManager>().PlaySound("Boss Quest Accepted");
            }
            else
            {
                FindObjectOfType<AudioManager>().PlaySound("Normal Quest Accepted");
            }
        }

        for(int i = 0; i < 3; i++)
        {
            MiscData.availableQuests[i] = questBoard.questList[i].GetComponent<QuestType>().questID;
        }

        if (bossQuest)
        {
            MiscData.availableBossQuest = targetQuest.GetComponent<QuestType>().questID;
        }
        
        SaveSystem.SaveGame();
    }

    public void disableQuest()
    {
        noLongerAccept.SetActive(true);
        acceptable = false;
    }

    public void loadQuest(GameObject newTargetQuest)
    {
        targetQuest = newTargetQuest;
        questDescription.text = newTargetQuest.GetComponent<QuestType>().description;
        acceptable = true;
        noLongerAccept.SetActive(false);
        questDescription.color = Color.black;
    }

    public void loadRewards()
    {
        QuestType questType = targetQuest.GetComponent<QuestType>();
        goldReward.GetComponentInChildren<Text>().text = questType.rewardGoldAmount.ToString();
        skillPointReward.GetComponentInChildren<Text>().text = questType.rewardSkillPoints.ToString();
        for(int i = 0; i < questType.rewardObjects.Length; i++)
        {
            itemRewards[i].SetActive(true);
            Image[] images = itemRewards[i].GetComponentsInChildren<Image>();
            images[1].sprite = questType.rewardObjects[i].GetComponent<DisplayItem>().displayIcon;
        }
    }
}
