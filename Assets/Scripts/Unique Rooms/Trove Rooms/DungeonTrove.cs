using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonTrove : MonoBehaviour
{
    GameObject spawnedIndicator;
    GameObject playerShip;
    public GameObject troveDisplay, examineIndicator;
    ItemTemplates itemTemplates;
    public int whatTier = 1;
    public GameObject targetArtifact;
    public Sprite unActive;
    SpriteRenderer spriteRenderer;
    public GameObject customArtifact;

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        itemTemplates = FindObjectOfType<ItemTemplates>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        troveDisplay = GameObject.Find("Trove Menu Parent").transform.GetChild(0).gameObject;
        setItem();
    }

    public void setUnActive()
    {
        spriteRenderer.sprite = unActive;
    }

    void setItem()
    {
        GameObject newItem;
        if (customArtifact == null)
        {
            if (whatTier == 1)
            {
                int percentItem = Random.Range(1, 101);
                if (percentItem <= 75)
                {
                    newItem = itemTemplates.loadRandomItem(1);
                    newItem.transform.parent = GameObject.Find("PresentItems").transform;
                }
                else
                {
                    newItem = itemTemplates.loadRandomItem(2);
                    newItem.transform.parent = GameObject.Find("PresentItems").transform;
                }
            }
            else if (whatTier == 2)
            {
                int percentItem = Random.Range(1, 101);
                if (percentItem <= 80)
                {
                    newItem = itemTemplates.loadRandomItem(2);
                    newItem.transform.parent = GameObject.Find("PresentItems").transform;
                }
                else
                {
                    newItem = itemTemplates.loadRandomItem(3);
                    newItem.transform.parent = GameObject.Find("PresentItems").transform;
                }
            }
            else
            {
                int percentItem = Random.Range(1, 101);
                if (percentItem <= 50)
                {
                    newItem = itemTemplates.loadRandomItem(2);
                    newItem.transform.parent = GameObject.Find("PresentItems").transform;
                }
                else if (percentItem > 50 && percentItem <= 95)
                {
                    newItem = itemTemplates.loadRandomItem(3);
                    newItem.transform.parent = GameObject.Find("PresentItems").transform;
                }
                else
                {
                    newItem = itemTemplates.loadRandomItem(4);
                    newItem.transform.parent = GameObject.Find("PresentItems").transform;
                }
            }
        }
        else
        {
            newItem = Instantiate(customArtifact);
            newItem.transform.parent = GameObject.Find("PresentItems").transform;
        }
        targetArtifact = newItem;
    }

    void updateDisplay()
    {
        TroveMenu menu = troveDisplay.GetComponentInChildren<TroveMenu>();
        menu.addSlot(targetArtifact.GetComponent<DisplayItem>());
        menu.trove = this;
    }

    void LateUpdate()
    {
        if (Vector2.Distance(playerShip.transform.position, transform.position) < 5f && playerShip.GetComponent<PlayerScript>().enemiesDefeated == true && spriteRenderer.sprite != unActive)
        {
            if (troveDisplay.activeSelf == false)
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

            if (troveDisplay.activeSelf == true)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
                {
                    troveDisplay.SetActive(false);
                    playerShip.GetComponent<PlayerScript>().removeRootingObject();
                    Time.timeScale = 1;
                    playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = false;
                }
            }
            else if (playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    troveDisplay.SetActive(true);
                    playerShip.GetComponent<PlayerScript>().addRootingObject();
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
