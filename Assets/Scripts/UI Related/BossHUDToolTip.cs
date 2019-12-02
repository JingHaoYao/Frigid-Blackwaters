using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHUDToolTip : MonoBehaviour
{
    public Text infoText;
    public Image goldReward;
    public Image skillPointReward;
    public Image[] itemRewards;
    public Text bossDefeatedAlreadyText;

    public void updateRewards(int _goldReward, int _skillPointReward, GameObject[] _itemRewards, string _infoText, bool bossAlreadyDefeated)
    {
        infoText.text = _infoText;

        if (bossAlreadyDefeated == false)
        {
            goldReward.gameObject.SetActive(true);
            skillPointReward.gameObject.SetActive(true);
            foreach(Image slot in itemRewards)
            {
                slot.gameObject.SetActive(true);
            }
            bossDefeatedAlreadyText.enabled = false;

            goldReward.GetComponentInChildren<Text>().text = _goldReward.ToString();
            skillPointReward.GetComponentInChildren<Text>().text = _skillPointReward.ToString();

            for (int i = 0; i < itemRewards.Length; i++)
            {
                if (i < _itemRewards.Length)
                {
                    itemRewards[i].GetComponentsInChildren<Image>()[1].enabled = true;
                    itemRewards[i].GetComponentsInChildren<Image>()[1].sprite = _itemRewards[i].GetComponent<DisplayItem>().displayIcon;
                }
                else
                {
                    itemRewards[i].GetComponentsInChildren<Image>()[1].enabled = false;
                }
            }
        }
        else
        {
            goldReward.gameObject.SetActive(false);
            skillPointReward.gameObject.SetActive(false);
            foreach (Image slot in itemRewards)
            {
                slot.gameObject.SetActive(false);
            }
            bossDefeatedAlreadyText.enabled = true;
        }
    }
}
