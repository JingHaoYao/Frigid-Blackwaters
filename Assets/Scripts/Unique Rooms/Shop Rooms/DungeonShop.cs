using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonShop : MonoBehaviour
{
    Inventory inventory;
    public ShopTilesUI shopTilesUI;
    public GameObject inventoryDisplay;
    GameObject shopDisplay;
    List<GameObject> sellingItems = new List<GameObject>();
    List<int> sellingItemsPrices = new List<int>();
    public GameObject examineIndicator;
    GameObject playerShip, spawnedIndicator;
    Text text;
    int numberItems = 0;
    ItemTemplates itemTemplates;
    GameObject toolTip;
    public int shopTier = 1;
    int whatDungeonLevel = 0;

    MenuSlideAnimation menuSlideAnimation = new MenuSlideAnimation();

    void SetAnimation()
    {
        menuSlideAnimation.SetOpenAnimation(new Vector3(-243, -585, 0), new Vector3(-243, 0, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(-243, 0, 0), new Vector3(-243, -585, 0), 0.25f);
    }

    void Start()
    {
        inventory = GameObject.Find("PlayerShip").GetComponent<Inventory>();
        shopTilesUI = inventory.shopDisplay.GetComponentInChildren<ShopTilesUI>();
        inventoryDisplay = inventory.inventory;
        shopDisplay = inventory.shopDisplay;
        text = GetComponent<Text>();
        playerShip = GameObject.Find("PlayerShip");
        toolTip = playerShip.GetComponent<PlayerScript>().obstacleToolTip;
        itemTemplates = GameObject.Find("ItemTemplates").GetComponent<ItemTemplates>();
        toolTip = inventory.toolTip;
        whatDungeonLevel = FindObjectOfType<DungeonEntryDialogueManager>().whatDungeonLevel;
        spawnSellingItems();
        SetAnimation();
    }

    void spawnSellingItems()
    {
        numberItems = Random.Range(1, 4) + shopTier;
        for (int i = 0; i < numberItems; i++)
        {
            int ifConsumable = Random.Range(1, 13 - shopTier * 2);
            if (ifConsumable <= (13 - shopTier * 2) - 5)
            {
                GameObject newItem = itemTemplates.loadRandomItem(5);
                newItem.transform.parent = GameObject.Find("PresentItems").transform;
                sellingItems.Add(newItem);
                sellingItemsPrices.Add(newItem.GetComponent<ConsumableBonus>().priceBase + Random.Range(0, 2) * 25);
            }
            else
            {
                if(shopTier == 1)
                {
                    int percentItem = Random.Range(1, 101);
                    if(percentItem <= 75)
                    {
                        GameObject newItem = itemTemplates.loadRandomItem(1);
                        newItem.transform.parent = GameObject.Find("PresentItems").transform;
                        sellingItems.Add(newItem);
                        sellingItemsPrices.Add(250 + Random.Range(0, 5) * 25 + (whatDungeonLevel - 1) * 750);
                    }
                    else
                    {
                        GameObject newItem = itemTemplates.loadRandomItem(2);
                        newItem.transform.parent = GameObject.Find("PresentItems").transform;
                        sellingItems.Add(newItem);
                        sellingItemsPrices.Add(350 + Random.Range(0, 5) * 50 + (whatDungeonLevel - 1) * 750);
                    }

                }
                else if(shopTier == 2)
                {
                    int percentItem = Random.Range(1, 101);
                    if (percentItem <= 50)
                    {
                        GameObject newItem = itemTemplates.loadRandomItem(1);
                        newItem.transform.parent = GameObject.Find("PresentItems").transform;
                        sellingItems.Add(newItem);
                        sellingItemsPrices.Add(150 + Random.Range(0, 5) * 25 + (whatDungeonLevel - 1) * 750);
                    }
                    else if(percentItem > 50 && percentItem <= 80)
                    {
                        GameObject newItem = itemTemplates.loadRandomItem(2);
                        newItem.transform.parent = GameObject.Find("PresentItems").transform;
                        sellingItems.Add(newItem);
                        sellingItemsPrices.Add(250 + Random.Range(0, 5) * 50 + (whatDungeonLevel - 1) * 750);
                    }
                    else
                    {
                        GameObject newItem = itemTemplates.loadRandomItem(3);
                        newItem.transform.parent = GameObject.Find("PresentItems").transform;
                        sellingItems.Add(newItem);
                        sellingItemsPrices.Add(400 + Random.Range(0, 5) * 50 + (whatDungeonLevel - 1) * 750);
                    }
                }
                else
                {
                    int percentItem = Random.Range(1, 101);
                    if (percentItem <= 50)
                    {
                        GameObject newItem = itemTemplates.loadRandomItem(2);
                        newItem.transform.parent = GameObject.Find("PresentItems").transform;
                        sellingItems.Add(newItem);
                        sellingItemsPrices.Add(250 + Random.Range(0, 5) * 25 + (whatDungeonLevel - 1) * 750);
                    }
                    else if (percentItem > 50 && percentItem <= 80)
                    {
                        GameObject newItem = itemTemplates.loadRandomItem(3);
                        newItem.transform.parent = GameObject.Find("PresentItems").transform;
                        sellingItems.Add(newItem);
                        sellingItemsPrices.Add(300 + Random.Range(0, 5) * 50 + (whatDungeonLevel - 1) * 750);
                    }
                    else
                    {
                        GameObject newItem = itemTemplates.loadRandomItem(4);
                        newItem.transform.parent = GameObject.Find("PresentItems").transform;
                        sellingItems.Add(newItem);
                        sellingItemsPrices.Add(600 + Random.Range(0, 5) * 50 + (whatDungeonLevel - 1) * 750);
                    }
                }
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
        Time.timeScale = 0;
        inventory.UpdateUI();
    }

    void LateUpdate()
    {
        if (Vector2.Distance(playerShip.transform.position, transform.position) < 5f && playerShip.GetComponent<PlayerScript>().enemiesDefeated == true)
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
                        menuSlideAnimation.PlayEndingAnimation(shopDisplay, () => { shopDisplay.SetActive(false); playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = false; });
                        PlayerProperties.playerInventory.PlayInventoryExitAnimation();
                        playerShip.GetComponent<PlayerScript>().removeRootingObject();
                        Time.timeScale = 1;
                    }
                }
                else
                {

                    if (playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            shopDisplay.SetActive(true);
                            menuSlideAnimation.PlayOpeningAnimation(shopDisplay);
                            PlayerProperties.playerInventory.PlayInventoryEnterAnimation();
                            inventoryDisplay.SetActive(true);
                            playerShip.GetComponent<Inventory>().UpdateUI();
                            playerShip.GetComponent<PlayerScript>().addRootingObject();
                            setShopDisplay();
                            playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = true;
                        }
                    }
                    else
                    {
                        if (spawnedIndicator != null)
                        {
                            Destroy(spawnedIndicator);
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
    }
}

