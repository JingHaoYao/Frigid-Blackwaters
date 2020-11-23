using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubShop : MonoBehaviour {
    Inventory inventory;
    public ShopTilesUI shopTilesUI;
    public GameObject shopDisplay;
    List<GameObject> sellingItems = new List<GameObject>();
    List<int> sellingItemsPrices = new List<int>();
    public GameObject examineIndicator;
    public GameObject inventoryDisplay;
    GameObject playerShip, spawnedIndicator;
    int numberItems = 0;
    GameObject toolTip;
    public List<GameObject> sellingItemsList = new List<GameObject>();
    public int startingChance, endingChance;
    public int priceMultiplier = 0;
    public DialogueSet loadedDialogue;
    public DialogueUI dialogueUI;
    public GameObject dialogueBlackOverlay;
    public GameObject dialogueIndicator;
    public string buildingID;
    public GameObject icon;
    ItemTemplates itemTemplates;

    MenuSlideAnimation menuSlideAnimation = new MenuSlideAnimation();

    void SetAnimation()
    {
        menuSlideAnimation.SetOpenAnimation(new Vector3(-243, -585, 0), new Vector3(-243, 22, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(-243, 22, 0), new Vector3(-243, -585, 0), 0.25f);
    }

    void Start()
    {
        itemTemplates = FindObjectOfType<ItemTemplates>();
        inventory = GameObject.Find("PlayerShip").GetComponent<Inventory>();
        shopTilesUI = shopDisplay.GetComponentInChildren<ShopTilesUI>();
        playerShip = GameObject.Find("PlayerShip");
        if(priceMultiplier == 0)
        {
            setSellingItemsList(false);
        }
        else
        {
            setSellingItemsList(true);
        }
        toolTip = inventory.toolTip;
        spawnSellingItems();
        SetAnimation();
    }

    void setSellingItemsList(bool artifactShop)
    {
        sellingItemsList.Clear();
        AvailableShopItems shopItems = FindObjectOfType<AvailableShopItems>();
        numberItems = Random.Range(startingChance, endingChance);

        switch(MiscData.dungeonLevelUnlocked)
        {
            case 1:
                for (int i = 0; i < numberItems; i++)
                {
                    if (artifactShop == true)
                    {
                        sellingItemsList.Add(itemTemplates.loadItem(shopItems.firstLevelArtifacts[Random.Range(0, shopItems.firstLevelArtifacts.Length)]));
                    }
                    else
                    {
                        sellingItemsList.Add(itemTemplates.loadItem(shopItems.firstLevelConsumables[Random.Range(0, shopItems.firstLevelConsumables.Length)]));
                    }
                }
                break;
            case 2:
                for (int i = 0; i < numberItems; i++)
                {
                    if (artifactShop == true)
                    {
                        if (Random.Range(0, 10) < 7)
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.secondLevelArtifacts[Random.Range(0, shopItems.secondLevelArtifacts.Length)]));
                        }
                        else
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.firstLevelArtifacts[Random.Range(0, shopItems.firstLevelArtifacts.Length)]));
                        }
                    }
                    else
                    {
                        if (Random.Range(0, 10) < 7)
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.secondLevelConsumables[Random.Range(0, shopItems.secondLevelConsumables.Length)]));
                        }
                        else
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.firstLevelConsumables[Random.Range(0, shopItems.firstLevelConsumables.Length)]));
                        }
                    }
                }
                break;
            case 3:
                for (int i = 0; i < numberItems; i++)
                {
                    if (artifactShop == true)
                    {
                        if (Random.Range(0, 10) < 7)
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.thirdLevelArtifacts[Random.Range(0, shopItems.thirdLevelArtifacts.Length)]));
                        }
                        else
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.secondLevelArtifacts[Random.Range(0, shopItems.secondLevelArtifacts.Length)]));
                        }
                    }
                    else
                    {
                        if (Random.Range(0, 10) < 7)
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.thirdLevelConsumables[Random.Range(0, shopItems.thirdLevelConsumables.Length)]));
                        }
                        else
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.secondLevelConsumables[Random.Range(0, shopItems.secondLevelConsumables.Length)]));
                        }
                    }
                }
                break;
            case 4:
                for (int i = 0; i < numberItems; i++)
                {
                    if (artifactShop == true)
                    {
                        if (Random.Range(0, 10) < 7)
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.fourthLevelArtifacts[Random.Range(0, shopItems.fourthLevelArtifacts.Length)]));
                        }
                        else
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.thirdLevelArtifacts[Random.Range(0, shopItems.thirdLevelArtifacts.Length)]));
                        }
                    }
                    else
                    {
                        if (Random.Range(0, 10) < 7)
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.fourthLevelConsumables[Random.Range(0, shopItems.fourthLevelConsumables.Length)]));
                        }
                        else
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.thirdLevelConsumables[Random.Range(0, shopItems.thirdLevelConsumables.Length)]));
                        }
                    }
                }
                break;
            case 5:
                for (int i = 0; i < numberItems; i++)
                {
                    if (artifactShop == true)
                    {
                        if (Random.Range(0, 10) < 7)
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.fifthLevelArtifacts[Random.Range(0, shopItems.fifthLevelArtifacts.Length)]));
                        }
                        else
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.fourthLevelArtifacts[Random.Range(0, shopItems.fourthLevelArtifacts.Length)]));
                        }
                    }
                    else
                    {
                        if (Random.Range(0, 10) < 7)
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.fifthLevelConsumables[Random.Range(0, shopItems.fifthLevelConsumables.Length)]));
                        }
                        else
                        {
                            sellingItemsList.Add(itemTemplates.loadItem(shopItems.fourthLevelConsumables[Random.Range(0, shopItems.fourthLevelConsumables.Length)]));
                        }
                    }
                }
                break;
        }
    }

    void spawnSellingItems()
    {
        foreach(GameObject item in sellingItemsList)
        {
            GameObject newItem = Instantiate(item);
            newItem.transform.parent = GameObject.Find("PresentItems").transform;
            sellingItems.Add(newItem);
            if (priceMultiplier == 0)
            {
                sellingItemsPrices.Add(newItem.GetComponent<ConsumableBonus>().priceBase + Random.Range(0, 3) * 50);
            }
            else
            {
                sellingItemsPrices.Add(priceMultiplier * Random.Range(3, 9) + (newItem.GetComponent<ArtifactBonus>().whatDungeonArtifact - 1) * 600);
            }
        }
    }

    void setShopDisplay()
    {
        shopDisplay.SetActive(true);
        shopTilesUI.shopItemList = sellingItems;
        shopTilesUI.prices = sellingItemsPrices;
        shopTilesUI.updateUI();
        inventory.inventory.SetActive(true);
        inventory.PlayInventoryEnterAnimation();
        Time.timeScale = 0;
        inventory.UpdateUI();
    }

    void LateUpdate()
    {
        if (Vector2.Distance(playerShip.transform.position, transform.position) < 5f && MiscData.unlockedBuildings.Contains(buildingID))
        {
            if (shopDisplay.activeSelf == false)
            {
                if (spawnedIndicator == null)
                {
                    spawnedIndicator = Instantiate(examineIndicator, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                    spawnedIndicator.GetComponent<ExamineIndicator>().parentObject = this.gameObject;
                }
            }
            else
            {
                if (spawnedIndicator != null)
                {
                    Destroy(spawnedIndicator);
                }
            }

            if (menuSlideAnimation.IsAnimating == false)
            {
                if (shopDisplay.activeSelf == true)
                {
                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.I))
                    {
                        inventory.PlayInventoryExitAnimation();
                        menuSlideAnimation.PlayEndingAnimation(shopDisplay, () => { shopDisplay.SetActive(false); PlayerProperties.playerScript.windowAlreadyOpen = false; });
                        PlayerProperties.playerScript.removeRootingObject();
                        Time.timeScale = 1;
                    }
                }
                else if (playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (loadedDialogue == null)
                        {
                            shopDisplay.SetActive(true);
                            menuSlideAnimation.PlayOpeningAnimation(shopDisplay);
                            inventoryDisplay.SetActive(true);
                            playerShip.GetComponent<Inventory>().UpdateUI();
                            PlayerProperties.playerScript.addRootingObject();
                            setShopDisplay();
                            playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = true;
                        }
                        else
                        {
                            dialogueIndicator.SetActive(false);
                            dialogueUI.targetDialogue = loadedDialogue;
                            dialogueUI.gameObject.SetActive(true);
                            dialogueBlackOverlay.SetActive(true);
                            HubShop[] hubShops = FindObjectsOfType<HubShop>();
                            loadedDialogue = null;
                            foreach (HubShop shop in hubShops)
                            {
                                shop.loadedDialogue = null;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (spawnedIndicator != null)
            {
                Destroy(spawnedIndicator);
            }
        }

        if (!MiscData.unlockedBuildings.Contains(buildingID))
        {
            icon.SetActive(false);
        }
        else
        {
            icon.SetActive(true);
        }
    }
}
