using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameFromTitleScreen : MonoBehaviour {
    bool clicked = false;
    public GameObject blackWindow;
    public GameObject objectToOpen;

    private void Start()
    {
        Cursor.visible = true;
    }

    public void open()
    {
        objectToOpen.SetActive(true);
    }

    
	public void loadPlayerHub()
    {
        if (clicked == false)
        {
            SaveData data = SaveSystem.GetSave();
            SaveSystem.loadData(data);

            StartCoroutine(fadeLoadScene());

            FindObjectOfType<AudioManager>().PlaySound("Play Button");

            clicked = true;
        }
    }

    int whichPlayerHubToLoad()
    {
        switch (MiscData.dungeonLevelUnlocked)
        {
            case 1:
                return 1;
            case 2:
                return 7;
            case 3:
                return 5;
            case 4:
                return 8;
            case 5:
                return 11;
        }

        return 1;
    }

    IEnumerator fadeLoadScene()
    {
        blackWindow.SetActive(true);
        blackWindow.GetComponent<Animator>().GetComponent<Animator>().SetTrigger("FadeOut");
        FindObjectOfType<AudioManager>().FadeOut("Title Screen Music", 0.2f);
        yield return new WaitForSeconds(1f);
        if (MiscData.finishedTutorial == false)
        {
            AsyncOperation openScene = SceneManager.LoadSceneAsync(3);
            Image loadingCircle = blackWindow.transform.GetChild(0).GetComponent<Image>();
            loadingCircle.gameObject.SetActive(true);
            loadingCircle.fillAmount = 0;
            openScene.allowSceneActivation = false;

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
        else
        {
            AsyncOperation openScene = SceneManager.LoadSceneAsync(whichPlayerHubToLoad());
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
}
