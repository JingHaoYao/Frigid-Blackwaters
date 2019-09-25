using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    ItemTemplates itemTemplates;
    public GameObject indicator;
    private bool beenOpened = false;
    GameObject playerShip;
    GameObject spawnedIndicator;
    public GameObject chestDisplay, inventoryDisplay;
    bool displayOn = false;
    public GameObject[] chestItems;
    public ChestSlot[] chestSlots;
    GameObject presentItems;
    Animator animator;
    int typeChest = 0;
    public static int bonusArtifactChance, bonusGold;
    public bool uniqueChest = false;
    public GameObject[] uniqueItems;
    public int numUniqueItems;
    public int extraArtifactChance = 0;
    public int betterArtifactChance = 0;
    int chestVariation = 0;
    public bool dontRunAnim = false;
    public int customGoldBase = 0;
    public int customGoldMultiplier = 0;
    GameObject toolTip;

    void selectAnim(int typeChest)
    {
        if (dontRunAnim == false)
        {
            if (uniqueChest == false)
            {
                if (beenOpened == false)
                {   
                    if (typeChest == 1)
                    {
                        chestVariation = Random.Range(0, 2);
                        if (chestVariation == 1)
                        {
                            animator.SetTrigger("2Closed");
                        }
                        else
                        {
                            animator.SetTrigger("4Closed");
                        }
                    }
                    else if (typeChest == 2)
                    {
                        animator.SetTrigger("1Closed");
                    }
                    else
                    {
                        animator.SetTrigger("3Closed");
                    }
                }
                else
                {
                    if (typeChest == 1)
                    {
                        if (chestVariation == 1)
                        {
                            animator.SetTrigger("2Open");
                        }
                        else
                        {
                            animator.SetTrigger("4Open");
                        }
                    }
                    else if (typeChest == 2)
                    {
                        animator.SetTrigger("1Open");
                    }
                    else
                    {
                        animator.SetTrigger("3Open");
                    }
                }
            }
            else
            {
                if (beenOpened)
                {
                    animator.SetTrigger("Opened");
                    //set open anim for future chests for parameter id to "Opened"
                }
            }
        }
    }

    void updateChest()
    {
        for (int i = 0; i < chestSlots.Length; i++)
        {
            chestSlots[i].targetChest = this.gameObject.GetComponent<Chest>();
        }
    }

    void generateItems(GameObject[] items)
    {
        if (uniqueChest == false)
        {
            int dangerValue = ((int)Vector3.Magnitude(transform.position) / 20) + 1;
            int numSlots = 0;
            int percentageArtifact = dangerValue + 7 + playerShip.GetComponent<PlayerScript>().numRoomsSinceLastArtifact + bonusArtifactChance + extraArtifactChance;
            int rareArtifactMod = 0, epicArtifactMod = 0;

            //Sets number of chest items
            if (dangerValue < 4)
            {
                if (Random.Range(1, 101) < 25)
                {
                    typeChest = 2;
                }
                else
                {
                    typeChest = 1;
                }
                numSlots = 1;
            }
            else if (dangerValue >= 4 && dangerValue < 7)
            {
                int rand = Random.Range(1, 101);
                if (rand < 50)
                {
                    typeChest = 1;
                }
                else if (rand >= 50)
                {
                    typeChest = 2;
                }
                numSlots = Random.Range(1, 3);
            }
            else
            {
                int rand = Random.Range(1, 101);
                if (rand < 40)
                {
                    typeChest = 1;
                }
                else if (rand >= 40 && rand <= 90)
                {
                    typeChest = 2;
                }
                numSlots = Random.Range(2, 4);
            }

            selectAnim(typeChest);

            rareArtifactMod = (typeChest - 1) * 3 + betterArtifactChance;
            epicArtifactMod = (typeChest - 2) * 4 + betterArtifactChance;

            for (int i = 0; i < numSlots; i++)
            {
                GameObject newItem;
                if (Random.Range(1, 101) <= percentageArtifact)
                {
                    int artifactRarity = Random.Range(0, 101);
                    if (artifactRarity > 50 + rareArtifactMod)
                    {
                        newItem = itemTemplates.loadRandomItem(1);
                    }
                    else if (artifactRarity > 20 + epicArtifactMod && artifactRarity <= 50 + rareArtifactMod)
                    {
                        newItem = itemTemplates.loadRandomItem(2);
                    }
                    else if (artifactRarity <= 20 + epicArtifactMod && artifactRarity > 5)
                    {
                        newItem = itemTemplates.loadRandomItem(3);
                    }
                    else
                    {
                        newItem = itemTemplates.loadRandomItem(4);
                    }
                    playerShip.GetComponent<PlayerScript>().numRoomsSinceLastArtifact = 0;
                }
                else
                {
                    newItem = Instantiate(itemTemplates.gold);
                    newItem.GetComponent<DisplayItem>().goldValue = dangerValue * 75 + Random.Range(0, 4) * 50 + 75 * typeChest + bonusGold;
                }
                newItem.transform.SetParent(presentItems.transform);
                items[i] = newItem;
            }
        }
        else
        {
            GameObject newItem;
            for (int i = 0; i < numUniqueItems; i++)
            {
                newItem = Instantiate(uniqueItems[Random.Range(0, uniqueItems.Length)]);
                if(newItem.GetComponent<DisplayItem>().goldValue > 0)
                {
                    newItem.GetComponent<DisplayItem>().goldValue = customGoldBase + Random.Range(0, 4) * customGoldMultiplier;
                }
                newItem.transform.parent = presentItems.transform;
                items[i] = newItem;
            }
        }
    }

    private void Awake()
    {
        chestItems = new GameObject[3];
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        itemTemplates = GameObject.Find("ItemTemplates").GetComponent<ItemTemplates>();
        chestDisplay = playerShip.GetComponent<Inventory>().chestDisplay;
        inventoryDisplay = playerShip.GetComponent<Inventory>().inventory;
        toolTip = playerShip.GetComponent<Inventory>().toolTip;
        chestDisplay.SetActive(false);
        chestSlots = chestDisplay.GetComponentsInChildren<ChestSlot>();
        presentItems = GameObject.Find("PresentItems");
        generateItems(chestItems);
    }

    private void LateUpdate()
    {
        if (Vector2.Distance(playerShip.transform.position, transform.position) < 2.5f && playerShip.GetComponent<PlayerScript>().enemiesDefeated == true)
        {
            if (spawnedIndicator == null)
            {
                spawnedIndicator = Instantiate(indicator, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                spawnedIndicator.GetComponent<ChestIndicator>().parentChest = this.gameObject;
            }

            if (displayOn == false)
            {
                if (Input.GetKeyDown(KeyCode.F) && playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
                {
                    playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = true;
                    beenOpened = true;
                    selectAnim(typeChest);
                    displayOn = true;
                    UpdateUI();
                    Time.timeScale = 0;
                    chestDisplay.SetActive(true);
                    inventoryDisplay.SetActive(true);
                    playerShip.GetComponent<Inventory>().UpdateUI();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F))
                {
                    if(toolTip.activeSelf == true)
                    {
                        toolTip.SetActive(false);
                    }
                    playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = false;
                    Time.timeScale = 1;
                    displayOn = false;
                    chestDisplay.SetActive(false);
                    inventoryDisplay.SetActive(false);
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

    public void UpdateUI()
    {
        updateChest();
        for (int i = 0; i < chestSlots.Length; i++)
        {
            if(chestItems[i] != null)
            {
                chestSlots[i].addSlot(chestItems[i].GetComponent<DisplayItem>());
            }
            else
            {
                chestSlots[i].deleteSlot();
            }
        }
    }
}
