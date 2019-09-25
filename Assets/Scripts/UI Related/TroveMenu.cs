using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TroveMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DisplayItem displayInfo;
    public GameObject toolTip;

    public Image imageIcon;
    Inventory inventory;
    PlayerScript playerScript;
    public DungeonTrove trove;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        playerScript = FindObjectOfType<PlayerScript>();
    }

    public void addSlot(DisplayItem _displayInfo)
    {
        displayInfo = _displayInfo;
        imageIcon.sprite = displayInfo.displayIcon;
    }

    public void transferItem()
    {
        if (displayInfo != null && inventory.itemList.Count < inventory.inventorySize)
        {
            if (inventory.itemList.Count < inventory.inventorySlots.Length)
            {
                inventory.itemList.Add(displayInfo.gameObject);
                FindObjectOfType<AudioManager>().PlaySound("Receive Item");
                //set trove unactive
                trove.setUnActive();
                transform.parent.gameObject.SetActive(false);
                playerScript.windowAlreadyOpen = false;
                Time.timeScale = 1;
                playerScript.shipRooted = false;
            }
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
