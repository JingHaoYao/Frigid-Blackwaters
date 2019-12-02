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
            // This line only for the demo - just to clear any artifacts that the player has left over
            PlayerItems.inventoryItemsIDs.Clear();

            StartCoroutine(fadeLoadScene());
            //loading save upon player clicking play
            SaveData data = SaveSystem.GetSave();
            SaveSystem.loadData(data);

            FindObjectOfType<AudioManager>().PlaySound("Play Button");

            clicked = true;
        }
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
            AsyncOperation openScene = SceneManager.LoadSceneAsync(1);
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
    }
}
