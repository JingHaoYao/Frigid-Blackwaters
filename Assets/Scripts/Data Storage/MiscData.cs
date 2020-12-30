using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MiscData
{
    public static bool readFirstBossDialogue = false;
    public static int numberQuestsCompleted = 0;
    public static List<string> bossesDefeated = new List<string>();
    public static float masterVolume = 0;
    public static float effectsVolume = 0;
    public static float musicVolume = 0;
    public static bool fullScreen = true;
    public static int qualityIndex = 5;
    public static int resolutionIndex = 0;
    public static bool muted = false;
    public static bool playerDied = false;
    public static int numberDungeonRuns = 0;
    public static bool skillPointsNotification = false;

    public static bool unlockedArticrafting = false;

    public static List<string> firstTimeTutorialsPlayed = new List<string>();

    //Dialogues that have been completed
    public static List<string> completedTavernDialogues = new List<string>();
    public static List<string> completedEntryDungeonDialogues = new List<string>();
    public static List<string> completedExamineDialogues = new List<string>();
    public static List<string> completedShopDialogues = new List<string>();
    public static List<string> completedStoryDialogues = new List<string>();
    public static List<string> completedHubReturnDialogues = new List<string>();

    //Tutorial dialogues
    public static List<string> completedUniqueRoomsDialogues = new List<string>();

    //Check points in the game that have been completed
    public static List<string> completedCheckPoints = new List<string>();

    //Which buildings have been unlocked
    public static List<string> unlockedBuildings = new List<string>();

    //Tutorial related
    public static bool finishedTutorial = false;
    public static bool questSymbolShown = false;
    public static bool dungeonMapSymbolShown = false;

    //Check for random dungeon entry chance
    public static bool enoughRoomsTraversed = true;

    //What dungeon stages unlocked
    public static int dungeonLevelUnlocked = 1;

    //Story mission related parameters
    public static string missionID;
    public static bool finishedMission;
    public static List<string> completedMissions = new List<string>();

    //Enemy encyclopedia related
    public static HashSet<string> seenEnemies = new HashSet<string>();
}
