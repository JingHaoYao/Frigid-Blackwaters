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
    public static bool muted = false;
    public static bool playerDied = false;
    public static int numberDungeonRuns = 0;
    public static bool skillPointsNotification = false;

    //Dialogues that have been completed
    public static List<string> completedTavernDialogues = new List<string>();
    public static List<string> completedEntryDungeonDialogues = new List<string>();
    public static List<string> completedExamineDialogues = new List<string>();
    public static List<string> completedShopDialogues = new List<string>();
    public static List<string> completedStoryDialogues = new List<string>();

    //tutorial dialogues
    public static List<string> completedUniqueRoomsDialogues = new List<string>();

    //check points in the game that have been completed
    public static List<string> completedCheckPoints = new List<string>();


    //which buildings have been unlocked
    public static List<string> unlockedBuildings = new List<string>();

    //tutorial related
    public static bool finishedTutorial = false;
    public static bool questSymbolShown = false;
    public static bool dungeonMapSymbolShown = false;

    //Check for random dungeon entry chance
    public static bool enoughRoomsTraversed = true;

    //what dungeon stages unlocked
    public static int dungeonLevelUnlocked = 1;

    // Story Mission Related Parameters
    public static string missionID = "defeat_undead_mariner";
    public static bool finishedMission;
    public static List<string> completedMissions = new List<string>();
}
