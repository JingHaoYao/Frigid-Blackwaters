using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSmithMenus : MonoBehaviour {
    public GameObject[] menusList;
    int currMenu = 0;
    public Text skillPointsText, storeGold, skillPointsPrice;
    public FireworkUpgradeTilesUI fireworkUpgradesMenu;
    public DragonsBreathUpgradeTilesUI dragonsBreathUpgradesMenu;
    public MusketUpgradeTilesUI musketUpgradesMenu;
    public SpreadshotUpgradeTilesUI spreadShotUpgradesMenu;
    public CannonUpgradeTilesUI cannonUpgradesMenu;
    public HealthUpgradeTilesUI hullUpgradesMenu;
    public GameObject weaponSelectorMenu, returnButton;
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

        if(PlayerUpgrades.musketUnlocked)
        {
            if (!PlayerUpgrades.musketUpgrades.Contains("musket_weapon_unlocked"))
            {
                PlayerUpgrades.musketUpgrades.Add("musket_weapon_unlocked");
            }
        }
        else
        {
            menusList[0].GetComponentInChildren<MusketUpgradeTilesUI>().musketUpgradeTiles[0].noLongerUnlockable = true;
        }

        if(PlayerUpgrades.cannonUnlocked)
        {
            if(!PlayerUpgrades.cannonUpgrades.Contains("cannon_unlocked"))
                PlayerUpgrades.cannonUpgrades.Add("cannon_unlocked");
        }
        else
        {
            menusList[1].GetComponentInChildren<CannonUpgradeTilesUI>().CannonUpgradeTiles[0].noLongerUnlockable = true;
        }

        if (PlayerUpgrades.spreadShotUnlocked)
        {
            if (!PlayerUpgrades.spreadshotUpgrades.Contains("spreadshot_weapon_unlocked"))
                PlayerUpgrades.spreadshotUpgrades.Add("spreadshot_weapon_unlocked");
        }
        else
        {
            menusList[2].GetComponentInChildren<SpreadshotUpgradeTilesUI>().SpreadshotUpgradeTiles[0].noLongerUnlockable = true;
        }

        if (PlayerUpgrades.fireworkUnlocked)
        {
            if (!PlayerUpgrades.fireworkUpgrades.Contains("firework_weapon_unlocked"))
                PlayerUpgrades.fireworkUpgrades.Add("firework_weapon_unlocked");
        }
        else
        {
            menusList[3].GetComponentInChildren<FireworkUpgradeTilesUI>().FireworkUpgradeTiles[0].noLongerUnlockable = true;
        }

        if (PlayerUpgrades.dragonsBreathUnlocked)
        {
            if (!PlayerUpgrades.dragonBreathUpgrades.Contains("dragon_breath_weapon_unlocked"))
                PlayerUpgrades.dragonBreathUpgrades.Add("dragon_breath_weapon_unlocked");
        }
        else
        {
            menusList[4].GetComponentInChildren<DragonsBreathUpgradeTilesUI>().DragonsBreathUpgradeTiles[0].noLongerUnlockable = true;
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

    /*public void nextMenu()
    {
        menusList[currMenu].SetActive(false);
        currMenu++;
        if(currMenu >= menusList.Length)
        {
            currMenu = 0;
        }
        menusList[currMenu].SetActive(true);
        FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
    }

    public void prevMenu()
    {
        menusList[currMenu].SetActive(false);
        currMenu--;
        if(currMenu < 0)
        {
            currMenu = menusList.Length - 1;
        }
        menusList[currMenu].SetActive(true);
        FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
    }*/

    public void resetUpgrades()
    {
        FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
        PlayerUpgrades.musketUpgrades.Clear();
        PlayerUpgrades.cannonUpgrades.Clear();
        PlayerUpgrades.spreadshotUpgrades.Clear();
        PlayerUpgrades.fireworkUpgrades.Clear();
        PlayerUpgrades.dragonBreathUpgrades.Clear();
        PlayerUpgrades.hullUpgrades.Clear();
        PlayerUpgrades.safeUpgrades.Clear();
        PlayerUpgrades.inventoryUpgrades.Clear();
        checkWeaponsUnlocked();

        if(weaponSelectorMenu.activeSelf == false)
        {
            menusList[currMenu].SetActive(false);
            menusList[currMenu].SetActive(true);
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
    }
}
