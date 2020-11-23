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
    public ConsumableConfirm consumableConfirmationWindow;

    MenuSlideAnimation menuSlideAnimation = new MenuSlideAnimation();

    void SetInventoryAnimation()
    {
        menuSlideAnimation.SetOpenAnimation(new Vector3(97, -585, 0), new Vector3(97, 0, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(97, 0, 0), new Vector3(97, -585, 0), 0.25f);
    }

	void Awake () {
        inventorySlots = itemSlotParent.GetComponentsInChildren<InventorySlot>();
        inventorySize = PlayerItems.maxInventorySize;
        PlayerProperties.playerInventory = this;
        SetInventoryAnimation();
    }

    public void PlayInventoryEnterAnimation()
    {
        menuSlideAnimation.PlayOpeningAnimation(inventory);
    }

    public void PlayInventoryExitAnimation()
    {
        menuSlideAnimation.PlayEndingAnimation(inventory, () => { inventory.SetActive(false); });
    }

	void LateUpdate () {
        if (chestDisplay.activeSelf == false)
        {
            if (GetComponent<PlayerScript>().playerDead == false)
            {
                if (menuSlideAnimation.IsAnimating == false)
                {
                    if (inventory.activeSelf == false)
                    {
                        if (Input.GetKeyDown(KeyCode.I) && (GetComponent<PlayerScript>().windowAlreadyOpen == false || this.GetComponent<Artifacts>().artifactsUI.activeSelf == true))
                        {
                            UpdateUI();
                            inventory.SetActive(true);
                            PlayInventoryEnterAnimation();
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
                            PlayInventoryExitAnimation();
                            Time.timeScale = 1;
                        }
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
        int totalGoldAmount = PlayerItems.totalGoldAmount;

        List<GameObject> itemsToRemove = new List<GameObject>();

        foreach(GameObject item in itemList)
        {
            DisplayItem itemDisplay = item.GetComponent<DisplayItem>();
            if (itemDisplay.goldValue > 0)
            {
                if (totalGoldAmount > 0)
                {
                    if (totalGoldAmount > 1000)
                    {
                        totalGoldAmount -= 1000;
                        itemDisplay.goldValue = 1000;

                    }
                    else
                    {
                        itemDisplay.goldValue = totalGoldAmount;
                        totalGoldAmount = 0;
                    }
                }
                else
                {
                    itemsToRemove.Add(item);
                }
            }
        }

        foreach(GameObject item in itemsToRemove)
        {
            itemList.Remove(item);
            Destroy(item);
        }

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
                inventorySlots[i].deleteSlot();
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

        foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
        {
            if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                slot.displayInfo.GetComponent<ArtifactEffect>().updatedInventory();
        }
        SaveSystem.SaveGame();
    }
}
