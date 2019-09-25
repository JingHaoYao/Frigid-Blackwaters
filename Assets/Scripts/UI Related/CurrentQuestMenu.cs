using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentQuestMenu : MonoBehaviour
{
    public GameObject questMenu;
    public Text questDescription;
    public Text progressDescription;
    public Text goldReward;
    public Text skillPointReward;
    public Image[] rewardItems;

    QuestManager questManager;

    PlayerScript playerScript;

    public GameObject qSymbol;

    void Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    private void LateUpdate()
    {
        if (questMenu.activeSelf == false)
        {
            if (playerScript.windowAlreadyOpen == false)
            {
                if (Input.GetKeyDown(KeyCode.Q) && questManager.currentQuest.GetComponent<QuestType>().questID != "empty_quest")
                {

                    if(MiscData.questSymbolShown == false && qSymbol.activeSelf == true)
                    {
                        MiscData.questSymbolShown = true;
                    }
                    Time.timeScale = 0;
                    playerScript.windowAlreadyOpen = true;
                    questMenu.SetActive(true);
                    UpdateUI();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
            {
                Time.timeScale = 1;
                playerScript.windowAlreadyOpen = false;
                questMenu.SetActive(false);
            }
        }


        if(questManager.currentQuest.GetComponent<QuestType>().questID != "empty_quest" && questManager.currentQuest.GetComponent<QuestType>().currentAmount >= 1 && MiscData.questSymbolShown == false)
        {
            qSymbol.SetActive(true);
        }
    }

    public void UpdateUI()
    {
        questDescription.text = questManager.currentQuest.GetComponent<QuestType>().description;
        progressDescription.text = "Progress: " + questManager.currentQuest.GetComponent<QuestType>().currentAmount.ToString() + "/" + questManager.currentQuest.GetComponent<QuestType>().requirementAmount.ToString();
        goldReward.text = questManager.currentQuest.GetComponent<QuestType>().rewardGoldAmount.ToString();
        skillPointReward.text = questManager.currentQuest.GetComponent<QuestType>().rewardSkillPoints.ToString();
        for(int i = 0; i < questManager.currentQuest.GetComponent<QuestType>().rewardObjects.Length; i++)
        {
            rewardItems[i].transform.parent.gameObject.SetActive(true);
            rewardItems[i].sprite = questManager.currentQuest.GetComponent<QuestType>().rewardObjects[i].GetComponent<DisplayItem>().displayIcon;
        }
    }
}
