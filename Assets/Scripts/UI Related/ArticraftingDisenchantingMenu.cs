using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ArticraftingDisenchantingMenu : MonoBehaviour
{
    [SerializeField] Button articraftingButton;
    [SerializeField] Text articraftingButtonText;
    [SerializeField] Text artifragmentsText;
    [SerializeField] Image artifactIcon;
    [SerializeField] Text fragmentsReceivedText;

    DisplayItem targetDisplayItem;

    MenuSlideAnimation menuSlideAnimation;
    [SerializeField] GameObject disenchantingMenu;
    [SerializeField] AudioSource disenchantAudio;
    [SerializeField] Sprite[] figureSpriteAnimation;
    [SerializeField] Image figureImage;
    bool isOpen = false;
    Color variedColor;
    bool fragmentsTextActive = false;

    [SerializeField] List<TutorialEntry> disenchantingMenuTutorialEntries;

    private void Awake()
    {
        PlayerProperties.articraftingDisenchantingMenu = this;
        SetArtifactsAnimation();
        VaryColor();
    }

    IEnumerator playFigureAnimation()
    {
        foreach(Sprite sprite in figureSpriteAnimation)
        {
            figureImage.sprite = sprite;
            yield return new WaitForSecondsRealtime(1 / 12f);
        }
    }

    void VaryColor()
    {
        LeanTween.value(0.4f, 1f, 0.5f).setEaseInOutQuad().setLoopPingPong().setOnUpdate((float val) => { variedColor = new Color(0, 1, 0.9717827f, val); }).setIgnoreTimeScale(true);
    }

    void SetArtifactsAnimation()
    {
        menuSlideAnimation = new MenuSlideAnimation();
        menuSlideAnimation.SetOpenAnimation(new Vector3(0, -585, 0), new Vector3(0, 0, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(0, 0, 0), new Vector3(0, -585, 0), 0.25f);
    }

    public void PlayEnteringAnimation()
    {
        isOpen = true;
        disenchantingMenu.SetActive(true);
        if (MiscData.firstTimeTutorialsPlayed.Contains("artifragmenting_menu"))
        {
            menuSlideAnimation.PlayOpeningAnimation(disenchantingMenu);
        }
        else
        {
            MiscData.firstTimeTutorialsPlayed.Add("artifragmenting_menu");
            menuSlideAnimation.PlayOpeningAnimation(disenchantingMenu, ShowTutorial);
        }
        targetDisplayItem = null;
        UpdateUI();
    }

    public void ShowTutorial()
    {
        PlayerProperties.tutorialWidgetMenu.Initialize(disenchantingMenuTutorialEntries);
    }

    private void Update()
    {
        if(fragmentsTextActive == true)
        {
            fragmentsReceivedText.color = variedColor;
            artifragmentsText.color = variedColor;
        }
        else
        {
            artifragmentsText.color = new Color(0, 1, 0.9717827f, 1f);
        }
    }

    void UpdateUI()
    {
        artifragmentsText.text = PlayerUpgrades.numberArtifragments.ToString();
        if(targetDisplayItem == null)
        {
            artifactIcon.enabled = false;
            fragmentsReceivedText.color = new Color(0, 1, 0.9717827f, 0);
            fragmentsTextActive = false;
        }
        else
        {
            artifactIcon.enabled = true;
            artifactIcon.sprite = targetDisplayItem.displayIcon;
            ArtifactBonus artifactBonus = targetDisplayItem.GetComponent<ArtifactBonus>();
            fragmentsReceivedText.text = determinePrice(artifactBonus.whatRarity, artifactBonus.whatDungeonArtifact).ToString() + " Fragments";
            fragmentsTextActive = true;
            fragmentsReceivedText.color = variedColor;
        }
    }

    IEnumerator textClimb(int numberArtifragmentsToAdd)
    {
        int numberLeftToAdd = 0;
        while(numberLeftToAdd < numberArtifragmentsToAdd)
        {
            if(numberArtifragmentsToAdd - numberLeftToAdd > 10)
            {
                numberLeftToAdd += 10;
            }
            else
            {
                numberLeftToAdd += 1;
            }
            artifragmentsText.text = ((PlayerUpgrades.numberArtifragments - numberArtifragmentsToAdd) + numberLeftToAdd).ToString();
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    int determinePrice(int rarity, int dungeonLevel)
    {
        int basePrice = 0;
        switch(rarity)
        {
            case 0:
                basePrice = 5;
                break;
            case 1:
                basePrice = 11;
                break;
            case 2:
                basePrice = 20;
                break;
            case 3:
                basePrice = 40;
                break;
        }
        return basePrice * dungeonLevel;
    }

    public void OpenDisenchantingMenu()
    {
        PlayerProperties.playerScript.windowAlreadyOpen = true;
        UpdateUI();
        PlayEnteringAnimation();
        Time.timeScale = 0;
    }

    public void CloseDisenchantingMenu()
    {
        PlayerProperties.playerScript.windowAlreadyOpen = false;
        Time.timeScale = 1;
        CloseWindow();
    }

    void CloseWindow()
    {
        isOpen = false;
        menuSlideAnimation.PlayEndingAnimation(disenchantingMenu, () => { disenchantingMenu.SetActive(false); ReturnItemIfPresent(); });
        PlayerProperties.tutorialWidgetMenu.CloseTutorial();
    }

    public void ReturnItemIfPresent()
    {
        if(targetDisplayItem != null)
        {
            PlayerProperties.playerInventory.itemList.Add(targetDisplayItem.gameObject);
            PlayerProperties.playerInventory.UpdateUI();
        }
        targetDisplayItem = null;
        UpdateUI();
    }

    public bool IsMenuOpened()
    {
        return isOpen;
    }

    public void SetTargetArtifact(DisplayItem displayItem)
    {
        ReturnItemIfPresent();
        this.targetDisplayItem = displayItem;
        UpdateUI();
    }

    public void DisenchantArtifact()
    {
        if (targetDisplayItem != null)
        {
            disenchantAudio.Play();
            StartCoroutine(playFigureAnimation());
            ArtifactBonus artifactBonus = targetDisplayItem.GetComponent<ArtifactBonus>();
            int price = determinePrice(artifactBonus.whatRarity, artifactBonus.whatDungeonArtifact);
            PlayerUpgrades.numberArtifragments += price;
            Destroy(targetDisplayItem);
            targetDisplayItem = null;
            UpdateUI();
            StartCoroutine(textClimb(price));

            SaveSystem.SaveGame();
        }
    }
    // some cases:
    // closing the menu while an artifact is in the slot - should return the artifact to the inventory
}
