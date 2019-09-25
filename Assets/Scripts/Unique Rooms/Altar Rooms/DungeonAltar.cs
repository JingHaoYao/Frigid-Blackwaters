using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAltar : MonoBehaviour
{
    GameObject spawnedIndicator;
    GameObject playerShip;
    public GameObject altarDisplay, examineIndicator;
    ItemTemplates itemTemplates;
    public int whatTier = 1;
    public int healthSacrifice;
    public GameObject altarArtifact;
    public Sprite unActive;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        itemTemplates = FindObjectOfType<ItemTemplates>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        setItem();
        altarDisplay = GameObject.Find("Altar Menu Parent").transform.GetChild(0).gameObject;
    }

    public void setUnActive()
    {
        spriteRenderer.sprite = unActive;
    }

    void setItem()
    {
        GameObject newItem;
        if(whatTier == 1)
        {
            int percentItem = Random.Range(1, 101);
            if(percentItem <= 50)
            {
                newItem = itemTemplates.loadRandomItem(1);
                newItem.transform.parent = GameObject.Find("PresentItems").transform;
                healthSacrifice = 200 + Random.Range(1, 5) * 25;
            }
            else
            {
                newItem = itemTemplates.loadRandomItem(2);
                newItem.transform.parent = GameObject.Find("PresentItems").transform;
                healthSacrifice = 350 + Random.Range(1, 4) * 25;
            }
        }
        else if(whatTier == 2)
        {
            int percentItem = Random.Range(1, 101);
            if (percentItem <= 25)
            {
                newItem = itemTemplates.loadRandomItem(1);
                newItem.transform.parent = GameObject.Find("PresentItems").transform;
                healthSacrifice = 125 + Random.Range(1, 5) * 25;
            }
            else if(percentItem > 25 && percentItem <= 75)
            {
                newItem = itemTemplates.loadRandomItem(2);
                newItem.transform.parent = GameObject.Find("PresentItems").transform;
                healthSacrifice = 250 + Random.Range(1, 4) * 25;
            }
            else
            {
                newItem = itemTemplates.loadRandomItem(3);
                newItem.transform.parent = GameObject.Find("PresentItems").transform;
                healthSacrifice = 500 + Random.Range(1, 4) * 50;
            }
        }
        else
        {
            int percentItem = Random.Range(1, 101);
            if (percentItem <= 25)
            {
                newItem = itemTemplates.loadRandomItem(1);
                newItem.transform.parent = GameObject.Find("PresentItems").transform;
                healthSacrifice = 200 + Random.Range(1, 4) * 25;
            }
            else if (percentItem > 25 && percentItem <= 80)
            {
                newItem = itemTemplates.loadRandomItem(2);
                newItem.transform.parent = GameObject.Find("PresentItems").transform;
                healthSacrifice = 400 + Random.Range(1, 4) * 25;
            }
            else
            {
                newItem = itemTemplates.loadRandomItem(3);
                newItem.transform.parent = GameObject.Find("PresentItems").transform;
                healthSacrifice = 650 + Random.Range(1, 4) * 50;
            }
        }
        altarArtifact = newItem;
    }

    void updateDisplay()
    {
        AltarMenu altarMenu = altarDisplay.GetComponentInChildren<AltarMenu>();
        altarMenu.addSlot(altarArtifact.GetComponent<DisplayItem>(), healthSacrifice);
        altarMenu.altar = this;
    }

    void LateUpdate()
    {
        if (Vector2.Distance(playerShip.transform.position, transform.position) < 5f && playerShip.GetComponent<PlayerScript>().enemiesDefeated == true && spriteRenderer.sprite != unActive)
        {
            if (altarDisplay.activeSelf == false)
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

            if (altarDisplay.activeSelf == true)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
                {
                    altarDisplay.SetActive(false);
                    playerShip.GetComponent<PlayerScript>().shipRooted = false;
                    Time.timeScale = 1;
                    playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = false;
                }
            }
            else if (playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    altarDisplay.SetActive(true);
                    playerShip.GetComponent<PlayerScript>().shipRooted = true;
                    updateDisplay();
                    Time.timeScale = 0;
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
        else
        {
            if (spawnedIndicator != null)
            {
                Destroy(spawnedIndicator);
            }
        }
    }
}

