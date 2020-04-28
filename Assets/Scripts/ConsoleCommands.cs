
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opencoding.CommandHandlerSystem;
using UnityEngine.SceneManagement;

public class ConsoleCommands : MonoBehaviour
{
    private void Awake()
    {
        CommandHandlers.RegisterCommandHandlers(this);
    }

    [CommandHandler(Name = "SkipTutorial", Description = "Skip the tutorial from the title screen and go straight to player hub")]
    private void SkipTutorial()
    {
        MiscData.finishedTutorial = true;
        SceneManager.LoadScene("Player Hub");
    }

    [CommandHandler(Name = "EquipWeapon", Description = "Equip a weapon on a specific side of your ship")]
    private void LoadWeapon(WeaponChoices weapon, WhichWeaponSide side)
    {
        Debug.Log(string.Format("Equipping weapon {0} on the {1} of the ship", weapon.ToString(), side.ToString()));
        string[] choices = Enum.GetNames(typeof(WeaponChoices));
        List<string> listChoices = new List<string>();
        foreach(string choice in choices)
        {
            listChoices.Add(choice);
        }

        if (side == WhichWeaponSide.Left)
        {
            PlayerUpgrades.whichLeftWeaponEquipped = listChoices.IndexOf(weapon.ToString());
        }
        else if(side == WhichWeaponSide.Center)
        {
            PlayerUpgrades.whichFrontWeaponEquipped = listChoices.IndexOf(weapon.ToString());
        }
        else
        {
            PlayerUpgrades.whichRightWeaponEquipped = listChoices.IndexOf(weapon.ToString());
        }

        if (PlayerProperties.playerScript != null)
        {
            FindObjectOfType<LoadWeaponChoices>().loadWeapon();
        }
    }

