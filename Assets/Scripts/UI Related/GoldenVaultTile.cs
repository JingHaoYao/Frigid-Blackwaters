using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GoldenVaultTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DisplayItem displayInfo;
    public GameObject imageIcon;
    public Inventory inventory;
    public GameObject toolTip;
    public GoldenVault goldenVault;

    void Start () {
        inventory = GameObject.Find("PlayerShip").GetComponent<Inventory>();
	}

	void Update () {
		
	}

    public void addSlot(DisplayItem _displayInfo)
    {
        displayInfo = _displayInfo;
        imageIcon.SetActive(true);
        imageIcon.GetComponent<Image>().sprite = _displayInfo.displayIcon;
    }

    public void deleteSlot()
    {
        displayInfo = null;
        imageIcon.SetActive(false);
    }

    public void transferItem()
    {
        if(displayInfo != null && inventory.itemList.Count < inventory.inventorySize)
        {
            goldenVault.vaultItems.Remove(displayInfo.gameObject);
            HubProperties.vaultItems.Remove(displayInfo.gameObject.name);
            inventory.itemList.Add(displayInfo.gameObject);
            inventory.UpdateUI();
            goldenVault.UpdateUI();
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
