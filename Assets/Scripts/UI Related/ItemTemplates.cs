using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTemplates : MonoBehaviour {
    // All these items do not contain quest items, as quest items do not 
    // need to be transfered between scenes.

    public GameObject gold;

    //first dungeon stage items
    public string[] tier1Artifacts;
    public string[] tier2Artifacts;
    public string[] tier3Artifacts;
    public string[] tier4Artifacts;
    public string[] consumables;
    public string[] uniqueConsumables;
    public string[] uniqueArtifacts;

    public string[] secondLevelTier1Artifacts;
    public string[] secondLevelTier2Artifacts;
    public string[] secondLevelTier3Artifacts;
    public string[] secondLevelTier4Artifacts;
    public string[] secondLevelConsumables;
    public string[] secondLevelUniqueArtifacts;

    public string[] thirdLevelTier1Artifacts;
    public string[] thirdLevelTier2Artifacts;
    public string[] thirdLevelTier3Artifacts;
    public string[] thirdLevelTier4Artifacts;
    public string[] thirdLevelConsumables;
    public string[] thirdLevelUniqueArtifacts;
    public string[] thirdLevelUniqueConsumables;

    public string[] fourthLevelTier1Artifacts;
    public string[] fourthLevelTier2Artifacts;
    public string[] fourthLevelTier3Artifacts;
    public string[] fourthLevelTier4Artifacts;
    public string[] fourthLevelUniqueArtifacts;
    public string[] fourthLevelConsumables;

    public Dictionary<string, string> itemDB = new Dictionary<string, string>();

    DungeonEntryDialogueManager dungeonEntryDialogueManager;

    private void Awake()
    {
        itemDB.Add("GoldItem", "Items/");
        dungeonEntryDialogueManager = FindObjectOfType<DungeonEntryDialogueManager>();

        // first dungeon stage items
        for (int i = 0; i < tier1Artifacts.Length; i++)
        {
            itemDB.Add(tier1Artifacts[i], "Items/First Dungeon Stage/Artifacts/Common Artifacts/");
        }

        for (int i = 0; i < tier2Artifacts.Length; i++)
        {
            itemDB.Add(tier2Artifacts[i], "Items/First Dungeon Stage/Artifacts/Uncommon Artifacts/");
        }

        for (int i = 0; i < tier3Artifacts.Length; i++)
        {
            itemDB.Add(tier3Artifacts[i], "Items/First Dungeon Stage/Artifacts/Rare Artifacts/");
        }

        for (int i = 0; i < tier4Artifacts.Length; i++)
        {
            itemDB.Add(tier4Artifacts[i], "Items/First Dungeon Stage/Artifacts/Legendary Artifacts/");
        }

        for (int i = 0; i < consumables.Length; i++)
        {
            itemDB.Add(consumables[i], "Items/First Dungeon Stage/Consumables/Regular Consumables/");
        }

        for (int i = 0; i < uniqueArtifacts.Length; i++)
        {
            itemDB.Add(uniqueArtifacts[i], "Items/First Dungeon Stage/Artifacts/Unique Artifacts/");
        }

        for (int i = 0; i < uniqueConsumables.Length; i++)
        {
            itemDB.Add(uniqueConsumables[i], "Items/First Dungeon Stage/Consumables/Unique Consumables/");
        }

        foreach(string id in secondLevelTier1Artifacts)
        {
            itemDB.Add(id, "Items/Second Dungeon Level/Artifacts/Common Artifacts/");
        }

        foreach (string id in secondLevelTier2Artifacts)
        {
            itemDB.Add(id, "Items/Second Dungeon Level/Artifacts/Uncommon Artifacts/");
        }

        foreach (string id in secondLevelTier3Artifacts)
        {
            itemDB.Add(id, "Items/Second Dungeon Level/Artifacts/Rare Artifacts/");
        }

        foreach (string id in secondLevelTier4Artifacts)
        {
            itemDB.Add(id, "Items/Second Dungeon Level/Artifacts/Legendary Artifacts/");
        }

        foreach (string id in secondLevelConsumables)
        {
            itemDB.Add(id, "Items/Second Dungeon Level/Consumables/Regular Consumables/");
        }

        foreach(string id in secondLevelUniqueArtifacts)
        {
            itemDB.Add(id, "Items/Second Dungeon Level/Artifacts/Unique Artifacts/");
        }

        foreach (string id in thirdLevelTier1Artifacts)
        {
            itemDB.Add(id, "Items/Third Dungeon Level/Artifacts/Common Artifacts/");
        }

        foreach (string id in thirdLevelTier2Artifacts)
        {
            itemDB.Add(id, "Items/Third Dungeon Level/Artifacts/Uncommon Artifacts/");
        }

        foreach (string id in thirdLevelTier3Artifacts)
        {
            itemDB.Add(id, "Items/Third Dungeon Level/Artifacts/Rare Artifacts/");
        }

        foreach (string id in thirdLevelTier4Artifacts)
        {
            itemDB.Add(id, "Items/Third Dungeon Level/Artifacts/Legendary Artifacts/");
        }

        foreach (string id in thirdLevelConsumables)
        {
            itemDB.Add(id, "Items/Third Dungeon Level/Consumables/Regular Consumables/");
        }

        foreach (string id in thirdLevelUniqueArtifacts)
        {
            itemDB.Add(id, "Items/Third Dungeon Level/Artifacts/Unique Artifacts/");
        }

        foreach(string id in thirdLevelUniqueConsumables)
        {
            itemDB.Add(id, "Items/Third Dungeon Level/Consumables/Special Consumables/");
        }

        foreach(string id in fourthLevelTier1Artifacts)
        {
            itemDB.Add(id, "Items/Fourth Dungeon Level/Artifacts/Common Artifacts/");
        }

        foreach (string id in fourthLevelTier2Artifacts)
        {
            itemDB.Add(id, "Items/Fourth Dungeon Level/Artifacts/Uncommon Artifacts/");
        }

        foreach (string id in fourthLevelTier3Artifacts)
        {
            itemDB.Add(id, "Items/Fourth Dungeon Level/Artifacts/Rare Artifacts/");
        }

        foreach (string id in fourthLevelTier4Artifacts)
        {
            itemDB.Add(id, "Items/Fourth Dungeon Level/Artifacts/Legendary Artifacts/");
        }

        foreach(string id in fourthLevelConsumables)
        {
            itemDB.Add(id, "Items/Fourth Dungeon Level/Consumables/Regular Consumables");
        }

        foreach (string id in fourthLevelUniqueArtifacts)
        {
            itemDB.Add(id, "Items/Fourth Dungeon Level/Artifacts/Unique Artifacts");
        }
    }

    public bool dbContainsID(string item_id)
    {
        string id = item_id;
        if (item_id.Contains("(Clone)"))
        {
            id = item_id.Replace("(Clone)", "").Trim();
        }

        if (itemDB.ContainsKey(id))
        {
            return true;
        }
        return false;
    }

    public GameObject loadItem(string item_id)
    {
        string id = item_id;
        if (item_id.Contains("(Clone)"))
        {
            id = item_id.Replace("(Clone)", "").Trim();
        }

        GameObject item = Resources.Load<DisplayItem>(itemDB[id] + id).gameObject;
        return item;
    }

    public GameObject loadRandomItem(int whatTier)
    {
        if (dungeonEntryDialogueManager.whatDungeonLevel == 1)
        {
            GameObject spawnedItem = null;
            switch (whatTier)
            {
                case 1:
                    spawnedItem = Resources.Load<GameObject>("Items/First Dungeon Stage/Artifacts/Common Artifacts/" + tier1Artifacts[Random.Range(0, tier1Artifacts.Length)]);
                    break;
                case 2:
                    spawnedItem = Resources.Load<GameObject>("Items/First Dungeon Stage/Artifacts/Uncommon Artifacts/" + tier2Artifacts[Random.Range(0, tier2Artifacts.Length)]);
                    break;
                case 3:
                    spawnedItem = Resources.Load<GameObject>("Items/First Dungeon Stage/Artifacts/Rare Artifacts/" + tier3Artifacts[Random.Range(0, tier3Artifacts.Length)]);
                    break;
                case 4:
                    spawnedItem = Resources.Load<GameObject>("Items/First Dungeon Stage/Artifacts/Legendary Artifacts/" + tier4Artifacts[Random.Range(0, tier4Artifacts.Length)]);
                    break;
                case 5:
                    spawnedItem = Resources.Load<GameObject>("Items/First Dungeon Stage/Consumables/Regular Consumables/" + consumables[Random.Range(0, consumables.Length)]);
                    break;
                default:
                    spawnedItem = Resources.Load<GameObject>("Items/First Dungeon Stage/Consumables/Regular Consumables/" + consumables[Random.Range(0, consumables.Length)]);
                    break;
            }
            return Instantiate(spawnedItem);
        }
        else if(dungeonEntryDialogueManager.whatDungeonLevel == 2)
        {
            GameObject spawnedItem = null;
            switch (whatTier)
            {
                case 1:
                    spawnedItem = Resources.Load<GameObject>("Items/Second Dungeon Level/Artifacts/Common Artifacts/" + secondLevelTier1Artifacts[Random.Range(0, secondLevelTier1Artifacts.Length)]);
                    break;
                case 2:
                    spawnedItem = Resources.Load<GameObject>("Items/Second Dungeon Level/Artifacts/Uncommon Artifacts/" + secondLevelTier2Artifacts[Random.Range(0, secondLevelTier2Artifacts.Length)]);
                    break;
                case 3:
                    spawnedItem = Resources.Load<GameObject>("Items/Second Dungeon Level/Artifacts/Rare Artifacts/" + secondLevelTier3Artifacts[Random.Range(0, secondLevelTier3Artifacts.Length)]);
                    break;
                case 4:
                    spawnedItem = Resources.Load<GameObject>("Items/Second Dungeon Level/Artifacts/Legendary Artifacts/" + secondLevelTier4Artifacts[Random.Range(0, secondLevelTier4Artifacts.Length)]);
                    break;
                case 5:
                    spawnedItem = Resources.Load<GameObject>("Items/Second Dungeon Level/Consumables/Regular Consumables/" + secondLevelConsumables[Random.Range(0, secondLevelConsumables.Length)]);
                    break;
                default:
                    spawnedItem = Resources.Load<GameObject>("Items/Second Dungeon Level/Consumables/Regular Consumables/" + secondLevelConsumables[Random.Range(0, secondLevelConsumables.Length)]);
                    break;
            }
            return Instantiate(spawnedItem);
        }
        else if (dungeonEntryDialogueManager.whatDungeonLevel == 3)
        {
            GameObject spawnedItem = null;
            switch (whatTier)
            {
                case 1:
                    spawnedItem = Resources.Load<GameObject>("Items/Third Dungeon Level/Artifacts/Common Artifacts/" + thirdLevelTier1Artifacts[Random.Range(0, thirdLevelTier1Artifacts.Length)]);
                    break;
                case 2:
                    spawnedItem = Resources.Load<GameObject>("Items/Third Dungeon Level/Artifacts/Uncommon Artifacts/" + thirdLevelTier2Artifacts[Random.Range(0, thirdLevelTier2Artifacts.Length)]);
                    break;
                case 3:
                    spawnedItem = Resources.Load<GameObject>("Items/Third Dungeon Level/Artifacts/Rare Artifacts/" + thirdLevelTier3Artifacts[Random.Range(0, thirdLevelTier3Artifacts.Length)]);
                    break;
                case 4:
                    spawnedItem = Resources.Load<GameObject>("Items/Third Dungeon Level/Artifacts/Legendary Artifacts/" + thirdLevelTier4Artifacts[Random.Range(0, thirdLevelTier4Artifacts.Length)]);
                    break;
                case 5:
                    spawnedItem = Resources.Load<GameObject>("Items/Third Dungeon Level/Consumables/Regular Consumables/" + thirdLevelConsumables[Random.Range(0, thirdLevelConsumables.Length)]);
                    break;
                default:
                    spawnedItem = Resources.Load<GameObject>("Items/Third Dungeon Level/Consumables/Regular Consumables/" + thirdLevelConsumables[Random.Range(0, thirdLevelConsumables.Length)]);
                    break;
            }
            return Instantiate(spawnedItem);
        }
        else if (dungeonEntryDialogueManager.whatDungeonLevel == 4)
        {
            GameObject spawnedItem = null;
            switch (whatTier)
            {
                case 1:
                    spawnedItem = Resources.Load<GameObject>("Items/Fourth Dungeon Level/Artifacts/Common Artifacts/" + fourthLevelTier1Artifacts[Random.Range(0, thirdLevelTier1Artifacts.Length)]);
                    break;
                case 2:
                    spawnedItem = Resources.Load<GameObject>("Items/Fourth Dungeon Level/Artifacts/Uncommon Artifacts/" + fourthLevelTier2Artifacts[Random.Range(0, thirdLevelTier2Artifacts.Length)]);
                    break;
                case 3:
                    spawnedItem = Resources.Load<GameObject>("Items/Fourth Dungeon Level/Artifacts/Rare Artifacts/" + fourthLevelTier3Artifacts[Random.Range(0, thirdLevelTier3Artifacts.Length)]);
                    break;
                case 4:
                    spawnedItem = Resources.Load<GameObject>("Items/Fourth Dungeon Level/Artifacts/Legendary Artifacts/" + fourthLevelTier4Artifacts[Random.Range(0, thirdLevelTier4Artifacts.Length)]);
                    break;
                case 5:
                    spawnedItem = Resources.Load<GameObject>("Items/Fourth Dungeon Level/Consumables/Regular Consumables/" + fourthLevelConsumables[Random.Range(0, thirdLevelConsumables.Length)]);
                    break;
                default:
                    spawnedItem = Resources.Load<GameObject>("Items/Fourth Dungeon Level/Consumables/Regular Consumables/" + fourthLevelConsumables[Random.Range(0, thirdLevelConsumables.Length)]);
                    break;
            }
            return Instantiate(spawnedItem);
        }
        else
        {
            return null;
        }
    }
}
