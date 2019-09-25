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

	void Update () {
		
	}

    public void updateUI()
    {
        if(totalStoredGold != null)
        {
            totalStoredGold.text = HubProperties.storeGold.ToString();
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
