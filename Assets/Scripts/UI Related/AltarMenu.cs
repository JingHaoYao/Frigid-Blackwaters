using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AltarMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DisplayItem displayInfo;
    public GameObject toolTip;
    public Text healthSacrificeText;
    int healthSacrifice;

    public Image imageIcon;
    Inventory inventory;
    PlayerScript playerScript;

    public GameObject particles;
    public DungeonAltar altar; 
    
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        playerScript = FindObjectOfType<PlayerScript>();
    }

    public void addSlot(DisplayItem _displayInfo, int _healthSacrifice)
    {
        displayInfo = _displayInfo;
        imageIcon.sprite = displayInfo.displayIcon;
        healthSacrifice = _healthSacrifice;
        healthSacrificeText.text = _healthSacrifice.ToString();
    }

    public void transferItem()
    {
        if (displayInfo != null && inventory.itemList.Count < inventory.inventorySize)
        {
            if (inventory.itemList.Count < inventory.inventorySlots.Length)
            {
                inventory.itemList.Add(displayInfo.gameObject);
                FindObjectOfType<AudioManager>().PlaySound("Receive Item");
                playerScript.trueDamage += healthSacrifice; //generate sacrificed health
                altar.setUnActive();
                Instantiate(particles, playerScript.gameObject.transform.position, Quaternion.identity);
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
