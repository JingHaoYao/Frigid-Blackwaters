using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerItems{
    public static List<string> inventoryItemsIDs = new List<string>();
    public static string[] activeArtifactsIDs = new string[3];
    public static int totalGoldAmount;
    public static int maxInventorySize = 10;
    public static int playerDamage = 0;
    public static int numberArtifactKills = 0;
}
