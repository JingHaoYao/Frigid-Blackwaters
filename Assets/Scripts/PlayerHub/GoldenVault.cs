using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldenVault : MonoBehaviour {
    public List<GameObject> vaultItems = new List<GameObject>();
    ItemTemplates itemTemplates;
    public GoldenVaultTile[] vaultTiles;
    GameObject playerShip, spawnedIndicator;
    public GameObject vaultDisplay, inventoryDisplay, examineIndicator, goldStoredIndicator;
    public GameObject icon;

    void loadPrevItems()
    {
        foreach (string item in HubProperties.vaultItems)
        {
            if (item != null)
            {
                GameObject newItem = Instantiate(itemTemplates.loadItem(item));
                newItem.transform.parent = GameObject.Find("PresentItems").transform; //bookkeeping
                vaultItems.Add(newItem);
            }
        }
        goldStoredIndicator.GetComponentInChildren<Text>().text = HubProperties.storeGold.ToString();
    }

    public void UpdateUI()
    {
        goldStoredIndicator.GetComponentInChildren<Text>().text = HubProperties.storeGold.ToString();
        HubProperties.vaultItems.Clear();
        if (vaultItems.Count > 0)
        {
            for (int i = 0; i < vaultTiles.Length; i++)
            {
                if (i < vaultItems.Count)
                {
                    vaultTiles[i].addSlot(vaultItems[i].GetComponent<DisplayItem>());
                    HubProperties.vaultItems.Add(vaultItems[i].name);
                }
                else
                {
                    vaultTiles[i].deleteSlot();
                }
            }
        }
        else
        {
            for (int i = 0; i < vaultTiles.Length; i++)
            {
                vaultTiles[i].deleteSlot(); // error handling
            }
        }

        SaveSystem.SaveGame();
    }

    void Start () {
        itemTemplates = FindObjectOfType<ItemTemplates>();
        playerShip = GameObject.Find("PlayerShip");
        loadPrevItems();
	}

	void LateUpdate () {
        if (Vector2.Distance(playerShip.transform.position, transform.position) < 5f && MiscData.unlockedBuildings.Contains("golden_vault"))
        {
            if (vaultDisplay.activeSelf == false)
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

            if (vaultDisplay.activeSelf == true)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    inventoryDisplay.SetActive(false);
                    playerShip.GetComponent<PlayerScript>().shipRooted = false;
                    playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = false;
                    vaultDisplay.SetActive(false);
                }
            }
            else if(playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    vaultDisplay.SetActive(true);
                    UpdateUI();
                    inventoryDisplay.SetActive(true);
                    playerShip.GetComponent<Inventory>().UpdateUI();
                    playerShip.GetComponent<PlayerScript>().shipRooted = true;
                    playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = true;
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

        if (!MiscData.unlockedBuildings.Contains("golden_vault"))
        {
            icon.SetActive(false);
        }
        else
        {
            icon.SetActive(true);
        }
    }
}
