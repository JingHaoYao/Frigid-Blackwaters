using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArticraftingTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image tileImage;
    [SerializeField] Image backgroundImage;
    private GameObject targetArtifact;
    private ArtifactBonus artifactBonus;
    private Text artifactText;
    private DisplayItem artifactDisplayItem;
    GameObject toolTip;
    int priceOfArtifact;
    ArticraftingConfirmationModal confirmModal;
    ArticraftingCraftingMenu craftingMenu;

    [SerializeField] Sprite[] animationSprites;
    
    IEnumerator animateCrafting()
    {
        foreach(Sprite sprite in animationSprites)
        {
            yield return new WaitForSecondsRealtime(1 / 12f);
            backgroundImage.sprite = sprite;
        }
    }
    
    public void Initialize(GameObject artifact, ArticraftingConfirmationModal modal, ArticraftingCraftingMenu menu)
    {
        targetArtifact = artifact;
        artifactBonus = artifact.GetComponent<ArtifactBonus>();
        artifactText = artifact.GetComponent<Text>();
        artifactDisplayItem = artifact.GetComponent<DisplayItem>();
        tileImage.sprite = artifactDisplayItem.displayIcon;
        tileImage.preserveAspect = true;
        toolTip = PlayerProperties.playerInventory.toolTip;
        confirmModal = modal;
        UpdatePrice();
        this.craftingMenu = menu;
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }

    public void Enable(GameObject artifact, ArticraftingConfirmationModal modal, ArticraftingCraftingMenu menu)
    {
        this.gameObject.SetActive(true);
        Initialize(artifact, modal, menu);
    }

    public void UpdatePrice()
    {
        priceOfArtifact = determinePrice(targetArtifact.name, artifactBonus.whatRarity, artifactBonus.whatDungeonArtifact);
    }

    private int determinePrice(string name, int rarity, int dungeonLevel)
    {
        string compareName = name;
        int numberDuplicates = 0;
        if (name.Contains("(Clone)"))
        {
            compareName = compareName.Replace("(Clone)", "").Trim();
        }

        foreach(string itemName in PlayerItems.inventoryItemsIDs)
        {
            if(itemName.Replace("(Clone)", "").Trim() == compareName)
            {
                numberDuplicates++;
            }
        }

        foreach(string itemName in PlayerItems.activeArtifactsIDs)
        {
            if (itemName != null)
            {
                if (itemName.Replace("(Clone)", "").Trim() == compareName)
                {
                    numberDuplicates++;
                }
            }
        }

        foreach(string itemName in HubProperties.vaultItems)
        {
            if (itemName.Replace("(Clone)", "").Trim() == compareName)
            {
                numberDuplicates++;
            }
        }

        switch(rarity)
        {
            case 0:
                return (10 + 5 * numberDuplicates) * dungeonLevel;
            case 1:
                return (16 + 8 * numberDuplicates) * dungeonLevel;
            case 2:
                return (30 + 15 * numberDuplicates) * dungeonLevel;
            case 3:
                return (70 + 35 * numberDuplicates) * dungeonLevel;
        }
        return -1;
    }

    void CraftArtifact()
    {
        PlayerUpgrades.numberArtifragments -= priceOfArtifact;
        UpdatePrice(); // update price
        GameObject artifactInstant = Instantiate(targetArtifact);
        PlayerProperties.playerInventory.itemList.Add(targetArtifact);
        PlayerProperties.playerInventory.UpdateUI();
        confirmModal.Disable();
        StartCoroutine(animateCrafting());
        craftingMenu.CraftedItemVisuals(priceOfArtifact);
    }

    public void ShowConfirmationModal()
    {
        PlayerProperties.audioManager.PlaySound("Generic Button Click");
        if(PlayerUpgrades.numberArtifragments < priceOfArtifact)
        {
            confirmModal.Initialize("You need " + (priceOfArtifact - PlayerUpgrades.numberArtifragments).ToString() + " more artifragments.", true, null); 
        }
        else if(PlayerProperties.playerInventory.itemList.Count >= PlayerItems.maxInventorySize)
        {
            confirmModal.Initialize("You don't have enough room in your inventory.", true, null);
        }
        else
        {
            confirmModal.Initialize("Articraft for " + priceOfArtifact.ToString() + " artifragments?", false, CraftArtifact);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.SetActive(true);
        toolTip.transform.position = this.transform.position;
        toolTip.GetComponentInChildren<Text>().text = artifactText.text;
    }
}
