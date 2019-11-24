using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //members needed to store vital information
    //about the golden vault in hub.
    public string[] storedVaultItems;
    public int storedVaultGold;

    //members needed to store info about upgrades
    public string[] musketUpgrades;
    public string[] cannonUpgrades;
    public string[] spreadShotUpgrades;
    public string[] fireworkUpgrades;
    public string[] dragonsBreathUpgrades;
    public string[] sniperUpgrades;
    public string[] hullUpgrades;
    public string[] inventoryUpgrades;
    public string[] safeUpgrades;
    public int numberSkillPoints;
    public int numberMaxSkillPoints;
    public int whichFrontWeaponEquipped;
    public int whichLeftWeaponEquipped;
    public int whichRightWeaponEquipped;
    public bool musketUnlocked, cannonUnlocked, spreadShotUnlocked,
                fireworkUnlocked, dragonsBreathUnlocked;
    public int unlockLevel;

    //members needed to store info about the player's
    //current information
    public string[] inventoryItemIds;
    public string[] equippedArtifactIds;
    public int totalGoldAmount;
    public int maxInventorySize;

    //misc data
    public bool finishedTutorial;
    public string currentQuestID;
    public bool finishedQuest;
    public string[] availableQuests;
    public string availableBossQuest;
    public int numberQuestsCompleted;

    public string[] completedBosses;

    public string upBinding;
    public string leftBinding;
    public string rightBinding;
    public string downBinding;
    public string dashBinding;
    public string artActive1;
    public string artActive2;
    public string artActive3;
    public bool targetConesEnabled;

    public string[] completedTavernDialogues;
    public string[] completedDungeonEntryDialogues;
    public string[] completedExamineDialogues;
    public string[] completedShopDialogues;
    public string[] completedStoryDialogues;
    public string[] unlockedBuildings;
    public string[] completedUniqueRoomDialogues;

    public string[] completedCheckPoints;

    public bool playerDied;

    public int numberDungeonRuns;

    public bool enoughRoomsTraversed;

    public int dungeonLevelUnlocked;

    public bool questSymbolShown;

    public bool dungeonMapSymbolShown;

    public bool skillPointsNotification;

    public string missionID;
    public bool missionFinished;

    public string[] completedMissions;

    public SaveData()
    {
        storedVaultItems = new string[HubProperties.vaultItems.Count];
        for(int i = 0; i < storedVaultItems.Length; i++)
        {
            storedVaultItems[i] = HubProperties.vaultItems[i];
        }
        storedVaultGold = HubProperties.storeGold;

        musketUpgrades = new string[PlayerUpgrades.musketUpgrades.Count];
        for(int i = 0; i < musketUpgrades.Length; i++)
        {
            musketUpgrades[i] = PlayerUpgrades.musketUpgrades[i];
        }
        cannonUpgrades = new string[PlayerUpgrades.cannonUpgrades.Count];
        for (int i = 0; i < cannonUpgrades.Length; i++)
        {
            cannonUpgrades[i] = PlayerUpgrades.cannonUpgrades[i];
        }
        spreadShotUpgrades = new string[PlayerUpgrades.spreadshotUpgrades.Count];
        for (int i = 0; i < spreadShotUpgrades.Length; i++)
        {
            spreadShotUpgrades[i] = PlayerUpgrades.spreadshotUpgrades[i];
        }
        fireworkUpgrades = new string[PlayerUpgrades.fireworkUpgrades.Count];
        for (int i = 0; i < fireworkUpgrades.Length; i++)
        {
            fireworkUpgrades[i] = PlayerUpgrades.fireworkUpgrades[i];
        }
        dragonsBreathUpgrades = new string[PlayerUpgrades.dragonBreathUpgrades.Count];
        for (int i = 0; i < dragonsBreathUpgrades.Length; i++)
        {
            dragonsBreathUpgrades[i] = PlayerUpgrades.dragonBreathUpgrades[i];
        }
        sniperUpgrades = new string[PlayerUpgrades.sniperUpgrades.Count];
        for(int i = 0; i < sniperUpgrades.Length; i++)
        {
            sniperUpgrades[i] = PlayerUpgrades.sniperUpgrades[i];
        }

        hullUpgrades = new string[PlayerUpgrades.hullUpgrades.Count];
        for (int i = 0; i < hullUpgrades.Length; i++)
        {
            hullUpgrades[i] = PlayerUpgrades.hullUpgrades[i];
        }
        inventoryUpgrades = new string[PlayerUpgrades.inventoryUpgrades.Count];
        for (int i = 0; i < inventoryUpgrades.Length; i++)
        {
            inventoryUpgrades[i] = PlayerUpgrades.inventoryUpgrades[i];
        }
        safeUpgrades = new string[PlayerUpgrades.safeUpgrades.Count];
        for(int i = 0; i < safeUpgrades.Length; i++)
        {
            safeUpgrades[i] = PlayerUpgrades.safeUpgrades[i];
        }
        numberSkillPoints = PlayerUpgrades.numberSkillPoints;
        numberMaxSkillPoints = PlayerUpgrades.numberMaxSkillPoints;
        whichFrontWeaponEquipped = PlayerUpgrades.whichFrontWeaponEquipped;
        whichLeftWeaponEquipped = PlayerUpgrades.whichLeftWeaponEquipped;
        whichRightWeaponEquipped = PlayerUpgrades.whichRightWeaponEquipped;
        musketUnlocked = PlayerUpgrades.musketUnlocked;
        spreadShotUnlocked = PlayerUpgrades.spreadShotUnlocked;
        cannonUnlocked = PlayerUpgrades.cannonUnlocked;
        fireworkUnlocked = PlayerUpgrades.fireworkUnlocked;
        dragonsBreathUnlocked = PlayerUpgrades.dragonsBreathUnlocked;
        unlockLevel = PlayerUpgrades.unlockLevel;

        inventoryItemIds = new string[PlayerItems.inventoryItemsIDs.Count];
        for(int i = 0; i < inventoryItemIds.Length; i++)
        {
            inventoryItemIds[i] = PlayerItems.inventoryItemsIDs[i];
        }
        equippedArtifactIds = PlayerItems.activeArtifactsIDs;
        totalGoldAmount = PlayerItems.totalGoldAmount;
        maxInventorySize = PlayerItems.maxInventorySize;

        finishedTutorial = MiscData.finishedTutorial;
        numberQuestsCompleted = MiscData.numberQuestsCompleted;

        completedBosses = new string[MiscData.bossesDefeated.Count];
        for(int i = 0; i < MiscData.bossesDefeated.Count; i++)
        {
            completedBosses[i] = MiscData.bossesDefeated[i];
        }

        upBinding = SavedKeyBindings.moveUp;
        leftBinding = SavedKeyBindings.moveLeft;
        rightBinding = SavedKeyBindings.moveRight;
        downBinding = SavedKeyBindings.moveDown;
        dashBinding = SavedKeyBindings.dash;
        artActive1 = SavedKeyBindings.firstArtifact;
        artActive2 = SavedKeyBindings.secondArtifact;
        artActive3 = SavedKeyBindings.thirdArtifact;
        targetConesEnabled = SavedKeyBindings.targetConesEnabled;

        completedTavernDialogues = new string[MiscData.completedTavernDialogues.Count];
        for(int i = 0; i < completedTavernDialogues.Length; i++)
        {
            completedTavernDialogues[i] = MiscData.completedTavernDialogues[i];
        }

        completedDungeonEntryDialogues = new string[MiscData.completedEntryDungeonDialogues.Count];
        for (int i = 0; i < completedDungeonEntryDialogues.Length; i++)
        {
            completedDungeonEntryDialogues[i] = MiscData.completedEntryDungeonDialogues[i];
        }

        completedExamineDialogues = new string[MiscData.completedExamineDialogues.Count];
        for(int i = 0; i < completedExamineDialogues.Length; i++)
        {
            completedExamineDialogues[i] = MiscData.completedExamineDialogues[i];
        }

        completedShopDialogues = new string[MiscData.completedShopDialogues.Count];
        for(int i = 0; i < completedShopDialogues.Length; i++)
        {
            completedShopDialogues[i] = MiscData.completedShopDialogues[i];
        }

        completedStoryDialogues = new string[MiscData.completedStoryDialogues.Count];
        for(int i = 0; i < completedStoryDialogues.Length; i++)
        {
            completedStoryDialogues[i] = MiscData.completedStoryDialogues[i];
        }

        completedCheckPoints = new string[MiscData.completedCheckPoints.Count];
        for(int i = 0; i < completedCheckPoints.Length; i++)
        {
            completedCheckPoints[i] = MiscData.completedCheckPoints[i];
        }

        unlockedBuildings = new string[MiscData.unlockedBuildings.Count];
        for(int i = 0; i < unlockedBuildings.Length; i++)
        {
            unlockedBuildings[i] = MiscData.unlockedBuildings[i];
        }

        completedUniqueRoomDialogues = new string[MiscData.completedUniqueRoomsDialogues.Count];
        for(int i = 0; i < completedUniqueRoomDialogues.Length; i++)
        {
            completedUniqueRoomDialogues[i] = MiscData.completedUniqueRoomsDialogues[i];
        }

        playerDied = MiscData.playerDied;
        numberDungeonRuns = MiscData.numberDungeonRuns;
        enoughRoomsTraversed = MiscData.enoughRoomsTraversed;
        dungeonLevelUnlocked = MiscData.dungeonLevelUnlocked;
        skillPointsNotification = MiscData.skillPointsNotification;

        questSymbolShown = MiscData.questSymbolShown;
        dungeonMapSymbolShown = MiscData.dungeonMapSymbolShown;

        missionID = MiscData.missionID;
        missionFinished = MiscData.finishedMission;

        completedMissions = new string[MiscData.completedMissions.Count];
        for(int i = 0; i < completedMissions.Length; i++)
        {
            completedMissions[i] = MiscData.completedMissions[i];
        }
    }
}
