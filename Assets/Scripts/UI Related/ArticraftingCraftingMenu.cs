using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArticraftingCraftingMenu : MonoBehaviour
{
    [SerializeField] GameObject tileArrangement;
    [SerializeField] Text levelText;
    [SerializeField] Button nextLevelButton;
    [SerializeField] Button previousLevelButton;
    [SerializeField] Text artifragmentsText;
    [SerializeField] ArticraftingConfirmationModal confirmationModal;
    [SerializeField] GameObject artifactTile;
    [SerializeField] AudioSource craftItem;
    AudioManager audioManager;

    List<ArticraftingTile> articraftTiles = new List<ArticraftingTile>();
    MenuSlideAnimation menuSlideAnimation;
    Coroutine textClimbRoutine;

    [SerializeField] List<TutorialEntry> tutorialEntries;

    int currentLevel = 1;
    private bool isOpen = false;

    [SerializeField] GameObject craftingMenu;

    public void ShowTutorial()
    {
        PlayerProperties.tutorialWidgetMenu.Initialize(tutorialEntries);
    }

    private void Awake()
    {
        SetArtifactsAnimation();
        PlayerProperties.articraftingCraftingMenu = this;
    }

    public ArticraftingConfirmationModal GetConfirmationModal()
    {
        return confirmationModal;
    }

    public void CraftedItemVisuals(int price)
    {
        if(textClimbRoutine != null)
        {
            StopCoroutine(textClimbRoutine);
        }
        textClimbRoutine = StartCoroutine(textClimb(price));
        craftItem.Play();
    }

    IEnumerator textClimb(int amountArtifragmentsSpent) {
        int numberFragmentsLeft = 0;

        while (numberFragmentsLeft < amountArtifragmentsSpent)
        {
            if (amountArtifragmentsSpent - numberFragmentsLeft > 10)
            {
                numberFragmentsLeft += 10;
            }
            else
            {
                numberFragmentsLeft += 1;
            }
            artifragmentsText.text = ((PlayerUpgrades.numberArtifragments + amountArtifragmentsSpent) - numberFragmentsLeft).ToString();
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    void SetArtifactsAnimation()
    {
        menuSlideAnimation = new MenuSlideAnimation();
        menuSlideAnimation.SetOpenAnimation(new Vector3(0, -585, 0), new Vector3(0, 0, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(0, 0, 0), new Vector3(0, -585, 0), 0.25f);
    }

    void UpdatePage(int level)
    {
        levelText.text = DungeonName(level);
        if(PlayerItems.pastArtifacts.ContainsKey(level))
        {
            if(articraftTiles.Count < PlayerItems.pastArtifacts[level].Count)
            {
                for(int i = 0; i < PlayerItems.pastArtifacts[level].Count - articraftTiles.Count; i++)
                {
                    ArticraftingTile tile = Instantiate(artifactTile, tileArrangement.transform).GetComponent<ArticraftingTile>();
                    articraftTiles.Add(tile);
                }
            }

            for(int i = 0; i < articraftTiles.Count; i++)
            {
                if(i >= PlayerItems.pastArtifacts[level].Count)
                {
                    articraftTiles[i].Disable();
                }
                else
                {
                    GameObject artifactToLoad = PlayerProperties.itemTemplates.loadItem(PlayerItems.pastArtifacts[level][i]);
                    articraftTiles[i].Enable(artifactToLoad, confirmationModal, this);
                }
            }
        }
        else
        {
            foreach(ArticraftingTile tile in articraftTiles)
            {
                tile.Disable();
            }
        }
    }

    public void OpenCraftingMenu()
    {
        UpdateFragmentsText();
        PlayerProperties.playerScript.windowAlreadyOpen = true;
        UpdatePage(currentLevel);

        if (currentLevel == MiscData.dungeonLevelUnlocked)
        {
            nextLevelButton.interactable = false;
        }
        else
        {
            nextLevelButton.interactable = true;
        }

        if (currentLevel == 1)
        {
            previousLevelButton.interactable = false;
        }
        else
        {
            previousLevelButton.interactable = true;
        }

        PlayEnteringAnimation();
        Time.timeScale = 0;
    }

    public void PlayEnteringAnimation()
    {
        isOpen = true;
        craftingMenu.SetActive(true);
        if (MiscData.firstTimeTutorialsPlayed.Contains("articrafting_menu"))
        {
            menuSlideAnimation.PlayOpeningAnimation(craftingMenu);
        }
        else
        {
            MiscData.firstTimeTutorialsPlayed.Add("articrafting_menu");
            menuSlideAnimation.PlayOpeningAnimation(craftingMenu, ShowTutorial);
        }
    }

    public void CloseCraftingMenu()
    {
        PlayerProperties.playerScript.windowAlreadyOpen = false;
        confirmationModal.Disable();
        Time.timeScale = 1;
        CloseWindow();

        PlayerProperties.tutorialWidgetMenu.CloseTutorial();
    }

    public void UpdateFragmentsText()
    {
        artifragmentsText.text = PlayerUpgrades.numberArtifragments.ToString();
    }

    void CloseWindow()
    {
        isOpen = false;
        menuSlideAnimation.PlayEndingAnimation(craftingMenu, () => { craftingMenu.SetActive(false); });
    }

    public bool IsMenuOpened()
    {
        return isOpen;
    }

    string DungeonName(int level)
    {
        switch(level)
        {
            case 1:
                return "Surface Level";
            case 2:
                return "Ice Level";
            case 3:
                return "Flora Level";
            case 4:
                return "Illusion Level";
            case 5:
                return "Inferno Level";
            case 6:
                return "Realm of Giants";
            case 7:
                return "Path of Memories";
            default:
                return "Default Level";
        }
    }

    public void ProgressToNextPage()
    {
        if(currentLevel < MiscData.dungeonLevelUnlocked)
        {
            currentLevel++;
            UpdatePage(currentLevel);
            if (currentLevel == MiscData.dungeonLevelUnlocked)
            {
                nextLevelButton.interactable = false;
            }
            else
            {
                nextLevelButton.interactable = true;
            }

            if (currentLevel == 1)
            {
                previousLevelButton.interactable = false;
            }
            else
            {
                previousLevelButton.interactable = true;
            }
        }
    }

    public void ProgressToPreviousPage()
    {
        if (currentLevel > 1)
        {
            currentLevel--;
            UpdatePage(currentLevel);
            if (currentLevel == MiscData.dungeonLevelUnlocked)
            {
                nextLevelButton.interactable = false;
            }
            else
            {
                nextLevelButton.interactable = true;
            }

            if (currentLevel == 1)
            {
                previousLevelButton.interactable = false;
            }
            else
            {
                previousLevelButton.interactable = true;
            }
        }
    }
}
