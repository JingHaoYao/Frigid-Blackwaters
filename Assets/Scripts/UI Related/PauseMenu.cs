using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    GameObject mainPauseMenu;
    public GameObject inventoryDisplay;
    public GameObject statsDisplay;
    public GameObject artifactsDisplay;
    public GameObject blackWindow;
    public GameObject soundOptions;
    public int whichSceneLoad = 0;
    public GameObject quitConfirmationButton, quitConfirmationButtonInCombat, quitToTitleSceneConfirmation;
    bool clickedSceneTransition = false;
    ShipStats stats;
    bool alreadyAppliedLossesQuit = false;
    public GameObject controlsMenu;
    public GameObject hubMap;
    public GameObject dungeonMap;
    public GameObject visualOptions;

    [SerializeField] Button artifragmentButton;
    [SerializeField] Button articraftButton;

    private void Start()
    {
        stats = PlayerProperties.playerShip.GetComponent<ShipStats>();
        mainPauseMenu = transform.GetChild(0).gameObject;

        PlayerProperties.pauseMenu = this;

        if(!PlayerProperties.playerScript.IsInPlayerHub() | !MiscData.unlockedArticrafting)
        {
            artifragmentButton.interactable = false;
            articraftButton.interactable = false;
            artifragmentButton.GetComponentInChildren<Text>().color = new Color(0, 1, 0.9893312f, 0.5f);
            articraftButton.GetComponentInChildren<Text>().color = new Color(0, 1, 0.9893312f, 0.5f);
        }
    }

    public void UnlockArtifragmentMenus()
    {
        if (MiscData.unlockedArticrafting)
        {
            artifragmentButton.interactable = true;
            articraftButton.interactable = true;
            artifragmentButton.GetComponentInChildren<Text>().color = new Color(0, 1, 0.9893312f, 1f);
            articraftButton.GetComponentInChildren<Text>().color = new Color(0, 1, 0.9893312f, 1f);
        }
    }

    private void Update()
    {
        if (mainPauseMenu.activeSelf == false)
        {
            if (
                PlayerProperties.playerScript.windowAlreadyOpen == false 
                && quitConfirmationButton.activeSelf == false 
                && quitConfirmationButtonInCombat.activeSelf == false
                && quitToTitleSceneConfirmation.activeSelf == false
                )
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PlayerProperties.playerScript.windowAlreadyOpen = true;
                    mainPauseMenu.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
        else
        {
            Time.timeScale = 0;
            PlayerProperties.playerScript.windowAlreadyOpen = true;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PlayerProperties.playerScript.windowAlreadyOpen = false;
                mainPauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void OpenInventoryAndArticrafting()
    {
        mainPauseMenu.SetActive(false);
        PlayerProperties.playerInventory.OpenCraftingAndInventory();
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void OpenInventoryAndArtifragmenting()
    {
        mainPauseMenu.SetActive(false);
        PlayerProperties.playerInventory.OpenDisenchantingAndInventory();
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void openDungeonMap()
    {
        dungeonMap.transform.localScale = new Vector3(1, 1, 1);
        PlayerProperties.playerShip.GetComponent<MapUI>().PlayOpenAnimation();
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
        mainPauseMenu.SetActive(false);
    }

    public void openSoundOptions()
    {
        mainPauseMenu.SetActive(false);
        soundOptions.SetActive(true);
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void openVisualOptions()
    {
        mainPauseMenu.SetActive(false);
        visualOptions.SetActive(true);
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void openInventory()
    {
        mainPauseMenu.SetActive(false);
        PlayerProperties.playerInventory.OpenArtifactsAndInventory();
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void openStats()
    {
        mainPauseMenu.SetActive(false);
        statsDisplay.SetActive(true);
        stats.PlayOpeningAnimation();
        stats.UpdateUI();
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void openPlayerMap()
    {
        hubMap.transform.GetChild(0).gameObject.SetActive(true);
        hubMap.GetComponent<PlayerHubMap>().UpdateUI();
        hubMap.GetComponent<PlayerHubMap>().PlayOpeningAnimation();
        mainPauseMenu.SetActive(false);
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    public void quitGame()
    {
        if (PlayerProperties.playerScript.enemiesDefeated == true)
        {
            MiscData.playerDied = true;
        }
        else
        {
            PlayerProperties.playerScript.playerDead = true;
            PlayerProperties.playerScript.applyInventoryLoss();
        }
        alreadyAppliedLossesQuit = true;
        SaveSystem.SaveGame();
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
        Application.Quit();
    }

    public void endExpedition()
    {
        if(PlayerProperties.playerScript.enemiesDefeated == true)
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
        if(PlayerProperties.playerScript.enemiesDefeated == true)
        {
            choosePlayerHubToLoad();
            SaveSystem.SaveGame();
        }
        else
        {
            MiscData.playerDied = true;
            PlayerProperties.playerScript.playerDead = true;
            PlayerProperties.playerScript.applyInventoryLoss();
            choosePlayerHubToLoad();
            SaveSystem.SaveGame();
        }
        FindObjectOfType<AudioManager>().PlaySound("Pause Menu Button");
    }

    void choosePlayerHubToLoad()
    {
        switch(MiscData.dungeonLevelUnlocked)
        {
            case 1:
                StartCoroutine(fadeLoadScene(1));
                break;
            case 2:
                StartCoroutine(fadeLoadScene(7));
                break;
            case 3:
                StartCoroutine(fadeLoadScene(5));
                break;
            case 4:
                StartCoroutine(fadeLoadScene(8));
                break;
            case 5:
                StartCoroutine(fadeLoadScene(11));
                break;
        }
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

    public void loadHub()
    {
        choosePlayerHubToLoad();
        SaveSystem.SaveGame();
    }
    
    public void quitToTitleScreen()
    {
        if (PlayerProperties.playerScript.enemiesDefeated == true)
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
        if (PlayerProperties.playerScript.enemiesDefeated == true)
        {
            StartCoroutine(fadeLoadScene(0));
            SaveSystem.SaveGame();
        }
        else
        {
            PlayerProperties.playerScript.playerDead = true;
            MiscData.playerDied = true;
            PlayerProperties.playerScript.applyInventoryLoss();
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
            if (PlayerProperties.playerScript.enemiesDefeated == false)
            {
                PlayerProperties.playerScript.playerDead = true;
                PlayerProperties.playerScript.applyInventoryLoss();
            }
        }
        SaveSystem.SaveGame();
    }
}
