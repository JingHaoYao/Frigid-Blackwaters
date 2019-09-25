using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TreasureMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DisplayItem displayInfo;
    public GameObject toolTip;

    public Image imageIcon;
    Inventory inventory;
    PlayerScript playerScript;
    public DungeonTreasure treasure;
    //need treasure object

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        playerScript = FindObjectOfType<PlayerScript>();
    }

    public void addSlot(DisplayItem _displayInfo)
    {
        displayInfo = _displayInfo;
        imageIcon.sprite = displayInfo.displayIcon;
        this.GetComponentInChildren<Text>().text = _displayInfo.goldValue.ToString();
    }

    public void transferItem()
    {
        if (displayInfo != null)
        {
            if (checkExistingGold() != 2)
            {
                if (inventory.itemList.Count < PlayerItems.maxInventorySize)
                {
                    inventory.itemList.Add(displayInfo.gameObject);
                }
                FindObjectOfType<AudioManager>().PlaySound("Pick Up Gold");
                transform.parent.gameObject.SetActive(false);
                playerScript.windowAlreadyOpen = false;
                treasure.setUnActive();
                Time.timeScale = 1;
                playerScript.shipRooted = false;
            }
        }
    }

    int checkExistingGold()
    {
        for (int i = inventory.itemList.Count - 1; i >= 0; i--)
        {
            if (inventory.itemList[i].GetComponent<DisplayItem>().goldValue != 0 && inventory.itemList[i].GetComponent<DisplayItem>().goldValue < 1000)
            {
                if (inventory.itemList[i].GetComponent<DisplayItem>().goldValue + displayInfo.goldValue > 1000)
                {
                    displayInfo.goldValue -= 1000 - inventory.itemList[i].GetComponent<DisplayItem>().goldValue;
                    inventory.itemList[i].GetComponent<DisplayItem>().goldValue = 1000;
                    return 0;
                }
                else
                {
                    inventory.itemList[i].GetComponent<DisplayItem>().goldValue += displayInfo.goldValue;
                    return 1;
                }
            }
        }
        //did nothing
        return 2;
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
