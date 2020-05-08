using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonSelectorButton : MonoBehaviour {
    public string whichSceneLoad;
    public bool locked = false;
    public GameObject lockedImage, levelDescriptor;
    public GameObject questIcon;
    public GameObject blackWindow;
    public int whatDungeonLevel = 1;
    bool alreadyLoadedScene = false;

    public void lockButton()
    {
        locked = true;
        if(lockedImage != null)
            lockedImage.SetActive(true);
    }

    public void unlockButton()
    {
        locked = false;
        if(lockedImage != null)
            lockedImage.SetActive(false);
    }

    IEnumerator fadeLoadScene(int whichScene = -1)
    {
        blackWindow.SetActive(true);
        blackWindow.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);

        AsyncOperation openScene;
        if (whichScene != -1)
        {
            openScene = SceneManager.LoadSceneAsync(whichScene);
        }
        else
        {
            openScene = SceneManager.LoadSceneAsync(whichSceneLoad);
        }
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

    void pickPlayerHubToLoad()
    {
        if(whichSceneLoad == "Player Hub")
        {
            if (MiscData.dungeonLevelUnlocked == 3)
            {
                StartCoroutine(fadeLoadScene(5));
            }
            else if(MiscData.dungeonLevelUnlocked == 2)
            {
                StartCoroutine(fadeLoadScene(7));
            }
            else
            {
                StartCoroutine(fadeLoadScene(1));
            }
        }
        else
        {
            StartCoroutine(fadeLoadScene());
        }
    }

    public void loadScene()
    {
        if (locked == false && alreadyLoadedScene == false)
        {
            Time.timeScale = 1;
            alreadyLoadedScene = true;
            pickPlayerHubToLoad();
            FindObjectOfType<AudioManager>().PlaySound("Dungeon Entry");
        }
    }

    private void OnEnable()
    {
        if(MiscData.dungeonLevelUnlocked >= whatDungeonLevel)
        {
            locked = false;
        }

        if (locked == true)
        {
            lockButton();
        }
        else
        {
            unlockButton();
        }
    }
}
