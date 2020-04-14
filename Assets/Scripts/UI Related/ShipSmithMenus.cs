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
    [SerializeField] private GameObject weaponSelectorMenu, returnButton;
    int skillPointPrice = 0;

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
        }
        else
        {
            plantMortarUpgradesMenu.PlantMortarUpgradeTiles[0].noLongerUnlockable = true;
            podFlyersUpgradesMenu.PodFlyersUpgradeTiles[0].noLongerUnlockable = true;
        }

        foreach (GameObject menu in menusList)
        {
            menu.SetActive(false);
        }
    }

    private void Update()
    {
        skillPointPrice = 1500 + PlayerUpgrades.numberMaxSkillPoints * 1250 + 500 * Mathf.FloorToInt(PlayerUpgrades.numberMaxSkillPoints / 2) * 500;
        skillPointsText.text = PlayerUpgrades.numberSkillPoints.ToString();
        storeGold.text = HubProperties.storeGold.ToString();
        skillPointsPrice.text = skillPointPrice.ToString();
    }

    private void Start()
    {
        checkWeaponsUnlocked();
        weaponSelectorMenu.SetActive(true);
        returnButton.SetActive(false);
    }

    private void OnEnable()
    {
        checkWeaponsUnlocked();
        weaponSelectorMenu.SetActive(true);
        returnButton.SetActive(false);
    }

    public void turnOnMenu(int whatMenu)
    {
        weaponSelectorMenu.SetActive(false);
        menusList[whatMenu].SetActive(true);
        returnButton.SetActive(true);
        currMenu = whatMenu;
        FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
    }

    public void returnToMenu()
    {
        menusList[currMenu].SetActive(false);
        weaponSelectorMenu.SetActive(true);
        returnButton.SetActive(false);
        FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
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
        checkWeaponsUnlocked();

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

        checkWeaponsUnlocked();

        if (weaponSelectorMenu.activeSelf == false)
        {
            menusList[currMenu].SetActive(false);
            menusList[currMenu].SetActive(true);
        }
    }
}
