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

    public Dictionary<string, string> itemDB = new Dictionary<string, string>();

    /*private void Start()
    {
        tier1Artifacts = new string[tier1ArtifactList.Length];
        tier2Artifacts = new string[tier2ArtifactList.Length];
        tier3Artifacts = new string[tier3ArtifactList.Length];
        tier4Artifacts = new string[tier4ArtifactList.Length];
        consumables = new string[consumableList.Length];
        darkMagics = new string[darkMagicItems.Length];

        for (int i = 0; i < tier1ArtifactList.Length; i++)
        {
            tier1Artifacts[i] = tier1ArtifactList[i].name;
        }

        for (int i = 0; i < tier2ArtifactList.Length; i++)
        {
            tier2Artifacts[i] = tier2ArtifactList[i].name;
        }

        for (int i = 0; i < tier3ArtifactList.Length; i++)
        {
            tier3Artifacts[i] = tier3ArtifactList[i].name;
        }

        for (int i = 0; i < tier4ArtifactList.Length; i++)
        {
            tier4Artifacts[i] = tier4ArtifactList[i].name;
        }

        for (int i = 0; i < consumableList.Length; i++)
        {
            consumables[i] = consumableList[i].name;
        }

        for(int i = 0; i < darkMagicItems.Length; i++)
        {
            darkMagics[i] = darkMagicItems[i].name;
        }
    }*/

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
        else
        {
            return null;
        }
    }
}
