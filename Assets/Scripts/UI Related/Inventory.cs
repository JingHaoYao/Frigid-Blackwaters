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

    private bool inventoryEnabled = true;

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

    public void DisableInventory()
    {
        inventoryEnabled = false;
    }

    public void EnableInventory()
    {
        inventoryEnabled = true;
    }

    public void PlayInventoryEnterAnimation()
    {
        menuSlideAnimation.PlayOpeningAnimation(inventory);
    }

    public void PlayInventoryExitAnimation()
    {
        menuSlideAnimation.PlayEndingAnimation(inventory, () => { inventory.SetActive(false); consumableConfirmationWindow.gameObject.SetActive(false); });
    }

    public void OpenArtifactsAndInventory()
    {
        UpdateUI();
        inventory.SetActive(true);
        PlayInventoryEnterAnimation();
        Time.timeScale = 0;
        PlayerProperties.playerArtifacts.OpenArtifacts();
    }

    public void OpenDisenchantingAndInventory()
    {
        UpdateUI();
        inventory.SetActive(true);
        PlayInventoryEnterAnimation();
        Time.timeScale = 0;
        PlayerProperties.articraftingDisenchantingMenu.OpenDisenchantingMenu();
    }

    public void OpenCraftingAndInventory()
    {
        UpdateUI();
        inventory.SetActive(true);
        PlayInventoryEnterAnimation();
        Time.timeScale = 0;
        PlayerProperties.articraftingCraftingMenu.OpenCraftingMenu();
    }

	void LateUpdate () {
        if (chestDisplay.activeSelf == false)
        {
            if (PlayerProperties.playerScript.playerDead == false)
            {
                if (menuSlideAnimation.IsAnimating == false && inventoryEnabled)
                {
                    if (inventory.activeSelf == false && !PlayerProperties.playerScript.windowAlreadyOpen)
                    {
                        if (Input.GetKeyDown(KeyCode.I))
                        {
                            OpenArtifactsAndInventory();
                        }
                        else if(Input.GetKeyDown(KeyCode.O) && MiscData.unlockedArticrafting)
                        {
                            if (PlayerProperties.playerScript.IsInPlayerHub())
                            {
                                OpenDisenchantingAndInventory();
                            }
                        }
                        else if(Input.GetKeyDown(KeyCode.P))
                        {
                            if (PlayerProperties.playerScript.IsInPlayerHub() && MiscData.unlockedArticrafting)
                            {
                                OpenCraftingAndInventory();
                            }
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            if (toolTip.activeSelf == true)
                            {
                                toolTip.SetActive(false);
                            }
                            PlayInventoryExitAnimation();
                            if (PlayerProperties.playerScript.IsInPlayerHub() && PlayerProperties.articraftingDisenchantingMenu.IsMenuOpened())
                            {
                                PlayerProperties.articraftingDisenchantingMenu.CloseDisenchantingMenu();
                            }
                            else if (PlayerProperties.playerScript.IsInPlayerHub() && PlayerProperties.articraftingCraftingMenu.IsMenuOpened())
                            {
                                PlayerProperties.articraftingCraftingMenu.CloseCraftingMenu();
                            }
                            else if (PlayerProperties.playerArtifacts.artifactsUI.activeSelf == true)
                            {
                                PlayerProperties.playerArtifacts.CloseArtifacts();
                            }
                            Time.timeScale = 1;
                        }
                        else if(Input.GetKeyDown(KeyCode.I))
                        {
                            if (PlayerProperties.playerArtifacts.artifactsUI.activeSelf == true)
                            {
                                if (toolTip.activeSelf == true)
                                {
                                    toolTip.SetActive(false);
                                }
                                PlayInventoryExitAnimation();
                                PlayerProperties.playerArtifacts.CloseArtifacts();
                                Time.timeScale = 1;
                            }
                        }
                        else if(Input.GetKeyDown(KeyCode.O))
                        {
                            if (PlayerProperties.playerScript.IsInPlayerHub() && PlayerProperties.articraftingDisenchantingMenu.IsMenuOpened())
                            {
                                if (toolTip.activeSelf == true)
                                {
                                    toolTip.SetActive(false);
                                }
                                PlayInventoryExitAnimation();
                                PlayerProperties.articraftingDisenchantingMenu.CloseDisenchantingMenu();
                                Time.timeScale = 1;
                            }
                        }
                        else if(Input.GetKeyDown(KeyCode.P))
                        {
                            if (PlayerProperties.playerScript.IsInPlayerHub() && PlayerProperties.articraftingCraftingMenu.IsMenuOpened())
                            {
                                if (toolTip.activeSelf == true)
                                {
                                    toolTip.SetActive(false);
                                }
                                PlayInventoryExitAnimation();
                                PlayerProperties.articraftingCraftingMenu.CloseCraftingMenu();
                                Time.timeScale = 1;
                            }
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

            if(itemDisplay.isArtifact)
            {
                ArtifactBonus artifactBonus = item.GetComponent<ArtifactBonus>();
                if(!PlayerItems.pastArtifacts.ContainsKey(artifactBonus.whatDungeonArtifact))
                {
                    PlayerItems.pastArtifacts.Add(artifactBonus.whatDungeonArtifact, new List<string>());
                }

                if (!PlayerItems.pastArtifacts[artifactBonus.whatDungeonArtifact].Contains(item.name.Replace("(Clone)", "").Trim()))
                {
                    PlayerItems.pastArtifacts[artifactBonus.whatDungeonArtifact].Add(item.name.Replace("(Clone)", "").Trim());
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
