using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOfTheDrownedTreasure : ArtifactBonus
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] DisplayItem displayItem;
    private ItemTemplates itemTemplates;
    DrownedTreasureChest goldenChestInstant, violentChestInstant, healingChestInstant;
    [SerializeField] GameObject goldenChest, violentchest, healingChest;
    [SerializeField] AudioSource waterSplashAudio;
    private bool artifactActive = false;
    Camera mainCamera;

    private void Start()
    {
        itemTemplates = FindObjectOfType<ItemTemplates>();
        mainCamera = Camera.main;
    }

    void healPlayer()
    {
        PlayerProperties.playerScript.healPlayer(PlayerProperties.playerScript.trueDamage);
    }

    void statBuff()
    {
        PlayerProperties.playerScript.conAttackBonus += 4;
        PlayerProperties.playerScript.conSpeedBonus += 2;
    }

    void addGold()
    {
        int goldToAdd = 2000;
        foreach (GameObject item in PlayerProperties.playerInventory.itemList)
        {
            DisplayItem displayItem = item.GetComponent<DisplayItem>();
            if (displayItem.goldValue > 0 && displayItem.goldValue < 1000)
            {
                int goldToSubtract = Mathf.Clamp(goldToAdd, 0, 1000 - displayItem.goldValue);
                goldToAdd -= goldToSubtract;
                displayItem.goldValue += goldToSubtract;
            }

            if (goldToAdd <= 0)
            {
                break;
            }
        }

        while(goldToAdd > 1000 && PlayerProperties.playerInventory.itemList.Count < PlayerItems.maxInventorySize)
        {
            GameObject goldInstant = Instantiate(itemTemplates.gold);
            goldInstant.GetComponent<DisplayItem>().goldValue = 1000;
            goldToAdd -= 1000;
            PlayerProperties.playerInventory.itemList.Add(goldInstant);
        }

        if (goldToAdd > 0 && PlayerProperties.playerInventory.itemList.Count < PlayerItems.maxInventorySize)
        {
            GameObject goldInstant = Instantiate(itemTemplates.gold);
            goldInstant.GetComponent<DisplayItem>().goldValue = goldToAdd;
            PlayerProperties.playerInventory.itemList.Add(goldInstant);
        }

        PlayerItems.totalGoldAmount += 2000 - goldToAdd;
    }

    public void activateTreasures(WhichTreasureChest whichTreasureChest)
    {
        artifactActive = false;
        switch (whichTreasureChest)
        {
            case WhichTreasureChest.Golden:
                addGold();
                violentChestInstant.Sink();
                healingChestInstant.Sink();
                break;
            case WhichTreasureChest.Violent:
                goldenChestInstant.Sink();
                healingChestInstant.Sink();
                statBuff();
                break;
            case WhichTreasureChest.Healing:
                violentChestInstant.Sink();
                goldenChestInstant.Sink();
                healPlayer();
                break;
        }
    }

    void spawnChests()
    {
        if (artifactActive == false)
        {
            PlayerProperties.playerArtifacts.numKills -= killRequirement;
            audioSource.Play();
            waterSplashAudio.Play();
            goldenChestInstant = Instantiate(goldenChest, checkSpawnPosition(Mathf.PI / 2), Quaternion.identity).GetComponent<DrownedTreasureChest>();
            violentChestInstant = Instantiate(violentchest, checkSpawnPosition((Mathf.PI / 2) - Mathf.PI * 2 / 3), Quaternion.identity).GetComponent<DrownedTreasureChest>();
            healingChestInstant = Instantiate(healingChest, checkSpawnPosition((Mathf.PI / 2) + Mathf.PI * 2 / 3), Quaternion.identity).GetComponent<DrownedTreasureChest>();
            goldenChestInstant.Initialize(this);
            violentChestInstant.Initialize(this);
            healingChestInstant.Initialize(this);
            artifactActive = true;
        }
    }

    Vector3 checkSpawnPosition(float directionInRad)
    {
        float distance = 3;
        Vector3 currentSpawnPos = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(directionInRad), Mathf.Sin(directionInRad)) * distance;
        while(Physics2D.OverlapCircle(currentSpawnPos, 0.35f, 12)){
            distance++;
            currentSpawnPos = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(directionInRad), Mathf.Sin(directionInRad)) * distance;
        }

        return new Vector3(Mathf.Clamp(currentSpawnPos.x, mainCamera.transform.position.x - 8, mainCamera.transform.position.x + 8), Mathf.Clamp(currentSpawnPos.y, mainCamera.transform.position.y - 8, mainCamera.transform.position.y + 8));
    }

    private void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    spawnChests();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    spawnChests();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    spawnChests();
                }
            }
        }
    }
}
