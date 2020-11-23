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

    public Text pageNumberText;
    public Text purchaseText;
    public Button forwardButton;
    public Button backwardButton;

    int whatPage = 0;

    MenuSlideAnimation menuSlideAnimation = new MenuSlideAnimation();

    void SetAnimation()
    {
        menuSlideAnimation.SetOpenAnimation(new Vector3(-243, -585, 0), new Vector3(-243, 22, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(-243, 22, 0), new Vector3(-243, -585, 0), 0.25f);
    }

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
        goldStoredIndicator.GetComponentInChildren<Text>().text = pickDisplay(HubProperties.storeGold);
    }

    string pickDisplay(int goldAmount)
    {
        if (goldAmount < 1000)
        {
            return goldAmount.ToString();
        }
        else if (goldAmount < 100000)
        {
            string goldToDisplay = ((float)goldAmount / 1000).ToString();
            return goldToDisplay.Substring(0, Mathf.Clamp(goldToDisplay.Length, 0, 4)) + "K";
        }
        else if (goldAmount < 1000000)
        {
            string goldToDisplay = ((float)goldAmount / 1000).ToString();
            return goldToDisplay.Substring(0, Mathf.Clamp(goldToDisplay.Length, 0, 3)) + "K";
        }
        else if (goldAmount < 10000000)
        {
            string goldToDisplay = ((float)goldAmount / 1000000).ToString();
            return goldToDisplay.Substring(0, Mathf.Clamp(goldToDisplay.Length, 0, 3)) + "M";
        }
        else
        {
            string goldToDisplay = ((float)goldAmount / 1000000).ToString();
            return goldToDisplay.Substring(0, Mathf.Clamp(goldToDisplay.Length, 0, 4)) + "M";
        }
    }

    private int price()
    {
        return 1000 * HubProperties.maxNumberVaultItems / 8;
    }

    public void UpdateUI()
    {
        goldStoredIndicator.GetComponentInChildren<Text>().text = pickDisplay(HubProperties.storeGold);

        purchaseText.text = "$" + price().ToString();

        if (vaultItems.Count > 0 && vaultItems.Count - whatPage * 8 > 0)
        {
            for (int i = 0; i < vaultTiles.Length; i++)
            {
                if (i < vaultItems.Count - (8 * whatPage))
                {
                    vaultTiles[i].addSlot(vaultItems[i + 8 * whatPage].GetComponent<DisplayItem>());
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

        if (whatPage == 0)
        {
            backwardButton.enabled = false;
            backwardButton.targetGraphic.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        else
        {
            backwardButton.enabled = true;
            backwardButton.targetGraphic.color = Color.white;
        }

        if ((whatPage + 1) * 8 == HubProperties.maxNumberVaultItems)
        {
            forwardButton.enabled = false;
            forwardButton.targetGraphic.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        else
        {
            forwardButton.enabled = true;
            forwardButton.targetGraphic.color = Color.white;
        }
        


        SaveSystem.SaveGame();
    }

    public void purchaseSlots()
    {
        if (HubProperties.storeGold > price())
        {
            FindObjectOfType<AudioManager>().PlaySound("Purchase Skill Point");
            HubProperties.storeGold -= price();
            HubProperties.maxNumberVaultItems += 8;
            UpdateUI();
        }
    }

    public void AdvanceForwardPage()
    {
        if((whatPage + 1) * 8 < HubProperties.maxNumberVaultItems)
        {
            whatPage++;
            pageNumberText.text = "Page " + (whatPage + 1);
            UpdateUI();
            FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
        }
    }

    public void AdvanceBackwardsPage()
    {
        if(whatPage - 1 >= 0)
        {
            whatPage--;
            pageNumberText.text = "Page " + (whatPage + 1);
            UpdateUI();
            FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
        }
    }

    void Start () {
        itemTemplates = FindObjectOfType<ItemTemplates>();
        playerShip = GameObject.Find("PlayerShip");
        loadPrevItems();
        SetAnimation();
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

            if (menuSlideAnimation.IsAnimating == false)
            {
                if (vaultDisplay.activeSelf == true)
                {
                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.E))
                    {
                        PlayerProperties.playerInventory.PlayInventoryExitAnimation();
                        PlayerProperties.playerScript.removeRootingObject();
                        menuSlideAnimation.PlayEndingAnimation(vaultDisplay, () => { vaultDisplay.SetActive(false); playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = false; });
                    }
                }
                else if (playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        vaultDisplay.SetActive(true);
                        menuSlideAnimation.PlayOpeningAnimation(vaultDisplay);
                        UpdateUI();
                        inventoryDisplay.SetActive(true);
                        PlayerProperties.playerInventory.PlayInventoryEnterAnimation();
                        playerShip.GetComponent<Inventory>().UpdateUI();
                        PlayerProperties.playerScript.addRootingObject();
                        playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = true;
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
