using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTreasure : MonoBehaviour
{
    GameObject spawnedIndicator;
    GameObject playerShip;
    public GameObject treasureDisplay, examineIndicator;
    ItemTemplates itemTemplates;
    public int whatTier = 1;
    public GameObject targetArtifact;
    public Sprite unActive;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        itemTemplates = FindObjectOfType<ItemTemplates>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        treasureDisplay = GameObject.Find("Treasure Menu Parent").transform.GetChild(0).gameObject;
        setItem();
    }

    public void setUnActive()
    {
        spriteRenderer.sprite = unActive;
    }

    void setItem()
    {
        GameObject newItem;
        if (whatTier == 1)
        {
            newItem = Instantiate(itemTemplates.gold);
            newItem.GetComponent<DisplayItem>().goldValue = 200 + 50 * Random.Range(1, 4) + 25 * Random.Range(1, 5);
            newItem.transform.parent = GameObject.Find("PresentItems").transform;
        }
        else if (whatTier == 2)
        {
            newItem = Instantiate(itemTemplates.gold);
            newItem.GetComponent<DisplayItem>().goldValue = 400 + 50 * Random.Range(1, 4) + 25 * Random.Range(1, 5);
            newItem.transform.parent = GameObject.Find("PresentItems").transform;
        }
        else
        {
            newItem = Instantiate(itemTemplates.gold);
            newItem.GetComponent<DisplayItem>().goldValue = 600 + 75 * Random.Range(1, 4) + 25 * Random.Range(1, 5);
            newItem.transform.parent = GameObject.Find("PresentItems").transform;
        }
        targetArtifact = newItem;
    }

    void updateDisplay()
    {
        TreasureMenu menu = treasureDisplay.GetComponentInChildren<TreasureMenu>();
        menu.addSlot(targetArtifact.GetComponent<DisplayItem>());
        menu.treasure = this;
    }

    void LateUpdate()
    {
        if (Vector2.Distance(playerShip.transform.position, transform.position) < 5f && playerShip.GetComponent<PlayerScript>().enemiesDefeated == true && spriteRenderer.sprite != unActive)
        {
            if (treasureDisplay.activeSelf == false)
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

            if (treasureDisplay.activeSelf == true)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
                {
                    treasureDisplay.SetActive(false);
                    playerShip.GetComponent<PlayerScript>().shipRooted = false;
                    Time.timeScale = 1;
                    playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = false;
                }
            }
            else if (playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    treasureDisplay.SetActive(true);
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
