using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeRoomItemPicker : MonoBehaviour
{
    ItemTemplates itemTemplates;
    public string[] pickedItems;
    public int whatTier = 1;
    public int goldAmount = 0;

    void Start()
    {
        itemTemplates = FindObjectOfType<ItemTemplates>();
        pickItems();
    }
    
    void pickItems()
    {
        if(whatTier == 1)
        {
            int percentItem = Random.Range(1, 101);
            if(percentItem < 75)
            {
                pickedItems = itemTemplates.tier2Artifacts;
            }
            else
            {
                pickedItems = itemTemplates.tier3Artifacts;
            }

            goldAmount = 300 + Random.Range(0, 4) * 50;
        }
        else if(whatTier == 2)
        {
            int percentItem = Random.Range(1, 101);
            if (percentItem <= 30)
            {
                pickedItems = itemTemplates.tier2Artifacts;
            }
            else if(percentItem >= 30 && percentItem <= 80)
            {
                pickedItems = itemTemplates.tier3Artifacts;
            }
            else
            {
                pickedItems = itemTemplates.tier4Artifacts;
            }
            goldAmount = 450 + Random.Range(0, 4) * 50;
        }
        else
        {
            int percentItem = Random.Range(1, 101);
            if (percentItem <= 70)
            {
                pickedItems = itemTemplates.tier3Artifacts;
            }
            else
            {
                pickedItems = itemTemplates.tier4Artifacts;
            }
            goldAmount = 600 + Random.Range(0, 4) * 50;
        }
    }
}
