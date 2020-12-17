
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opencoding.CommandHandlerSystem;
using UnityEngine.SceneManagement;

public class ConsoleCommands : MonoBehaviour
{
    [SerializeField] public DialogueUI dialogueUI;
    private void Awake()
    {
        CommandHandlers.RegisterCommandHandlers(this);
    }

    [CommandHandler(Name = "AddFlammableStack", Description = "Adds a flammable stack to the player")]
    private void AddFlammableStack()
    {
        FindObjectOfType<FlammableController>().AddFlammableStack(null);
    }

    [CommandHandler(Name = "IgniteFlammableStacks", Description = "Ignites flammable stacks and deals damage to player")]
    private void IgniteFlammableStacks()
    {
        FindObjectOfType<FlammableController>().IgniteFlammableStacks(null);
    }

    [CommandHandler(Name = "SkipTutorial", Description = "Skip the tutorial from the title screen and go straight to player hub")]
    private void SkipTutorial()
    {
        MiscData.finishedTutorial = true;
        SceneManager.LoadScene("Player Hub");
    }

    [CommandHandler(Name = "AddArtifragments", Description = "Add a given number of artifragments")]
    private void AddArtifragments(int artifragments)
    {
        PlayerUpgrades.numberArtifragments += artifragments;
    }

    [CommandHandler(Name = "UnlockArticrafting", Description = "Unlock the articrafting and artifragment menus")]
    private void UnlockArticrafting()
    {
        MiscData.unlockedArticrafting = true;
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
            case WeaponChoices.LoneSpark:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "static_field_upgrade" : "", PlayerUpgrades.loneSparkUpgrades);
                break;
            case WeaponChoices.GadgetShot:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "bounce_explosions_upgrade" : "", PlayerUpgrades.gadgetShotUpgrades);
                break;
            case WeaponChoices.FinBlade:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "soul_reaver_upgrade" : "", PlayerUpgrades.finBladeUpgrades);
                break;
            case WeaponChoices.RevolvingCannon:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "bullet_cartridge_upgrade" : "", PlayerUpgrades.revolvingCannonUpgrades);
                break;
            case WeaponChoices.SmeltingLaser:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "focusing_laser_upgrade" : "", PlayerUpgrades.smeltingLaserUpgrades);
                break;
            case WeaponChoices.TremorMaker:
                AddUpgrades(whatTier, isLeftUpgradeTree ? "burn_radius_upgrade" : "", PlayerUpgrades.tremorMakerUpgrades);
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

    [CommandHandler(Name = "GivePlayerDebugItem", Description = "Give a player a super powerful artifact")]
    private void GivePlayerDebugItem()
    {
        if (PlayerProperties.playerInventory != null)
        {
            GameObject newItem = Instantiate(Resources.Load<GameObject>("Items/Debug Item"));
            newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
            PlayerProperties.playerInventory.itemList.Add(newItem);
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

    [CommandHandler(Name = "StartDragonArmorBossFight", Description = "Start the dragon armor boss fight if you're on the fifth level")]
    private void StartDragonArmorBossFight()
    {
        SkyClimberBossManager skyClimberBossManager = FindObjectOfType<SkyClimberBossManager>();
        if (skyClimberBossManager != null)
        {
            skyClimberBossManager.startMovingPlayer();
        }
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

    [CommandHandler(Name = "ActivateFogEffect", Description = "Activate fog invisibility effect for the fourth level")]
    private void ActivateFogEffect(float duration)
    {
        FindObjectOfType<FourthLevelFogController>().ActivateFog();
        FindObjectOfType<FogCycleRoom>().applyInvisStatusEffects(duration);
        StartCoroutine(WaitForFogDuration(duration));
    }

    [CommandHandler(Name = "AddAllEnemiesToPool", Description = "Add all existing enemies in the scene to EnemyPool")]
    private void AddAllEnemiesToPool()
    {
        foreach(Enemy enemy in FindObjectsOfType<Enemy>())
        {
            EnemyPool.addEnemy(enemy);
        }
    }

    [CommandHandler(Name = "GrantHealthBonus", Description = "Add a temporary health bonus")]
    private void GrantHealthBonus(int healthBonus)
    {
        PlayerProperties.playerScript.healthBonus = healthBonus;
        PlayerProperties.playerScript.shipHealth = PlayerProperties.playerScript.shipHealthMAX + healthBonus;
    }

    [CommandHandler(Name = "TeleportPlayerAndCamera", Description = "Teleports the player to a specific location")]
    private void TelportPlayerAndCamera(int xPosition, int yPosition)
    {
        PlayerProperties.playerShip.transform.position = new Vector3(xPosition, yPosition);
        Camera.main.transform.position = new Vector3(xPosition, yPosition);
    }

    [CommandHandler(Name = "PlayDialogue", Description = "Play a Dialogue using the Dialogue System")]
    private void PlayDialogue(string dialogueResourcePath, bool menuSlideAnimation)
    {
        DialogueSet dialogueSetToPlay = Resources.Load<DialogueSet>(dialogueResourcePath);
        FindObjectOfType<DungeonEntryDialogueManager>().dialogueUI.LoadDialogueUI(dialogueSetToPlay, 0f, () => { }, menuSlideAnimation);
    }

    IEnumerator WaitForFogDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        FindObjectOfType<FourthLevelFogController>().DeActivateFog();
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
        PolluxShrine,
        LoneSpark,
        GadgetShot,
        FinBlade,
        RevolvingCannon, 
        SmeltingLaser, 
        TremorMaker
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
