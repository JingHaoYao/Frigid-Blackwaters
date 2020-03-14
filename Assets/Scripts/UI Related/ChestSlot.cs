using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChestSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject imageIcon, goldInfo;
    public Chest targetChest;
    public DisplayItem displayInfo;
    Inventory inventory;
    public GameObject toolTip;
    ItemTemplates itemTemplates;
    GameObject presentItems;

    private void Start()
    {
        inventory = PlayerProperties.playerInventory;
        itemTemplates = FindObjectOfType<ItemTemplates>();
        presentItems = GameObject.Find("PresentItems");
        toolTip = inventory.toolTip;
    }

    public void addSlot(DisplayItem _displayInfo)
    {
        displayInfo = _displayInfo;
        imageIcon.SetActive(true);
        goldInfo.SetActive(true);
        imageIcon.GetComponent<Image>().sprite = displayInfo.displayIcon;
        if (displayInfo.goldValue != 0)
        {
            goldInfo.GetComponent<Text>().enabled = true;
            goldInfo.GetComponent<Text>().text = displayInfo.goldValue.ToString();
        }
        else
        {
            goldInfo.GetComponent<Text>().enabled = false;
        }
    }

    public void deleteSlot()
    {
        displayInfo = null;
        imageIcon.SetActive(false);
        goldInfo.SetActive(false);
        if (toolTip != null)
        {
            toolTip.SetActive(false);
        }
    }

    bool checkExistingGold()
    {
        while (displayInfo.goldValue > 1000 && inventory.itemList.Count < PlayerItems.maxInventorySize)
        {
            displayInfo.goldValue -= 1000;
            GameObject newGoldItem = Instantiate(itemTemplates.gold);
            newGoldItem.GetComponent<DisplayItem>().goldValue = 1000;
            inventory.itemList.Add(newGoldItem);
        }

        for (int i = inventory.itemList.Count - 1; i >= 0; i--)
        {
            if (inventory.itemList[i].GetComponent<DisplayItem>().goldValue < 1000 && inventory.itemList[i].GetComponent<DisplayItem>().goldValue > 0)
            {
                if (inventory.itemList[i].GetComponent<DisplayItem>().goldValue + displayInfo.goldValue > 1000)
                {
                    displayInfo.goldValue -= 1000 - inventory.itemList[i].GetComponent<DisplayItem>().goldValue;
                    inventory.itemList[i].GetComponent<DisplayItem>().goldValue = 1000;
                    return false;
                }
                else
                {
                    inventory.itemList[i].GetComponent<DisplayItem>().goldValue += displayInfo.goldValue;
                    return true;
                }
            }
        }
        return false;
    }

    public void transferItem()
    {
        if (displayInfo != null)
        {
            if (displayInfo.goldValue > 0)
            {
                if (checkExistingGold() != true)
                {
                    if (inventory.itemList.Count < PlayerItems.maxInventorySize)
                    {
                        inventory.itemList.Add(displayInfo.gameObject);
                    }
                }
                FindObjectOfType<AudioManager>().PlaySound("Pick Up Gold");
            }

            if (inventory.itemList.Count < PlayerItems.maxInventorySize)
            {
                int index = 0;
                for (int i = 0; i < targetChest.chestSlots.Length; i++)
                {
                    if (targetChest.chestSlots[i] == this)
                    {
                        index = i;
                    }
                }
                
                if(displayInfo.goldValue <= 0)
                {
                    inventory.itemList.Add(displayInfo.gameObject);
                    FindObjectOfType<AudioManager>().PlaySound("Receive Item");
                }
                targetChest.chestItems[index] = null;
            }
            inventory.UpdateUI();
            targetChest.UpdateUI();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (displayInfo != null)
        {
            toolTip.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (displayInfo != null)
        {
            toolTip.SetActive(true);
            toolTip.transform.position = this.transform.position;
            toolTip.GetComponentInChildren<Text>().text = displayInfo.GetComponent<Text>().text;
        }
    }
}
