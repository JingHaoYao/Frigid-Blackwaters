using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GoldenVaultTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DisplayItem displayInfo;
    public GameObject imageIcon;
    public Inventory inventory;
    public GameObject toolTip;
    public GoldenVault goldenVault;

    void Start () {
        inventory = GameObject.Find("PlayerShip").GetComponent<Inventory>();
	}

	void Update () {
		
	}

    public void addSlot(DisplayItem _displayInfo)
    {
        displayInfo = _displayInfo;
        imageIcon.SetActive(true);
        imageIcon.GetComponent<Image>().sprite = _displayInfo.displayIcon;
    }

    public void deleteSlot()
    {
        displayInfo = null;
        imageIcon.SetActive(false);
        PlayerProperties.artifactToolTip.gameObject.SetActive(false);
        toolTip.SetActive(false);
        PlayerProperties.consumableToolTip.gameObject.SetActive(false);
    }

    public void transferItem()
    {
        if(displayInfo != null && inventory.itemList.Count < inventory.inventorySize)
        {
            goldenVault.vaultItems.Remove(displayInfo.gameObject);
            HubProperties.vaultItems.Remove(displayInfo.gameObject.name);
            inventory.itemList.Add(displayInfo.gameObject);
            inventory.UpdateUI();
            goldenVault.UpdateUI();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (displayInfo != null)
        {
            toolTip.SetActive(false);
            PlayerProperties.artifactToolTip.gameObject.SetActive(false);
            PlayerProperties.consumableToolTip.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (displayInfo != null)
        {
            if (displayInfo.isArtifact)
            {
                ArtifactBonus artifactBonus = displayInfo.GetComponent<ArtifactBonus>();
                PlayerProperties.artifactToolTip.SetTextAndPosition(
                    artifactBonus.artifactName,
                    artifactBonus.descriptionText.text,
                    artifactBonus.effectText == null ? "" : artifactBonus.effectText.text,
                    artifactBonus.attackBonus,
                    artifactBonus.speedBonus,
                    artifactBonus.healthBonus,
                    artifactBonus.defenseBonus,
                    artifactBonus.periodicHealing,
                    displayInfo.hasActive,
                    displayInfo.soulBound,
                    artifactBonus.killRequirement,
                    artifactBonus.whatRarity,
                    transform.position);
            }
            else if (displayInfo.isConsumable)
            {
                ConsumableBonus consumableBonus = displayInfo.GetComponent<ConsumableBonus>();
                PlayerProperties.consumableToolTip.SetTextAndPosition(
                    consumableBonus.consumableName,
                    consumableBonus.loreText.text,
                    consumableBonus.effectText == null ? "" : consumableBonus.effectText.text,
                    consumableBonus.attackBonus,
                    consumableBonus.speedBonus,
                    consumableBonus.defenseBonus,
                    consumableBonus.restoredHealth,
                    consumableBonus.duration,
                    transform.position);
            }
            else
            {
                PlayerProperties.toolTip.SetTextAndPosition(displayInfo.GetComponent<Text>().text, transform.position);
            }
        }
    }
}
