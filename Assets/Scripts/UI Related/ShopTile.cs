using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopTile : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{
    public int price;
    public GameObject imageIcon, priceShower;
    public DisplayItem displayInfo;
    public Inventory inventory;
    public GameObject toolTip;
    public ShopTilesUI shopTilesUI;
    public bool isHubTile = false;

    void Start () {
        inventory = GameObject.Find("PlayerShip").GetComponent<Inventory>();
        toolTip = inventory.toolTip;
        shopTilesUI = transform.parent.GetComponent<ShopTilesUI>();
    }

	void Update () {
	}

    int tallyGold()
    {
        int totalGold = 0;
        if (inventory.itemList.Count > 0)
        {
            for(int i = 0; i < inventory.itemList.Count; i++) {
                totalGold += inventory.itemList[i].GetComponent<DisplayItem>().goldValue;
            }
        }
        return totalGold;
    }

    public void purchaseItem()
    {
        if (isHubTile == false)
        {
            int gold = tallyGold();
            int remainder = price;

            int goldStacks = Mathf.FloorToInt((float)gold / 1000);

            int goldStacksAfterPriceReduction = Mathf.FloorToInt((float)(gold - price) / 1000);

            if
            (
               gold >= price
               && (inventory.itemList.Count < PlayerItems.maxInventorySize || goldStacksAfterPriceReduction < goldStacks)
               && displayInfo != null
            )
            {
                inventory.itemList.Add(displayInfo.gameObject);
                int index = inventory.itemList.Count - 1;
                while (remainder > 0)
                {
                    while (inventory.itemList[index].GetComponent<DisplayItem>().goldValue == 0)
                    {
                        index--;
                    }

                    if (remainder >= inventory.itemList[index].GetComponent<DisplayItem>().goldValue)
                    {
                        remainder -= inventory.itemList[index].GetComponent<DisplayItem>().goldValue;
                        inventory.itemList.Remove(inventory.itemList[index]);
                    }
                    else
                    {
                        inventory.itemList[index].GetComponent<DisplayItem>().goldValue -= remainder;
                        remainder = 0;
                    }
                }
                int itemListIndex = shopTilesUI.shopItemList.IndexOf(displayInfo.gameObject);
                shopTilesUI.shopItemList.Remove(displayInfo.gameObject);
                shopTilesUI.prices.Remove(shopTilesUI.prices[itemListIndex]);
                deleteSlot();
                inventory.UpdateUI();
                FindObjectOfType<AudioManager>().PlaySound("Purchase Item");
            }
        }
        else
        {
            int gold = HubProperties.storeGold;
            if
            (
               gold >= price
               && inventory.itemList.Count < GameObject.Find("PlayerShip").GetComponent<Inventory>().inventorySize
               && displayInfo != null
            )
            {
                HubProperties.storeGold -= price;
                inventory.itemList.Add(displayInfo.gameObject);
                int itemListIndex = shopTilesUI.shopItemList.IndexOf(displayInfo.gameObject);
                shopTilesUI.shopItemList.Remove(displayInfo.gameObject);
                shopTilesUI.prices.Remove(shopTilesUI.prices[itemListIndex]);
                deleteSlot();
                inventory.UpdateUI();
                FindObjectOfType<AudioManager>().PlaySound("Purchase Item");
            }
        }
        shopTilesUI.updateUI();
    }

    public void addSlot(DisplayItem _displayInfo)
    {
        displayInfo = _displayInfo;
        imageIcon.SetActive(true);
        priceShower.SetActive(true);
        priceShower.GetComponent<Text>().text = price.ToString();
        imageIcon.GetComponent<Image>().sprite = _displayInfo.displayIcon;
    }

    public void deleteSlot()
    {
        displayInfo = null;
        imageIcon.SetActive(false);
        priceShower.SetActive(false);
        if(toolTip)
            toolTip.SetActive(false);
    }

    public void exitToolTip()
    {
        if (displayInfo != null)
        {
            toolTip.SetActive(false);
        }
    }

    public void enterToolTip()
    {

        if (displayInfo != null)
        {
            PlayerProperties.toolTip.SetTextAndPosition(displayInfo.GetComponent<Text>().text, transform.position);
        }
    }
}