    [CommandHandler(Name = "UpgradeWeapon", Description = "Upgrade a weapon")]
    private void UpgradeWeapon(WeaponChoices weapon, int whatTier, bool isLeftUpgradeTree = true)
    {
        Debug.Log("Upgrading weapon " + weapon.ToString());
        switch (weapon)
        {
            case WeaponChoices.Musket:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "precision_shot_unlocked" : "", PlayerUpgrades.musketUpgrades);
                break;
            case WeaponChoices.Cannon:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "momentum_blast_unlocked" : "", PlayerUpgrades.cannonUpgrades);
                break;
            case WeaponChoices.Spreadshot:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "spreadshot_five_spray_unlocked" : "", PlayerUpgrades.spreadshotUpgrades);
                break;
            case WeaponChoices.Firework:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "large_explosions_unlocked" : "", PlayerUpgrades.fireworkUpgrades);
                break;
            case WeaponChoices.DragonsBreath:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "blue_flames_unlocked" : "", PlayerUpgrades.dragonBreathUpgrades);
                break;
            case WeaponChoices.Sniper:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "unlock_high_velocity_bullets" : "", PlayerUpgrades.sniperUpgrades);
                break;
            case WeaponChoices.ChemicalSprayer:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "unlock_explosive_blast" : "", PlayerUpgrades.chemicalSprayerUpgrades);
                break;
            case WeaponChoices.GlaiveLauncher:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "unlock_bouncing_glaives" : "", PlayerUpgrades.glaiveLauncherUpgrades);
                break;
            case WeaponChoices.PlantMortar:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "unlock_aila_upgrade" : "", PlayerUpgrades.plantMortarUpgrades);
                break;
            case WeaponChoices.PodFlyers:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "unlock_spiky_pods" : "", PlayerUpgrades.podFlyersUpgrades);
                break;
            case WeaponChoices.PolluxShrine:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "unlock_waves_light_balls" : "", PlayerUpgrades.polluxShrineUpgrades);
                break;

        }
    }

    [CommandHandler(Name = "DebugReloadScene", Description = "Reload the current scene")]
    private void DebugReloadScene()
    {
        Debug.Log("Reloading Scene!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [CommandHandler(Name = "UnlockLevel", Description = "Unlock a level to access from the player hub")]
    private void UnlockLevel(int whatLevel)
    {
        Debug.Log("Unlocking Level " + whatLevel.ToString());
        MiscData.dungeonLevelUnlocked = whatLevel;
    }

    [CommandHandler(Name = "ForceCompleteMission", Description = "Add a mission as completed in your player data")]
    private void ForceCompleteMission(AllBosses boss)
    {
        Debug.Log("Completing mission with boss " + boss.ToString());
        switch (boss)
        {
            case AllBosses.UndeadMariner:
                MiscData.completedMissions.Add("defeat_undead_mariner");
                break;
            case AllBosses.SeaTerror:
                MiscData.completedMissions.Add("defeat_sea_terror");
                break;
            case AllBosses.ElderFrostMage:
                MiscData.completedMissions.Add("defeat_elder_frost_mage");
                break;
            case AllBosses.Sentinel:
                MiscData.completedMissions.Add("defeat_sentinel");
                break;
            case AllBosses.TombGuardian:
                MiscData.completedMissions.Add("defeat_the_flail_golem");
                break;
            case AllBosses.CrustaceaKing:
                MiscData.completedMissions.Add("defeat_the_crustacea_king");
                break;
            case AllBosses.SpectralHelmsman:
                MiscData.completedMissions.Add("defeat_the_spectral_helmsman");
                break;
            case AllBosses.Ahalfar:
                MiscData.completedMissions.Add("defeat_ahalfar");
                break;
        }
    }

    [CommandHandler(Name = "AddPlayerGold", Description = "Give yourself some stored gold")]
    private void AddPlayerGold(int amountOfGold)
    {
        Debug.Log(string.Format("Adding {0} gold to the player", amountOfGold));
        HubProperties.storeGold += amountOfGold;
    }

    [CommandHandler(Name = "GivePlayerItem", Description = "Give yourself an item by name")]
    private void GivePlayerItem(string itemReference)
    {
        if(PlayerProperties.playerInventory != null)
        {
            ItemTemplates itemDB = FindObjectOfType<ItemTemplates>();
            if (itemDB != null)
            {
                if (itemDB.dbContainsID(itemReference) == false)
                {
                    Debug.Log(string.Format("Can't find item with reference {0}", itemReference));
                }
                else
                {
                    Debug.Log(string.Format("Giving player item {0}, try reloading your inventory by closing and opening it", itemReference));
                    GameObject newItem = Instantiate(itemDB.loadItem(itemReference));
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    PlayerProperties.playerInventory.itemList.Add(newItem);
                }
            }
        }
    }

    [CommandHandler(Name = "AddArtifactKills", Description = "Give yourself a number of artifact kills")]
    private void AddArtifactKills(int numberKillsToAdd)
    {
        Debug.Log(string.Format("Adding {0} artifact kills", numberKillsToAdd));
        PlayerProperties.playerArtifacts.numKills += numberKillsToAdd;
    }

    [CommandHandler(Name = "SpawnIdleDummyEnemy", Description = "Spawn an idle dummy enemy at your cursor position")]
    private void SpawnIdleDummyEnemy()
    {
        Instantiate(Resources.Load<GameObject>("Regular Enemies/Dummy Enemy"), Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    }

    [CommandHandler(Name = "AddGoldItem", Description = "Spawn new gold items in inventory")]
    private void SpawnGoldItems(int goldAmount, int numberGoldItems)
    {
        GameObject goldItem = FindObjectOfType<ItemTemplates>().gold;
        for(int i = 0; i < numberGoldItems; i++)
        {
            GameObject goldItemInstant = Instantiate(goldItem);
            goldItemInstant.GetComponent<DisplayItem>().goldValue = goldAmount;
            PlayerProperties.playerInventory.itemList.Add(goldItemInstant);
        }
    }

    [CommandHandler(Name = "UnlockAllBuildings", Description = "Unlock all buildings in the player hub")]
    private void UnlockAllBuildings()
    {
        MiscData.unlockedBuildings.Add("provisions");
        MiscData.unlockedBuildings.Add("artifact_shop");
        MiscData.unlockedBuildings.Add("golden_vault");
        MiscData.unlockedBuildings.Add("shipsmith");
        MiscData.unlockedBuildings.Add("weapon_outfitter");
    }

    private void AddUpgrades(int tier, string leftUpgradeTree, List<string> upgrades)
    {
        for(int i = 0; i < Mathf.Clamp(tier, 0, 6); i++)
        {  
            if(i == 3)
            {
                upgrades.Add(leftUpgradeTree);
            }
            else
            {
                upgrades.Add("Generic_Upgrade");
            }
        }
    }

    enum WeaponChoices
    {
        Musket,
        Cannon,
        Spreadshot,
        Firework,
        DragonsBreath,
        Sniper,
        ChemicalSprayer,
        GlaiveLauncher,
        PlantMortar,
        PodFlyers,
        PolluxShrine
    }

    enum WhichWeaponSide
    {
        Left,
        Right,
        Center
    }

    enum AllBosses
    {
        UndeadMariner,
        SeaTerror,
        ElderFrostMage,
        Sentinel,
        TombGuardian,
        CrustaceaKing,
        SpectralHelmsman,
        Ahalfar
    }


}
