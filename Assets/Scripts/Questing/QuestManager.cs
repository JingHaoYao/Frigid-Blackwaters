using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public string[] questNames;
    public string[] questIDList;
    public Dictionary<string, string> questDB = new Dictionary<string, string>();
    public GameObject currentQuest;
    public GameObject questCompletedEffect;
    bool playedQuestCompleted = false;

    private void Update()
    {
        if (currentQuest.GetComponent<QuestType>().completed == true && playedQuestCompleted == false)
        {
            playedQuestCompleted = true;
            questCompletedEffect.SetActive(true);
        }
    }

    QuestType loadQuestResource(int whatLevel, string quest_id)
    {
        if(quest_id == "empty_quest")
        {
            return Resources.Load<QuestType>("Quests/Empty Quest");
        }

        if (questDB.ContainsKey(quest_id))
        {
            if (whatLevel == 1)
            {
                return Resources.Load<QuestType>("Quests/First Dungeon Quests/" + questDB[quest_id]);
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    private void Awake()
    {
        /*questNames = new string[allQuests.Length];
        questIDList = new string[allQuests.Length];

        for (int i = 0; i < questNames.Length; i++)
        {
            questNames[i] = allQuests[i].name;
        }

        for (int i = 0; i < questIDList.Length; i++)
        {
            questIDList[i] = allQuests[i].GetComponent<QuestType>().questID;
        }

        for (int i = 0; i < questIDList.Length; i++)
        {
            questDB.Add(questIDList[i], questNames[i]);
        }*/

        for(int i = 0; i < questIDList.Length; i++)
        {
            questDB.Add(questIDList[i], questNames[i]);
        }

        loadQuest(loadQuestResource(FindObjectOfType<DungeonEntryDialogueManager>().whatDungeonLevel, MiscData.currentQuestID).gameObject);
        MiscData.availableQuests = new string[3] { "empty_quest", "empty_quest", "empty_quest" };
        MiscData.availableBossQuest = "empty_quest";
    }

    public void addKill(string id)
    {
        if (currentQuest.GetComponent<KillQuest>())
        {
            currentQuest.GetComponent<KillQuest>().progressQuest(id);
        }
    }

    public void addExamine(GameObject examinedObject)
    {
        if (currentQuest.GetComponent<ExamineQuest>())
        {
            currentQuest.GetComponent<ExamineQuest>().progressQuest(examinedObject);
        }
    }

    public void addItemCollect(List<GameObject> itemList)
    {
        if (currentQuest.GetComponent<GatherQuest>())
        {
            currentQuest.GetComponent<GatherQuest>().progressQuest(itemList);
        }
    }

    public void addExplore(int numberRoomsVisited)
    {
        if (currentQuest.GetComponent<ExploreQuest>())
        {
            currentQuest.GetComponent<ExploreQuest>().progressQuest(numberRoomsVisited);
        }
    }

    public void loadQuest(GameObject quest)
    {
        currentQuest = Instantiate(quest);
    }
}
