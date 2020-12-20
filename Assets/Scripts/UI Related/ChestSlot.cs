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
        PlayerProperties.artifactToolTip.gameObject.SetActive(false);
    }

    void addGold(int index)
    {
        for(int i = 0; i < inventory.itemList.Count; i++)
        {
            if (inventory.itemList[i].GetComponent<DisplayItem>().goldValue < 1000 && inventory.itemList[i].GetComponent<DisplayItem>().goldValue > 0)
            {
                if (inventory.itemList[i].GetComponent<DisplayItem>().goldValue + displayInfo.goldValue > 1000)
                {
                    displayInfo.goldValue -= 1000 - inventory.itemList[i].GetComponent<DisplayItem>().goldValue;
                    inventory.itemList[i].GetComponent<DisplayItem>().goldValue = 1000;
                }
                else
                {
                    inventory.itemList[i].GetComponent<DisplayItem>().goldValue += displayInfo.goldValue;
                    displayInfo.goldValue = 0;
                    break;
                }
            }
        }

        while (displayInfo.goldValue > 1000 && inventory.itemList.Count < PlayerItems.maxInventorySize)
        {
            displayInfo.goldValue -= 1000;
            GameObject newGoldItem = Instantiate(itemTemplates.gold);
            newGoldItem.GetComponent<DisplayItem>().goldValue = 1000;
            inventory.itemList.Add(newGoldItem);
        }

        if(inventory.itemList.Count < PlayerItems.maxInventorySize && displayInfo.goldValue > 0)
        {
            GameObject newGoldItem = Instantiate(itemTemplates.gold);
            newGoldItem.GetComponent<DisplayItem>().goldValue = displayInfo.goldValue;
            inventory.itemList.Add(newGoldItem);
            displayInfo.goldValue = 0;
        }

        if(displayInfo.goldValue <= 0)
        {
            Destroy(displayInfo.gameObject);
            targetChest.chestItems[index] = null;
        }
    }

    public void transferItem()
    {
        int index = 0;
        for (int i = 0; i < targetChest.chestSlots.Length; i++)
        {
            if (targetChest.chestSlots[i] == this)
            {
                index = i;
            }
        }

        if (displayInfo != null)
        {
            if (displayInfo.goldValue > 0)
            {
                addGold(index);
                FindObjectOfType<AudioManager>().PlaySound("Pick Up Gold");
            }
            else
            {
                if (inventory.itemList.Count < PlayerItems.maxInventorySize)
                {
                    inventory.itemList.Add(displayInfo.gameObject);
                    FindObjectOfType<AudioManager>().PlaySound("Receive Item");
                    targetChest.chestItems[index] = null;
                }
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
            PlayerProperties.artifactToolTip.gameObject.SetActive(false);
            PlayerProperties.consumableToolTip.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (displayInfo != null)
        {
            if (displayInfo.isArtifact)
            {
                ArtifactBonus artifactBonus = displayInfo.GetComponent<ArtifactBonus>();
                PlayerProperties.artifactToolTip.SetTextAndPosition(
                    artifactBonus.artifactName,
                    artifactBonus.descriptionText.text,
                    artifactBonus.effectText == null ? "" : artifactBonus.effectText.text,
                    artifactBonus.attackBonus,
                    artifactBonus.speedBonus,
                    artifactBonus.healthBonus,
                    artifactBonus.defenseBonus,
                    artifactBonus.periodicHealing,
                    displayInfo.hasActive,
                    displayInfo.soulBound,
                    artifactBonus.killRequirement,
                    artifactBonus.whatRarity,
                    transform.position);
            }
            else if (displayInfo.isConsumable)
            {
                ConsumableBonus consumableBonus = displayInfo.GetComponent<ConsumableBonus>();
                PlayerProperties.consumableToolTip.SetTextAndPosition(
                    consumableBonus.consumableName,
                    consumableBonus.loreText.text,
                    consumableBonus.effectText == null ? "" : consumableBonus.effectText.text,
                    consumableBonus.attackBonus,
                    consumableBonus.speedBonus,
                    consumableBonus.defenseBonus,
                    consumableBonus.restoredHealth,
                    consumableBonus.duration,
                    transform.position);
            }
            else
            {
                PlayerProperties.toolTip.SetTextAndPosition(displayInfo.GetComponent<Text>().text, transform.position);
            }
        }
    }
}
