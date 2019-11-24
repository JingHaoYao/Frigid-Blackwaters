using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    PlayerScript playerScript;
    GameObject mainPauseMenu;
    public GameObject inventoryDisplay;
    public GameObject statsDisplay;
    public GameObject artifactsDisplay;
    public GameObject blackWindow;
    public GameObject questDisplay;
    public GameObject soundOptions;
    public int whichSceneLoad = 0;
    public GameObject quitConfirmationButton, quitConfirmationButtonInCombat, quitToTitleSceneConfirmation;
    bool clickedSceneTransition = false;
    Inventory inventory;
    Artifacts artifacts;
    ShipStats stats;
    bool alreadyAppliedLossesQuit = false;
    public GameObject controlsMenu;
    public GameObject hubMap;
    public GameObject dungeonMap;

    private void Start()
    {
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        inventory = GameObject.Find("PlayerShip").GetComponent<Inventory>();
        stats = GameObject.Find("PlayerShip").GetComponent<ShipStats>();
        mainPauseMenu = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (mainPauseMenu.activeSelf == false)
        {
            if (
                playerScript.windowAlreadyOpen == false 
                && quitConfirmationButton.activeSelf == false 
                && quitConfirmationButtonInCombat.activeSelf == false
                && quitToTitleSceneConfirmation.activeSelf == false
                )
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    playerScript.windowAlreadyOpen = true;
                    mainPauseMenu.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                playerScript.windowAlreadyOpen = false;
                mainPauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void openDungeonMap()
    {
        dungeonMap.transform.localScale = new Vector3(1, 1, 1);
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
        mainPauseMenu.SetActive(false);
    }

    public void openSoundOptions()
    {
        mainPauseMenu.SetActive(false);
        soundOptions.SetActive(true);
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void openInventory()
    {
        mainPauseMenu.SetActive(false);
        inventoryDisplay.SetActive(true);
        inventory.UpdateUI();
        artifactsDisplay.SetActive(true);
        artifacts.UpdateUI();
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void openStats()
    {
        mainPauseMenu.SetActive(false);
        statsDisplay.SetActive(true);
        stats.UpdateUI();
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void openPlayerMap()
    {
        hubMap.transform.GetChild(0).gameObject.SetActive(true);
        hubMap.GetComponent<PlayerHubMap>().UpdateUI();
        mainPauseMenu.SetActive(false);
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void quitGame()
    {
        if (playerScript.enemiesDefeated == true)
        {
            MiscData.playerDied = true;
        }
        else
        {
            playerScript.playerDead = true;
            playerScript.applyInventoryLoss();
        }
        alreadyAppliedLossesQuit = true;
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
        SaveSystem.SaveGame();
        Application.Quit();
    }

    public void endExpedition()
    {
        if(playerScript.enemiesDefeated == true)
        {
            mainPauseMenu.SetActive(false);
            quitConfirmationButton.SetActive(true);
        }
        else
        {
            mainPauseMenu.SetActive(false);
            quitConfirmationButtonInCombat.SetActive(true);
        }
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void endExpeditionLoadHub()
    {
        Time.timeScale = 1;
        if(playerScript.enemiesDefeated == true)
        {
            StartCoroutine(fadeLoadScene(1));
            SaveSystem.SaveGame();
        }
        else
        {
            MiscData.playerDied = true;
            playerScript.playerDead = true;
            playerScript.applyInventoryLoss();
            StartCoroutine(fadeLoadScene(1));
            SaveSystem.SaveGame();
        }
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void goBack(GameObject areYouSureButton)
    {
        mainPauseMenu.SetActive(true);
        areYouSureButton.SetActive(false);
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    IEnumerator fadeLoadScene(int whichScene)
    {
        if (clickedSceneTransition == false)
        {
            clickedSceneTransition = true;
            blackWindow.SetActive(true);
            blackWindow.GetComponent<Animator>().GetComponent<Animator>().SetTrigger("FadeOut");
            yield return new WaitForSeconds(1f);
            AsyncOperation openScene = SceneManager.LoadSceneAsync(whichScene);
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

    public void loadHub()
    {
        StartCoroutine(fadeLoadScene(1));
        SaveSystem.SaveGame();
    }
    
    public void quitToTitleScreen()
    {
        if (playerScript.enemiesDefeated == true)
        {
            mainPauseMenu.SetActive(false);
            quitToTitleSceneConfirmation.SetActive(true);
        }
        else
        {
            mainPauseMenu.SetActive(false);
            quitToTitleSceneConfirmation.SetActive(true);
        }
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void loadTitleScene()
    {
        Time.timeScale = 1;
        if (playerScript.enemiesDefeated == true)
        {
            StartCoroutine(fadeLoadScene(0));
            SaveSystem.SaveGame();
        }
        else
        {
            playerScript.playerDead = true;
            MiscData.playerDied = true;
            playerScript.applyInventoryLoss();
            StartCoroutine(fadeLoadScene(0));
            SaveSystem.SaveGame();
        }
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void openControlsMenu()
    {
        mainPauseMenu.SetActive(false);
        controlsMenu.SetActive(true);
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    private void OnApplicationQuit()
    {
        if (alreadyAppliedLossesQuit == false)
        {
            if (playerScript.enemiesDefeated == false)
            {
                playerScript.playerDead = true;
                playerScript.applyInventoryLoss();
            }
        }
        SaveSystem.SaveGame();
    }
}
