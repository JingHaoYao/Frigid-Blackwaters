using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkMagicDealer : MonoBehaviour {
    Inventory inventory;
    public ShopTilesUI shopTilesUI;
    GameObject shopDisplay;
    List<GameObject> sellingItems = new List<GameObject>();
    List<int> sellingItemsPrices = new List<int>();
    public GameObject examineIndicator, yesIndicator, noIndicator;
    bool toolTipActive = false;
    GameObject playerShip, spawnedIndicator, obstacleToolTip, spawnedYI, spawnedNI;
    Text text;
    int numberItems = 0;
    ItemTemplates itemTemplates;
    GameObject toolTip;

    void Start()
    {
        inventory = GameObject.Find("PlayerShip").GetComponent<Inventory>();
        shopTilesUI = inventory.shopDisplay.GetComponentInChildren<ShopTilesUI>();
        shopDisplay = inventory.shopDisplay;
        text = GetComponent<Text>();
        playerShip = GameObject.Find("PlayerShip");
        obstacleToolTip = playerShip.GetComponent<PlayerScript>().obstacleToolTip;
        itemTemplates = GameObject.Find("ItemTemplates").GetComponent<ItemTemplates>();
        spawnSellingItems();
        toolTip = inventory.toolTip;
    }

    void spawnSellingItems()
    {
        numberItems = Random.Range(2, 4);
        for (int i = 0; i < numberItems; i++)
        {
            GameObject newItem = itemTemplates.loadRandomItem(2);
            newItem.transform.parent = GameObject.Find("PresentItems").transform;
            sellingItems.Add(newItem);
            sellingItemsPrices.Add(newItem.GetComponent<ConsumableBonus>().priceBase + Random.Range(0, 4) * 25);
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
                if (toolTipActive == false)
                {
                    if (spawnedIndicator == null)
                    {
                        spawnedIndicator = Instantiate(examineIndicator, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                        spawnedIndicator.GetComponent<ExamineIndicator>().parentObject = this.gameObject;
                        Destroy(spawnedYI);
                        Destroy(spawnedNI);
                    }
                }
                else
                {
                    if (spawnedIndicator != null)
                    {
                        Destroy(spawnedIndicator);
                        spawnedYI = Instantiate(yesIndicator, transform.position + new Vector3(-1, 1, 0), Quaternion.identity);
                        spawnedNI = Instantiate(noIndicator, transform.position + new Vector3(1, 1, 0), Quaternion.identity);
                    }

                    if (Input.GetKeyDown(KeyCode.Z) && playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
                    {
                        playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = true;
                        obstacleToolTip.SetActive(false);
                        toolTipActive = false;
                        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = false;
                        Destroy(spawnedYI);
                        Destroy(spawnedNI);
                        //do shop thing
                        setShopDisplay();
                    }

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        obstacleToolTip.SetActive(false);
                        toolTipActive = false;
                        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = false;
                        Destroy(spawnedYI);
                        Destroy(spawnedNI);
                    }
                }

                if (Input.GetKeyDown(KeyCode.E) && playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
                {
                    if (obstacleToolTip.activeSelf == true)
                    {
                        obstacleToolTip.SetActive(false);
                        toolTipActive = false;
                        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = false;
                    }
                    else
                    {
                        toolTipActive = true;
                        obstacleToolTip.GetComponentInChildren<Text>().text = text.text;
                        obstacleToolTip.SetActive(true);
                        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = true;
                    }
                }
            }

            if (shopDisplay.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
            {
                if(toolTip.activeSelf == true)
                {
                    toolTip.SetActive(false);
                }
                playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = false;
                shopDisplay.SetActive(false);
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
