using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialRoomTemplates : MonoBehaviour {
    public string[] miniBosses = new string[3];

    public GameObject loadUniqueRoom(int whatTier, int whichUnique)
    {
        // 1 - shop
        // 2 - gamble
        // 3 - trial
        // 4 - minibosses
        // 5 - trove
        // 6 - treasure
        // 7 - altar
        if (FindObjectOfType<DungeonEntryDialogueManager>().whatDungeonLevel == 1) {
            switch (whichUnique) {
                case 1:
                    return Resources.Load<GameObject>("Unique Rooms/First Dungeon Level/Shop Rooms/First Dungeon Shop Tier " + whatTier.ToString());
                case 2:
                    return Resources.Load<GameObject>("Unique Rooms/First Dungeon Level/Gamble Rooms/First Dungeon Gamble Tier " + whatTier.ToString());
                case 3:
                    return Resources.Load<GameObject>("Unique Rooms/First Dungeon Level/Challenge Rooms/First Dungeon Challenge Room Tier " + whatTier.ToString());
                case 4:
                    string whichCombat = miniBosses[Random.Range(0, miniBosses.Length)];
                    return Resources.Load<GameObject>("Unique Rooms/First Dungeon Level/Special Combat Rooms/" + whichCombat + "/" + whichCombat);
                case 5:
                    return Resources.Load<GameObject>("Unique Rooms/First Dungeon Level/Trove Rooms/First Dungeon Trove Tier " + whatTier.ToString());
                case 6:
                    return Resources.Load<GameObject>("Unique Rooms/First Dungeon Level/Treasure Rooms/First Dungeon Level Treasure Room Tier " + whatTier.ToString());
                case 7:
                    return Resources.Load<GameObject>("Unique Rooms/First Dungeon Level/Altar Rooms/First Dungeon Tier " + whatTier.ToString() + " Altar");
                default:
                    return null;
            }
        }
        else
        {
            return null;
        }
    } 
}
