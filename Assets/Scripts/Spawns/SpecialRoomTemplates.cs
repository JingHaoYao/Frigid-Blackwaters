using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialRoomTemplates : MonoBehaviour {
    public string[] miniBosses = new string[3];
    DungeonEntryDialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DungeonEntryDialogueManager>();
    }

    public GameObject loadUniqueRoom(int whatTier, int whichUnique)
    {
        // 1 - shop
        // 2 - gamble
        // 3 - trial
        // 4 - minibosses
        // 5 - trove
        // 6 - treasure
        // 7 - altar

        switch (dialogueManager.whatDungeonLevel)
        {
            case 1:
                switch (whichUnique)
                {
                    case 1:
                        return Resources.Load<GameObject>("Unique Rooms/First Dungeon Level/Shop Rooms/First Dungeon Shop Tier " + whatTier.ToString());
                    case 2:
                        return Resources.Load<GameObject>("Unique Rooms/First Dungeon Level/Gamble Rooms/First Dungeon Gamble Tier " + whatTier.ToString());
                    case 3:
                        return Resources.Load<GameObject>("Unique Rooms/First Dungeon Level/Challenge Rooms/First Dungeon Challenge Room Tier " + whatTier.ToString());
                    case 4:
                        string whichCombat = miniBosses[Random.Range(0, miniBosses.Length)];
                        MiscData.seenEnemies.Add(whichCombat);
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
            case 2:
                switch (whichUnique)
                {
                    case 1:
                        return Resources.Load<GameObject>("Unique Rooms/Second Dungeon Level/Shop Rooms/Second Dungeon Shop Tier " + whatTier.ToString());
                    case 2:
                        return Resources.Load<GameObject>("Unique Rooms/Second Dungeon Level/Gamble Rooms/Second Dungeon Gamble Tier " + whatTier.ToString());
                    case 3:
                        return Resources.Load<GameObject>("Unique Rooms/Second Dungeon Level/Challenge Rooms/Second Dungeon Challenge Room Tier " + whatTier.ToString());
                    case 4:
                        string whichCombat = miniBosses[Random.Range(0, miniBosses.Length)];
                        MiscData.seenEnemies.Add(whichCombat);
                        return Resources.Load<GameObject>("Unique Rooms/Second Dungeon Level/Special Combat Rooms/" + whichCombat + "/" + whichCombat);
                    case 5:
                        return Resources.Load<GameObject>("Unique Rooms/Second Dungeon Level/Trove Rooms/Second Dungeon Trove Tier " + whatTier.ToString());
                    case 6:
                        return Resources.Load<GameObject>("Unique Rooms/Second Dungeon Level/Treasure Rooms/Second Dungeon Level Treasure Room Tier " + whatTier.ToString());
                    case 7:
                        return Resources.Load<GameObject>("Unique Rooms/Second Dungeon Level/Altar Rooms/Second Dungeon Tier " + whatTier.ToString() + " Altar");
                    default:
                        return null;
                }
            case 3:
                switch (whichUnique)
                {
                    case 1:
                        return Resources.Load<GameObject>("Unique Rooms/Third Dungeon Level/Shop Rooms/Third Dungeon Shop Tier " + whatTier.ToString());
                    case 2:
                        return Resources.Load<GameObject>("Unique Rooms/Third Dungeon Level/Gamble Rooms/Third Dungeon Gamble Tier " + whatTier.ToString());
                    case 3:
                        return Resources.Load<GameObject>("Unique Rooms/Third Dungeon Level/Challenge Rooms/Third Dungeon Challenge Room Tier " + whatTier.ToString());
                    case 4:
                        string whichCombat = miniBosses[Random.Range(0, miniBosses.Length)];
                        MiscData.seenEnemies.Add(whichCombat);
                        return Resources.Load<GameObject>("Unique Rooms/Third Dungeon Level/Special Combat Rooms/" + whichCombat + "/" + whichCombat);
                    case 5:
                        return Resources.Load<GameObject>("Unique Rooms/Third Dungeon Level/Trove Rooms/Third Dungeon Trove Tier " + whatTier.ToString());
                    case 6:
                        return Resources.Load<GameObject>("Unique Rooms/Third Dungeon Level/Treasure Rooms/Third Dungeon Level Treasure Room Tier " + whatTier.ToString());
                    case 7:
                        return Resources.Load<GameObject>("Unique Rooms/Third Dungeon Level/Altar Rooms/Third Dungeon Tier " + whatTier.ToString() + " Altar");
                    default:
                        return null;
                }
            case 4:
                switch (whichUnique)
                {
                    case 1:
                        return Resources.Load<GameObject>("Unique Rooms/Fourth Dungeon Level/Shop Rooms/Fourth Dungeon Shop Tier " + whatTier.ToString());
                    case 2:
                        return Resources.Load<GameObject>("Unique Rooms/Fourth Dungeon Level/Gamble Rooms/Fourth Dungeon Gamble Tier " + whatTier.ToString());
                    case 3:
                        return Resources.Load<GameObject>("Unique Rooms/Fourth Dungeon Level/Challenge Rooms/Fourth Dungeon Challenge Room Tier " + whatTier.ToString());
                    case 4:
                        string whichCombat = miniBosses[Random.Range(0, miniBosses.Length)];
                        MiscData.seenEnemies.Add(whichCombat);
                        return Resources.Load<GameObject>("Unique Rooms/Fourth Dungeon Level/Special Combat Rooms/" + whichCombat + "/" + whichCombat);
                    case 5:
                        return Resources.Load<GameObject>("Unique Rooms/Fourth Dungeon Level/Trove Rooms/Fourth Dungeon Trove Tier " + whatTier.ToString());
                    case 6:
                        return Resources.Load<GameObject>("Unique Rooms/Fourth Dungeon Level/Treasure Rooms/Fourth Dungeon Level Treasure Room Tier " + whatTier.ToString());
                    case 7:
                        return Resources.Load<GameObject>("Unique Rooms/Fourth Dungeon Level/Altar Rooms/Fourth Dungeon Tier " + whatTier.ToString() + " Altar");
                    default:
                        return null;
                }
            case 5:
                switch (whichUnique)
                {
                    case 1:
                        return Resources.Load<GameObject>("Unique Rooms/Fifth Dungeon Level/Shop Rooms/Fifth Dungeon Shop Tier " + whatTier.ToString());
                    case 2:
                        return Resources.Load<GameObject>("Unique Rooms/Fifth Dungeon Level/Gamble Rooms/Fifth Dungeon Gamble Tier " + whatTier.ToString());
                    case 3:
                        return Resources.Load<GameObject>("Unique Rooms/Fifth Dungeon Level/Challenge Rooms/Fifth Dungeon Challenge Room Tier " + whatTier.ToString());
                    case 4:
                        string whichCombat = miniBosses[Random.Range(0, miniBosses.Length)];
                        MiscData.seenEnemies.Add(whichCombat);
                        return Resources.Load<GameObject>("Unique Rooms/Fifth Dungeon Level/Special Combat Rooms/" + whichCombat + "/" + whichCombat);
                    case 5:
                        return Resources.Load<GameObject>("Unique Rooms/Fifth Dungeon Level/Trove Rooms/Fifth Dungeon Trove Tier " + whatTier.ToString());
                    case 6:
                        return Resources.Load<GameObject>("Unique Rooms/Fifth Dungeon Level/Treasure Rooms/Fifth Dungeon Level Treasure Room Tier " + whatTier.ToString());
                    case 7:
                        return Resources.Load<GameObject>("Unique Rooms/Fifth Dungeon Level/Altar Rooms/Fifth Dungeon Tier " + whatTier.ToString() + " Altar");
                    default:
                        return null;
                }
        }

        return null;
    } 
}
