using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRewards : MonoBehaviour
{
    PlayerScript playerScript;
    public GameObject goldRewards, skillPointsRewards;
    public GameObject[] itemRewards;
    public GameObject noQuestText;

    public void loadRewardsMenu(int gold, int skillPoints, GameObject[] items)
    {
        goldRewards.GetComponentInChildren<Text>().text = gold.ToString();
        skillPointsRewards.GetComponentInChildren<Text>().text = skillPoints.ToString();
        foreach(GameObject slot in itemRewards)
        {
            slot.SetActive(false);
        }

        for (int i = 0; i < items.Length; i++)
        {
            itemRewards[i].SetActive(true);
            Image[] imageList = itemRewards[i].GetComponentsInChildren<Image>();
            imageList[1].sprite = items[i].GetComponent<DisplayItem>().displayIcon;
        }
    }

    public void noQuest()
    {
        goldRewards.SetActive(false);
        skillPointsRewards.SetActive(false);
        foreach(GameObject slot in itemRewards)
        {
            slot.SetActive(false);
        }
        noQuestText.SetActive(true);
    }
}
