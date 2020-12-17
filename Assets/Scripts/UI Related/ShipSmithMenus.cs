using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSmithMenus : MonoBehaviour {
    public GameObject[] menusList;
    int currMenu = 0;
    [SerializeField] private Text skillPointsText, storeGold, skillPointsPrice;
    [SerializeField] private FireworkUpgradeTilesUI fireworkUpgradesMenu;
    [SerializeField] private DragonsBreathUpgradeTilesUI dragonsBreathUpgradesMenu;
    [SerializeField] private MusketUpgradeTilesUI musketUpgradesMenu;
    [SerializeField] private SpreadshotUpgradeTilesUI spreadShotUpgradesMenu;
    [SerializeField] private CannonUpgradeTilesUI cannonUpgradesMenu;
    [SerializeField] private HealthUpgradeTilesUI hullUpgradesMenu;
    [SerializeField] private SniperUpgradeTilesUI sniperUpgradesMenu;
    [SerializeField] private ChemicalSprayerUpgradeTilesUI chemicalSprayerUpgradesMenu;
    [SerializeField] private GlaiveLauncherUpgradeTileUI glaiveLauncherUpgradesMenu;
    [SerializeField] private PlantMortarUpgradeTilesUI plantMortarUpgradesMenu;
    [SerializeField] private PodFlyersUpgradeTilesUI podFlyersUpgradesMenu;
    [SerializeField] private PolluxShrineUpgradeTilesUI polluxShrineUpgradesMenu;
    [SerializeField] private LoneSparkUpgradeTilesUI loneSparkUpgradeTilesMenu;
    [SerializeField] private GadgetShotUpgradeTilesUI gadgetShotUpgradeTilesMenu;
    [SerializeField] private FinBladeUpgradeTilesUI finBladeUpgradeTilesMenu;
    [SerializeField] private RevolvingCannonUpgradeTilesUI revolvingCannonUpgradeTilesMenu;
    [SerializeField] private SmeltingLaserUpgradeTilesUI smeltingLaserUpgradeTilesMenu;
    [SerializeField] private TremorMakerUpgradeTilesUI tremorMakerUpgradeTilesMenu;
    [SerializeField] private GameObject weaponSelectorMenu, returnButton;
    int skillPointPrice = 0;

    [SerializeField] List<TutorialEntry> tutorialEntries;

    public void ShowTutorial()
    {
        PlayerProperties.tutorialWidgetMenu.Initialize(tutorialEntries);
    }

    MenuSlideAnimation menuSlideAnimation = new MenuSlideAnimation();

    void SetAnimation()
    {
        menuSlideAnimation.SetOpenAnimation(new Vector3(-950, 0, 0), new Vector3(0, 0, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(0, 0, 0), new Vector3(950, 0, 0), 0.25f);
    }

    public void purchaseSkillPoint()
    {
        FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
        if (HubProperties.storeGold >= skillPointPrice)
        {
            HubProperties.storeGold -= skillPointPrice;
            PlayerUpgrades.numberMaxSkillPoints++;
            PlayerUpgrades.numberSkillPoints++;
        }
    }

    public void OpenMusketMenu()
    {
        turnOnMenu(0);
    }

    void checkWeaponsUnlocked()
    {
        foreach(GameObject menu in menusList)
        {
            menu.SetActive(true);
        }

        if(MiscData.dungeonLevelUnlocked >= 1)
        {
            if (!PlayerUpgrades.musketUpgrades.Contains("musket_weapon_unlocked"))
                PlayerUpgrades.musketUpgrades.Add("musket_weapon_unlocked");

            if (!PlayerUpgrades.cannonUpgrades.Contains("cannon_unlocked"))
                PlayerUpgrades.cannonUpgrades.Add("cannon_unlocked");

            if (!PlayerUpgrades.spreadshotUpgrades.Contains("spreadshot_weapon_unlocked"))
                PlayerUpgrades.spreadshotUpgrades.Add("spreadshot_weapon_unlocked");

            if (!PlayerUpgrades.fireworkUpgrades.Contains("firework_weapon_unlocked"))
                PlayerUpgrades.fireworkUpgrades.Add("firework_weapon_unlocked");

            if (!PlayerUpgrades.dragonBreathUpgrades.Contains("dragon_breath_weapon_unlocked"))
                PlayerUpgrades.dragonBreathUpgrades.Add("dragon_breath_weapon_unlocked");
        }

        if (MiscData.dungeonLevelUnlocked >= 2)
        {
            if (!PlayerUpgrades.sniperUpgrades.Contains("unlock_sniper"))
                PlayerUpgrades.sniperUpgrades.Add("unlock_sniper");

            if (!PlayerUpgrades.chemicalSprayerUpgrades.Contains("unlock_chemical_sprayer"))
                PlayerUpgrades.chemicalSprayerUpgrades.Add("unlock_chemical_sprayer");

            if (!PlayerUpgrades.glaiveLauncherUpgrades.Contains("unlock_glaive_launcher"))
            {
                PlayerUpgrades.glaiveLauncherUpgrades.Add("unlock_glaive_launcher");
            }
        }
        else
        {
            sniperUpgradesMenu.SniperUpgradeTiles[0].noLongerUnlockable = true;
            chemicalSprayerUpgradesMenu.ChemicalSprayerUpgradeTiles[0].noLongerUnlockable = true;
            glaiveLauncherUpgradesMenu.GlaiveLauncherUpgradeTiles[0].noLongerUnlockable = true;
        }

        if(MiscData.dungeonLevelUnlocked >= 3)
        {
            if (!PlayerUpgrades.plantMortarUpgrades.Contains("unlock_the_plant_mortar"))
                PlayerUpgrades.plantMortarUpgrades.Add("unlock_the_plant_mortar");
            if (!PlayerUpgrades.podFlyersUpgrades.Contains("unlock_the_pod_flyers"))
            {
                PlayerUpgrades.podFlyersUpgrades.Add("unlock_the_pod_flyers");
            }
            if (!PlayerUpgrades.polluxShrineUpgrades.Contains("unlock_the_pollux_shrine"))
            {
                PlayerUpgrades.polluxShrineUpgrades.Add("unlock_the_pollux_shrine");
            }
        }
        else
        {
            plantMortarUpgradesMenu.PlantMortarUpgradeTiles[0].noLongerUnlockable = true;
            podFlyersUpgradesMenu.PodFlyersUpgradeTiles[0].noLongerUnlockable = true;
            polluxShrineUpgradesMenu.PolluxShrineUpgradeTiles[0].noLongerUnlockable = true;
        }

        if (MiscData.dungeonLevelUnlocked >= 4)
        {
            if (!PlayerUpgrades.loneSparkUpgrades.Contains("unlock_the_lone_spark"))
            {
                PlayerUpgrades.loneSparkUpgrades.Add("unlock_the_lone_spark");
            }

            if (!PlayerUpgrades.gadgetShotUpgrades.Contains("unlock_the_gadget_shot"))
            {
                PlayerUpgrades.gadgetShotUpgrades.Add("unlock_the_gadget_shot");
            }

            if (!PlayerUpgrades.finBladeUpgrades.Contains("unlock_the_fin_blade"))
            {
                PlayerUpgrades.finBladeUpgrades.Add("unlock_the_fin_blade");
            }
        }
        else
        {
            loneSparkUpgradeTilesMenu.LoneSparkUpgradeTiles[0].noLongerUnlockable = true;
            gadgetShotUpgradeTilesMenu.GadgetShotUpgradeTiles[0].noLongerUnlockable = true;
            finBladeUpgradeTilesMenu.FinBladeUpgradeTiles[0].noLongerUnlockable = true;
        }

        if (MiscData.dungeonLevelUnlocked >= 5)
        {
            if (!PlayerUpgrades.revolvingCannonUpgrades.Contains("unlock_the_revolving_cannon"))
            {
                PlayerUpgrades.revolvingCannonUpgrades.Add("unlock_the_revolving_cannon");
            }

            if (!PlayerUpgrades.smeltingLaserUpgrades.Contains("unlock_the_smelting_laser"))
            {
                PlayerUpgrades.smeltingLaserUpgrades.Add("unlock_the_smelting_laser");
            }

            if (!PlayerUpgrades.tremorMakerUpgrades.Contains("unlock_the_tremor_maker"))
            {
                PlayerUpgrades.tremorMakerUpgrades.Add("unlock_the_tremor_maker");
            }
        }
        else
        {
            revolvingCannonUpgradeTilesMenu.RevolvingCannonUpgradeTiles[0].noLongerUnlockable = true;
            smeltingLaserUpgradeTilesMenu.SmeltingLaserUpgradeTiles[0].noLongerUnlockable = true;
            tremorMakerUpgradeTilesMenu.TremorMakerUpgradeTiles[0].noLongerUnlockable = true;
        }

        foreach (GameObject menu in menusList)
        {
            menu.SetActive(false);
        }
    }

    private void Update()
    {
        skillPointPrice = 1500 + PlayerUpgrades.numberMaxSkillPoints * (750 + 300 * Mathf.FloorToInt(PlayerUpgrades.numberMaxSkillPoints / 4));
        skillPointsText.text = PlayerUpgrades.numberSkillPoints.ToString();
        storeGold.text = pickDisplay(HubProperties.storeGold);
        skillPointsPrice.text = pickDisplay(skillPointPrice);
    }

    string pickDisplay(int goldAmount)
    {
        if (goldAmount < 1000)
        {
            return goldAmount.ToString();
        }
        else if (goldAmount < 100000)
        {
            string goldToDisplay = ((float)goldAmount / 1000).ToString();
            return goldToDisplay.Substring(0, Mathf.Clamp(goldToDisplay.Length, 0, 4)) + "K";
        }
        else if (goldAmount < 1000000)
        {
            string goldToDisplay = ((float)goldAmount / 1000).ToString();
            return goldToDisplay.Substring(0, Mathf.Clamp(goldToDisplay.Length, 0, 3)) + "K";
        }
        else if(goldAmount < 10000000)
        {
            string goldToDisplay = ((float)goldAmount / 1000000).ToString();
            return goldToDisplay.Substring(0, Mathf.Clamp(goldToDisplay.Length, 0, 3)) + "M";
        }
        else
        {
            string goldToDisplay = ((float)goldAmount / 1000000).ToString();
            return goldToDisplay.Substring(0, Mathf.Clamp(goldToDisplay.Length, 0, 4)) + "M";
        }
    }

    private void Start()
    {
        checkWeaponsUnlocked();
        weaponSelectorMenu.SetActive(true);
        returnButton.SetActive(false);
        SetAnimation();
    }

    private void OnEnable()
    {
        checkWeaponsUnlocked();
        weaponSelectorMenu.SetActive(true);
        weaponSelectorMenu.transform.localPosition = Vector3.zero;
        returnButton.SetActive(false);
    }

    public void turnOnMenu(int whatMenu)
    {
        if (menuSlideAnimation.IsAnimating == false)
        {
            menuSlideAnimation.PlayEndingAnimation(weaponSelectorMenu, () => { weaponSelectorMenu.SetActive(false); });
            menusList[whatMenu].SetActive(true);
            menuSlideAnimation.PlayOpeningAnimation(menusList[whatMenu]);
            returnButton.SetActive(true);
            currMenu = whatMenu;
            FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
        }
    }

    public void returnToMenu()
    {
        if (menuSlideAnimation.IsAnimating == false && weaponSelectorMenu.activeSelf == false)
        {
            menuSlideAnimation.PlayEndingAnimation(menusList[currMenu], () => { menusList[currMenu].SetActive(false); });
            weaponSelectorMenu.SetActive(true);
            menuSlideAnimation.PlayOpeningAnimation(weaponSelectorMenu);
            returnButton.SetActive(false);
            FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
        }
    }

    public void resetUpgrades()
    {
        FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
        PlayerUpgrades.musketUpgrades.Clear();
        PlayerUpgrades.cannonUpgrades.Clear();
        PlayerUpgrades.spreadshotUpgrades.Clear();
        PlayerUpgrades.fireworkUpgrades.Clear();
        PlayerUpgrades.dragonBreathUpgrades.Clear();
        PlayerUpgrades.sniperUpgrades.Clear();
        PlayerUpgrades.dragonBreathUpgrades.Clear();
        PlayerUpgrades.chemicalSprayerUpgrades.Clear();
        PlayerUpgrades.glaiveLauncherUpgrades.Clear();
        PlayerUpgrades.hullUpgrades.Clear();
        PlayerUpgrades.safeUpgrades.Clear();
        PlayerUpgrades.inventoryUpgrades.Clear();
        PlayerUpgrades.plantMortarUpgrades.Clear();
        PlayerUpgrades.podFlyersUpgrades.Clear();
        PlayerUpgrades.polluxShrineUpgrades.Clear();
        PlayerUpgrades.loneSparkUpgrades.Clear();
        PlayerUpgrades.gadgetShotUpgrades.Clear();
        PlayerUpgrades.finBladeUpgrades.Clear();
        PlayerUpgrades.revolvingCannonUpgrades.Clear();
        PlayerUpgrades.smeltingLaserUpgrades.Clear();
        PlayerUpgrades.tremorMakerUpgrades.Clear();
        checkWeaponsUnlocked();

        List<GameObject> objectsToRemove = new List<GameObject>();

        for(int i = 0; i < PlayerProperties.playerInventory.itemList.Count; i++)
        {
            if(i >= 10)
            {
                objectsToRemove.Add(PlayerProperties.playerInventory.itemList[i]);
            }
        }

        GoldenVault vault = FindObjectOfType<GoldenVault>();

        foreach(GameObject item in objectsToRemove)
        {
            PlayerProperties.playerInventory.itemList.Remove(item);
            PlayerProperties.playerInventory.UpdateUI();
            vault.vaultItems.Add(item);
            HubProperties.vaultItems.Add(item.name);
        }

        PlayerUpgrades.numberSkillPoints = PlayerUpgrades.numberMaxSkillPoints;
        SaveSystem.SaveGame();

        foreach (MusketUpgradeTile tile in musketUpgradesMenu.musketUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach (SpreadshotUpgradeTile tile in spreadShotUpgradesMenu.SpreadshotUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach (CannonUpgradeTile tile in cannonUpgradesMenu.CannonUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach (FireworkUpgradeTile tile in fireworkUpgradesMenu.FireworkUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach (DragonsBreathUpgradeTile tile in dragonsBreathUpgradesMenu.DragonsBreathUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach (HullUpgradeTile tile in hullUpgradesMenu.HullUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach(SniperUpgradeTile tile in sniperUpgradesMenu.SniperUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach(ChemicalSprayerUpgradeTile tile in chemicalSprayerUpgradesMenu.ChemicalSprayerUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach(GlaiveLauncherUpgradeTile tile in glaiveLauncherUpgradesMenu.GlaiveLauncherUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach(PlantMortarUpgradeTile tile in plantMortarUpgradesMenu.PlantMortarUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach(PodFlyersUpgradeTile tile in podFlyersUpgradesMenu.PodFlyersUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach(PolluxShrineUpgradeTile tile in polluxShrineUpgradesMenu.PolluxShrineUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach (LoneSparkUpgradeTile tile in loneSparkUpgradeTilesMenu.LoneSparkUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach(GadgetShotUpgradeTile tile in gadgetShotUpgradeTilesMenu.GadgetShotUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach(FinBladeUpgradeTile tile in finBladeUpgradeTilesMenu.FinBladeUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach (RevolvingCannonUpgradeTile tile in revolvingCannonUpgradeTilesMenu.RevolvingCannonUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }
        
        foreach(SmeltingLaserUpgradeTile tile in smeltingLaserUpgradeTilesMenu.SmeltingLaserUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        foreach(TremorMakerUpgradeTile tile in tremorMakerUpgradeTilesMenu.TremorMakerUpgradeTiles)
        {
            tile.noLongerUnlockable = false;
        }

        checkWeaponsUnlocked();

        if (weaponSelectorMenu.activeSelf == false)
        {
            menusList[currMenu].SetActive(false);
            menusList[currMenu].SetActive(true);
        }
    }
}
