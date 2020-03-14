using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossSelectMenu : MonoBehaviour
{
    HubMissionLoader missionManager;
    public Button finalBossButton;
    public Button[] minorBossButtons;
    public GameObject blackWindow;
    public LevelSelectorMenu levelSelectMenu;
    bool isLoadingLevel = false;
    bool loadedBackToMenu = false;

    public BossHUDToolTip toolTip;

    //these need to be set when loading the menu
    string sceneToLoad;
    int whatLevel;

    private void Start()
    {
        missionManager = FindObjectOfType<HubMissionLoader>();
    }

    public void loadMissionIcons(int whichDungeonLevel, string sceneName)
    {
        missionManager = FindObjectOfType<HubMissionLoader>();

        sceneToLoad = sceneName;
        whatLevel = whichDungeonLevel;

        int completedAmount = 0;
        for (int i = 0; i < minorBossButtons.Length; i++)
        {
            // Set mission to the first + sequentially going upward mission
            StoryMission mission = missionManager.allStoryMissions[missionManager.dungeonLevelThresholds[whatLevel - 1] - 4 + i];

            // Set the button sprite to whatever the mission sprite is
            minorBossButtons[i].GetComponent<Image>().sprite = mission.missionIcon;

            // Adjusting the star display
            for (int k = 0; k < 5; k++)
            {
                // Accesses image objects and sets them to active depending on the difficulty level
                if (k < mission.difficulty)
                {
                    minorBossButtons[i].GetComponentInChildren<GridLayoutGroup>().transform.GetChild(k).gameObject.SetActive(true);
                }
                else
                {
                    minorBossButtons[i].GetComponentInChildren<GridLayoutGroup>().transform.GetChild(k).gameObject.SetActive(false);
                }
            }

            // if mission already completed, set completed image to active
            if (MiscData.completedMissions.Contains(mission.missionID))
            {
                minorBossButtons[i].GetComponentsInChildren<Image>()[1].enabled = true;
                completedAmount++;
            }
            else
            {
                minorBossButtons[i].GetComponentsInChildren<Image>()[1].enabled = false;
            }

            // Set the title to the name of the boss
            minorBossButtons[i].GetComponentInChildren<Text>().text = mission.bossName;
        }

        StoryMission bossMission = missionManager.allStoryMissions[missionManager.dungeonLevelThresholds[whatLevel - 1] - 1];

        finalBossButton.GetComponent<Image>().sprite = bossMission.missionIcon;
        for (int i = 0; i < 5; i++)
        {
            if (i < bossMission.difficulty)
            {
                finalBossButton.GetComponentInChildren<GridLayoutGroup>().transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                finalBossButton.GetComponentInChildren<GridLayoutGroup>().transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if (MiscData.completedMissions.Contains(bossMission.missionID))
        {
            finalBossButton.GetComponentsInChildren<Image>()[1].enabled = true;
        }
        else
        {
            finalBossButton.GetComponentsInChildren<Image>()[1].enabled = false;

            // If all previous three boss quests are complete, allow the player to challenge the final boss of the level
            if(completedAmount >= 3)
            {
                finalBossButton.GetComponentsInChildren<Image>()[2].enabled = false;
                finalBossButton.GetComponent<Image>().color = Color.white;
            }
            else
            {
                finalBossButton.GetComponentsInChildren<Image>()[2].enabled = true;
                finalBossButton.GetComponent<Image>().color = Color.grey;
                finalBossButton.enabled = false;
            }
        }


        finalBossButton.GetComponentInChildren<Text>().text = bossMission.bossName;
    }

    public void loadDungeonLevel(int whichButton)
    {
        if (isLoadingLevel == false)
        {
            MiscData.missionID = missionManager.allStoryMissions[missionManager.dungeonLevelThresholds[whatLevel - 1] - 4 + whichButton].missionID;
            MiscData.finishedMission = false;
            MiscData.numberDungeonRuns++;
            isLoadingLevel = true;
            Time.timeScale = 1;
            FindObjectOfType<AudioManager>().PlaySound("Dungeon Entry");
            StartCoroutine(fadeLoadScene(sceneToLoad, MiscData.missionID));
        }
    }

    public void returnToLevelSelect()
    {
        if(loadedBackToMenu == false)
        {
            StartCoroutine(transitionMenus());
            loadedBackToMenu = true;
        }
    }

    public void turnOnToolTip(int whatButton, Vector3 position)
    {
        toolTip.gameObject.SetActive(true);
        toolTip.GetComponent<RectTransform>().localPosition = position;
        StoryMission mission = missionManager.allStoryMissions[missionManager.dungeonLevelThresholds[whatLevel - 1] - 4 + whatButton];


        toolTip.updateRewards(mission.goldReward, mission.skillPointReward, mission.itemRewards, mission.bossInfo, MiscData.completedMissions.Contains(mission.missionID));
    }

    public void turnOffToolTip()
    {
        toolTip.gameObject.SetActive(false);
    }

    IEnumerator transitionMenus()
    {
        transform.parent.GetComponentInChildren<BlackOverlay>().transition();
        yield return new WaitForSeconds(1f);
        levelSelectMenu.transform.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        loadedBackToMenu = false;
    }

    IEnumerator fadeLoadScene(string whichSceneLoad, string missionID)
    {
        blackWindow.SetActive(true);
        blackWindow.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);

        AsyncOperation openScene = SceneManager.LoadSceneAsync(whichSceneLoad);
        Image loadingCircle = blackWindow.transform.GetChild(0).GetComponent<Image>();
        loadingCircle.gameObject.SetActive(true);
        loadingCircle.fillAmount = 0;
        openScene.allowSceneActivation = false;
        loadingCircle.transform.position = new Vector3(Screen.width - 50, 50);

        while (!openScene.isDone)
        {
            float progress = Mathf.Clamp01(openScene.progress / 0.9f);
            loadingCircle.fillAmount = progress;
            if (openScene.progress >= 0.9f)
            {
                openScene.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
