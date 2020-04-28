using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HubLoader : MonoBehaviour
{
    GameObject playerShip;
    PlayerScript playerScript;
    bool clickedSceneTransition = false;
    public GameObject blackWindow;

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        playerScript = playerShip.GetComponent<PlayerScript>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "PlayerShip")
        {
            playerScript.playerDead = true;
            choosePlayerHubToLoad();
        }
    }

    IEnumerator fadeLoadScene(int whichScene)
    {
        if (clickedSceneTransition == false)
        {
            clickedSceneTransition = true;
            blackWindow.SetActive(true);
            blackWindow.GetComponent<Animator>().GetComponent<Animator>().SetTrigger("FadeOut");
            FindObjectOfType<AudioManager>().FadeOut("Tutorial Background Music", 0.2f);
            FindObjectOfType<AudioManager>().FadeOut("Storm Background Noise", 0.2f);
            yield return new WaitForSeconds(1f);
            AsyncOperation openScene = SceneManager.LoadSceneAsync(1);
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

    void choosePlayerHubToLoad()
    {
        if (MiscData.dungeonLevelUnlocked == 3)
        {
            StartCoroutine(fadeLoadScene(5));
        }
        else
        {
            StartCoroutine(fadeLoadScene(1));
        }
    }


}
