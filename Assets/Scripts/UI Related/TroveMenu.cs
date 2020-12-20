using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TroveMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DisplayItem displayInfo;
    public GameObject toolTip;

    public Image imageIcon;
    Inventory inventory;
    PlayerScript playerScript;
    public DungeonTrove trove;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        playerScript = FindObjectOfType<PlayerScript>();
    }

    public void addSlot(DisplayItem _displayInfo)
    {
        displayInfo = _displayInfo;
        imageIcon.sprite = displayInfo.displayIcon;
    }

    public void transferItem()
    {
        if (displayInfo != null && inventory.itemList.Count < inventory.inventorySize)
        {
            if (inventory.itemList.Count < inventory.inventorySlots.Length)
            {
                inventory.itemList.Add(displayInfo.gameObject);
                FindObjectOfType<AudioManager>().PlaySound("Receive Item");
                //set trove unactive
                imageIcon.enabled = false;
                trove.setUnActive();
                trove.PlayEndingAnimation();
                playerScript.windowAlreadyOpen = false;
                Time.timeScale = 1;
                PlayerProperties.playerScript.removeRootingObject();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (displayInfo != null)
        {
            toolTip.SetActive(false);
            PlayerProperties.artifactToolTip.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (displayInfo != null)
        {
            if (!displayInfo.isArtifact)
            {
                PlayerProperties.toolTip.SetTextAndPosition(displayInfo.GetComponent<Text>().text, transform.position);
            }
            else
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
        }
    }
}
