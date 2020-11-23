using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGamble : MonoBehaviour
{
    GameObject spawnedIndicator;
    GameObject playerShip;
    public GameObject gambleDisplay, examineIndicator;
    ItemTemplates itemTemplates;
    public int whatTier = 1;
    SpriteRenderer spriteRenderer;
    public bool hasItem = false;
    GambleMenuArtifactSlot artifactSlot;
    GambleMenuGoldSlot goldSlot;
    Animator animator;
    public Sprite depleted, replenished, regular;
    public int gamblePrice = 0;
    DisplayItem targetItem;
    int whatDungeonLevel;

    MenuSlideAnimation menuSlideAnimation = new MenuSlideAnimation();

    void SetAnimation()
    {
        menuSlideAnimation.SetOpenAnimation(new Vector3(0, -585, 0), new Vector3(0, 0, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(0, 0, 0), new Vector3(0, -585, 0), 0.25f);
    }

    public void PlayEndingAnimation()
    {
        menuSlideAnimation.PlayEndingAnimation(gambleDisplay, () => { gambleDisplay.SetActive(false); });
    }

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        itemTemplates = FindObjectOfType<ItemTemplates>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gambleDisplay = GameObject.Find("Gamble Menu Parent").transform.GetChild(0).gameObject;
        artifactSlot = gambleDisplay.GetComponentInChildren<GambleMenuArtifactSlot>();
        goldSlot = gambleDisplay.GetComponentInChildren<GambleMenuGoldSlot>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
        whatDungeonLevel = FindObjectOfType<DungeonEntryDialogueManager>().whatDungeonLevel;
        setPrice();
        SetAnimation();
    }

    void setPrice()
    {
        if(whatTier == 1)
        {
            gamblePrice = 50 + Random.Range(1, 3) * 50 + Random.Range(1, 4) * 25 + (whatDungeonLevel - 1) * 300;
        }
        else if(whatTier == 2)
        {
            gamblePrice = 150 + Random.Range(1, 4) * 50 + Random.Range(1, 4) * 25 + (whatDungeonLevel - 1) * 350;
        }
        else
        {
            gamblePrice = 300 + Random.Range(1, 4) * 50 + Random.Range(1, 4) * 25 + (whatDungeonLevel - 1) * 400;
        }
    }

    IEnumerator fail()
    {
        animator.enabled = true;
        animator.SetTrigger("Failure");
        yield return new WaitForSeconds(2f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(5f / 12f);
        hasItem = false;
        animator.enabled = false;
        spriteRenderer.sprite = depleted;
    }

    IEnumerator success()
    {
        animator.enabled = true;
        animator.SetTrigger("Success");
        yield return new WaitForSeconds(2f / 12f);
        this.GetComponents<AudioSource>()[0].Play();
        yield return new WaitForSeconds(7f / 12f);
        hasItem = true;
        animator.enabled = false;
        spriteRenderer.sprite = replenished;
    }

    public void gamble()
    {
        targetItem = gambleItem();
        if(targetItem == null)
        {
            StartCoroutine(fail());
        }
        else
        {
            StartCoroutine(success());
        }
    }

    void updateDisplay(DisplayItem item)
    {
        spriteRenderer.sprite = regular;
        artifactSlot.gamble = this;
        goldSlot.gamble = this;
        if(hasItem == true)
        {
            artifactSlot.gameObject.SetActive(true);
            goldSlot.gameObject.SetActive(false);
            artifactSlot.addSlot(item);
        }
        else
        {
            artifactSlot.gameObject.SetActive(false);
            goldSlot.gameObject.SetActive(true);
            goldSlot.text.text = gamblePrice.ToString();
        }
    }

    DisplayItem gambleItem()
    {
        if(whatTier == 1)
        {
            int percentItem = Random.Range(1, 101);
            if(percentItem <= 40)
            {
                return null;
            }
            else if(percentItem > 40 && percentItem < 70)
            {
                if(Random.Range(0, 3) != 1)
                {
                    GameObject newItem = itemTemplates.loadRandomItem(5);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
                else
                {
                    GameObject newItem = itemTemplates.loadRandomItem(6);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
            }
            else
            {
                int whatArtifact = Random.Range(1, 101);
                if(whatArtifact <= 60)
                {
                    GameObject newItem = itemTemplates.loadRandomItem(1);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
                else if(whatArtifact > 60 && whatArtifact <= 90)
                {
                    GameObject newItem = itemTemplates.loadRandomItem(2);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
                else
                {
                    GameObject newItem = itemTemplates.loadRandomItem(3);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
            }
        }
        else if(whatTier == 2)
        {
            int percentItem = Random.Range(1, 101);
            if (percentItem <= 30)
            {
                return null;
            }
            else if (percentItem > 30 && percentItem < 60)
            {
                if (Random.Range(0, 3) != 1)
                {
                    GameObject newItem = itemTemplates.loadRandomItem(5);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
                else
                {
                    GameObject newItem = itemTemplates.loadRandomItem(6);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
            }
            else
            {
                int whatArtifact = Random.Range(1, 101);
                if (whatArtifact <= 25)
                {
                    GameObject newItem = itemTemplates.loadRandomItem(1);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
                else if (whatArtifact > 25 && whatArtifact <= 70)
                {
                    GameObject newItem = itemTemplates.loadRandomItem(2);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
                else
                {
                    GameObject newItem = itemTemplates.loadRandomItem(3);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
            }
        }
        else
        {
            int percentItem = Random.Range(1, 101);
            if (percentItem <= 20)
            {
                return null;
            }
            else if (percentItem > 20 && percentItem < 50)
            {
                if (Random.Range(0, 3) != 1)
                {
                    GameObject newItem = itemTemplates.loadRandomItem(5);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
                else
                {
                    GameObject newItem = itemTemplates.loadRandomItem(6);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
            }
            else
            {
                int whatArtifact = Random.Range(1, 101);
                if (whatArtifact <= 40)
                {
                    GameObject newItem = itemTemplates.loadRandomItem(2);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
                else if (whatArtifact > 40 && whatArtifact <= 80)
                {
                    GameObject newItem = itemTemplates.loadRandomItem(3);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
                else
                {
                    GameObject newItem = itemTemplates.loadRandomItem(4);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    return newItem.GetComponent<DisplayItem>();
                }
            }
        }
    }
    

    void LateUpdate()
    {
        if (Vector2.Distance(playerShip.transform.position, transform.position) < 5f && playerShip.GetComponent<PlayerScript>().enemiesDefeated == true && animator.enabled == false)
        {
            if (gambleDisplay.activeSelf == false)
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
                if (gambleDisplay.activeSelf == true)
                {
                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
                    {
                        menuSlideAnimation.PlayEndingAnimation(gambleDisplay, () => { gambleDisplay.SetActive(false); });
                        playerShip.GetComponent<PlayerScript>().removeRootingObject();
                        Time.timeScale = 1;
                        playerShip.GetComponent<PlayerScript>().windowAlreadyOpen = false;
                    }
                }
                else if (playerShip.GetComponent<PlayerScript>().windowAlreadyOpen == false)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        gambleDisplay.SetActive(true);
                        menuSlideAnimation.PlayOpeningAnimation(gambleDisplay);
                        updateDisplay(targetItem);
                        playerShip.GetComponent<PlayerScript>().addRootingObject();
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
