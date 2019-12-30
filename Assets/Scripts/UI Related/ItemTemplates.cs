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
    public string[] darkMagics;
    public string[] uniqueConsumables;
    public string[] uniqueArtifacts;

    public string[] secondLevelTier1Artifacts;
    public string[] secondLevelTier2Artifacts;
    public string[] secondLevelTier3Artifacts;
    public string[] secondLevelTier4Artifacts;
    public string[] secondLevelConsumables;
    public string[] secondLevelUniqueArtifacts;

    public Dictionary<string, string> itemDB = new Dictionary<string, string>();

    private void Awake()
    {
        itemDB.Add("GoldItem", "Items/");

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

        for (int i = 0; i < darkMagics.Length; i++)
        {
            itemDB.Add(darkMagics[i], "Items/First Dungeon Stage/Consumables/Dark Magic Consumables/");
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
            itemDB.Add(id, "Items/Second Dungeon Level/Consumables/Unique Artifacts/");
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
        if (FindObjectOfType<DungeonEntryDialogueManager>().whatDungeonLevel == 1)
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
                case 6:
                    spawnedItem = Resources.Load<GameObject>("Items/First Dungeon Stage/Consumables/Dark Magic Consumables/" + darkMagics[Random.Range(0, darkMagics.Length)]);
                    break;
            }
            return Instantiate(spawnedItem);
        }
        else if(FindObjectOfType<DungeonEntryDialogueManager>().whatDungeonLevel == 2)
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
                case 6:
                    spawnedItem = Resources.Load<GameObject>("Items/Second Dungeon Level/Consumables/Regular Consumables/" + secondLevelConsumables[Random.Range(0, secondLevelConsumables.Length)]);
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
