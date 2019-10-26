using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public GameObject inventory;
    public int inventorySize = 10;
    public InventorySlot[] inventorySlots;
    public List<GameObject> itemList = new List<GameObject>();
    public Transform itemSlotParent;
    public GameObject chestDisplay;
    public GameObject toolTip;
    public GameObject shopDisplay;
    public GameObject vaultDisplay;

	void Awake () {
        inventorySlots = itemSlotParent.GetComponentsInChildren<InventorySlot>();
        inventorySize = PlayerItems.maxInventorySize;
    }

	void LateUpdate () {
        if (chestDisplay.activeSelf == false)
        {
            if (GetComponent<PlayerScript>().playerDead == false)
            {
                if (inventory.activeSelf == false)
                {
                    if (Input.GetKeyDown(KeyCode.I) && (GetComponent<PlayerScript>().windowAlreadyOpen == false || this.GetComponent<Artifacts>().artifactsUI.activeSelf == true))
                    {
                        UpdateUI();
                        inventory.SetActive(true);
                        Time.timeScale = 0;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.I))
                    {
                        if (toolTip.activeSelf == true)
                        {
                            toolTip.SetActive(false);
                        }
                        inventory.SetActive(false);
                        Time.timeScale = 1;
                    }
                }
            }
        }
	}

    public int tallyGold()
    {
        int totalGold = 0;
        if (itemList.Count > 0)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                totalGold += itemList[i].GetComponent<DisplayItem>().goldValue;
            }
        }
        return totalGold;
    }

    public void UpdateUI()
    {
        PlayerItems.inventoryItemsIDs.Clear();
        PlayerItems.totalGoldAmount = tallyGold();
        if (itemList.Count > 0)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                if (i < itemList.Count)
                {
                    inventorySlots[i].addSlot(itemList[i].GetComponent<DisplayItem>());
                    PlayerItems.inventoryItemsIDs.Add(itemList[i].name);
                }
                else
                {
                    inventorySlots[i].deleteSlot();
                }
                inventorySlots[i].unlockSlot();
            }

            for(int i = inventorySize; i < 25; i++)
            {
                inventorySlots[i].lockSlot();
            }
        }
        else
        {
            for (int i = 0; i < inventorySize; i++)
            {
                inventorySlots[i].deleteSlot(); // error handling
                inventorySlots[i].unlockSlot();
            }

            for (int i = inventorySize; i < 25; i++)
            {
                inventorySlots[i].lockSlot();
            }
        }
        if (GameObject.Find("QuestManager"))
        {
            GameObject.Find("QuestManager").GetComponent<QuestManager>().addItemCollect(itemList);
        }

        foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
        {
            if (slot.displayInfo != null)
                slot.displayInfo.GetComponent<ArtifactBonus>().updatedInventory = true;
        }

        SaveSystem.SaveGame();
    }
}
