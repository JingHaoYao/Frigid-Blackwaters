using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoBossManager : BossManager
{
    public GameObject doorSeal;
    public GameObject boss;
    bool roomInit = false;
    public GameObject blackWindow;

    void Update()
    {
        if (Vector2.Distance(transform.position, Camera.main.transform.position) < 0.2f && roomInit == false)
        {
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = false;
            Instantiate(doorSeal, Camera.main.transform.position +  new Vector3(10.4f, 0, 0) , Quaternion.Euler(0, 0,  180));
            Instantiate(doorSeal, Camera.main.transform.position + new Vector3(-10.4f, 0, 0), Quaternion.Euler(0, 0, 0));
            boss.SetActive(true);
            StartCoroutine(adjustPlayer());
            roomInit = true;
        }
    }

    IEnumerator adjustPlayer()
    {
        GameObject playerShip = GameObject.Find("PlayerShip");
        //moves the player forward by a bit to adjust for ice walls spawning
        if (playerShip.transform.position.y > transform.position.y + 5)
        {
            playerShip.transform.position += new Vector3(0, -2f, 0);
        }
        else if (playerShip.transform.position.x > transform.position.x + 5)
        {
            playerShip.transform.position += new Vector3(-2f, 0, 0);
        }
        else if (playerShip.transform.position.x < transform.position.x - 5)
        {
            playerShip.transform.position += new Vector3(2f, 0, 0);
        }
        else
        {
            playerShip.transform.position += new Vector3(0, 2f, 0);
        }
        playerShip.GetComponent<PlayerScript>().shipRooted = true;
        yield return new WaitForSeconds(0.2f);
        playerShip.GetComponent<PlayerScript>().shipRooted = false;
    }

    IEnumerator fadeLoadScene(string whichSceneLoad)
    {
        blackWindow.SetActive(true);
        blackWindow.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);

        AsyncOperation openScene = SceneManager.LoadSceneAsync(whichSceneLoad);
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

    public void loadTitleScreen()
    {
        StartCoroutine(fadeLoadScene("Title Screen"));
    }
}
