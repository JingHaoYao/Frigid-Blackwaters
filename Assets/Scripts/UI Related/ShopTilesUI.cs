using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTilesUI : MonoBehaviour {
    public List<GameObject> shopItemList;
    public List<int> prices;

    public ShopTile[] shopTiles;
    public Text totalStoredGold;

	void Start () {
        shopTiles = GetComponentsInChildren<ShopTile>();
        updateUI();
	}

    string pickDisplay(int goldAmount)
    {
        if (goldAmount < 1000)
        {
            return goldAmount.ToString();
        }
        else if (goldAmount < 100000)
        {
            string goldToDisplay = ((float)goldAmount / 1000).ToString();
            return goldToDisplay.Substring(0, Mathf.Clamp(goldToDisplay.Length, 0, 4)) + "K";
        }
        else if (goldAmount < 1000000)
        {
            string goldToDisplay = ((float)goldAmount / 1000).ToString();
            return goldToDisplay.Substring(0, Mathf.Clamp(goldToDisplay.Length, 0, 3)) + "K";
        }
        else if (goldAmount < 10000000)
        {
            string goldToDisplay = ((float)goldAmount / 1000000).ToString();
            return goldToDisplay.Substring(0, Mathf.Clamp(goldToDisplay.Length, 0, 3)) + "M";
        }
        else
        {
            string goldToDisplay = ((float)goldAmount / 1000000).ToString();
            return goldToDisplay.Substring(0, Mathf.Clamp(goldToDisplay.Length, 0, 4)) + "M";
        }
    }

    public void updateUI()
    {
        if(totalStoredGold != null)
        {
            totalStoredGold.text = pickDisplay(HubProperties.storeGold);
        }

        for(int i = 0; i < shopTiles.Length; i++)
        {
            if (i < shopItemList.Count){
                shopTiles[i].displayInfo = shopItemList[i].GetComponent<DisplayItem>();
                shopTiles[i].price = prices[i];
                shopTiles[i].addSlot(shopTiles[i].displayInfo);
            }
            else
            {
                shopTiles[i].deleteSlot();
            }
        }
    }

    private void OnEnable()
    {
        updateUI();
    }
}
