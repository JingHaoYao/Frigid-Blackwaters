using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorMenu : MonoBehaviour
{
    public Button[] levelSelectorList;
    public BossSelectMenu menu;
    bool selectedMenu = false;
    public string[] levelNames;

    public void loadLevel(int whichLevel)
    {
        if (selectedMenu == false && MiscData.dungeonLevelUnlocked >= whichLevel)
        {
            StartCoroutine(transitionMenus(whichLevel, levelNames[whichLevel - 1]));
            selectedMenu = true;
        }
    }

    IEnumerator transitionMenus(int whichLevel, string levelName)
    {
        transform.parent.GetComponentInChildren<BlackOverlay>().transition();
        yield return new WaitForSeconds(1f);
        menu.transform.gameObject.SetActive(true);
        menu.loadMissionIcons(whichLevel, levelName);
        this.gameObject.SetActive(false);
        selectedMenu = false;
    }

    private void OnEnable()
    {
        for(int i = 0; i < levelSelectorList.Length; i++)
        {
            if (MiscData.dungeonLevelUnlocked >= i + 1)
            {
                levelSelectorList[i].GetComponentsInChildren<Image>()[1].enabled = false;
            }
            else
            {
                levelSelectorList[i].GetComponentsInChildren<Image>()[1].enabled = true;
            }
        }
    }
}
