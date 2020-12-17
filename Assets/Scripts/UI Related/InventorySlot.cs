
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public GameObject imageIcon, goldInfo;
    public DisplayItem displayInfo;
    public Inventory inventory;
    public Artifacts artifacts;
    public GameObject toolTip;
    GoldenVault goldenVault;
    public GameObject goldenVaultDisplay;
    public bool slotLocked = true;
    public GameObject lockedIcon;

    private void Start()
    {
        inventory = PlayerProperties.playerInventory;
        artifacts = PlayerProperties.playerArtifacts;
        toolTip = inventory.toolTip;
        goldenVault = FindObjectOfType<GoldenVault>();
    }

    public void unlockSlot()
    {
        slotLocked = false;
        lockedIcon.SetActive(false);
    }

    public void lockSlot()
    {
        slotLocked = true;
        lockedIcon.SetActive(true);
    }

    public void addSlot(DisplayItem _displayInfo)
    {
        displayInfo = _displayInfo;
        imageIcon.SetActive(true);
        goldInfo.SetActive(true);
        imageIcon.GetComponent<Image>().sprite = displayInfo.displayIcon;
        if (displayInfo.goldValue != 0)
        {
            goldInfo.GetComponent<Text>().text = displayInfo.goldValue.ToString();
        }
        else
        {
            goldInfo.SetActive(false);
        }
    }

    public void deleteSlot()
    {
        displayInfo = null;
        imageIcon.SetActive(false);
        goldInfo.SetActive(false);
        toolTip.SetActive(false);
    }

    public void deleteItem()
    {
        if (displayInfo != null && Input.GetKey(KeyCode.LeftShift) && SceneManager.GetActiveScene().name != "Tutorial" && FindObjectOfType<ConsumableConfirm>() == null)
        {
            FindObjectOfType<AudioManager>().PlaySound("Destroy Item");
            inventory.itemList.Remove(displayInfo.gameObject);
            if(displayInfo.GetComponent<ArtifactEffect>())
            {
                displayInfo.GetComponent<ArtifactEffect>().artifactDestroyed();
            }
            Destroy(displayInfo.gameObject);
            displayInfo = null;
        }
    }

    public void consumeItem()
    {
        if(!Input.GetKeyDown(KeyCode.LeftShift) && displayInfo != null && displayInfo.isConsumable == true && FindObjectOfType<ConsumableConfirm>() == null)
        {
            if (goldenVaultDisplay != null)
            {
                if (goldenVaultDisplay.activeSelf != true)
                {
                    if (PlayerProperties.playerScript.shipHealth + displayInfo.GetComponent<ConsumableBonus>().restoredHealth > PlayerProperties.playerScript.shipHealthMAX)
                    {
                        inventory.consumableConfirmationWindow.gameObject.SetActive(true);
                        inventory.consumableConfirmationWindow.objectInQuestion = displayInfo.gameObject;
                    }
                    else
                    {
                        if (displayInfo.gameObject.GetComponent<ConsumableBonus>().restoredHealth > 0)
                        {
                            FindObjectOfType<AudioManager>().PlaySound("Consume Heal Item");
                        }
                        else
                        {
                            FindObjectOfType<AudioManager>().PlaySound("Consume Non Heal Item");
                        }
                        displayInfo.gameObject.GetComponent<ConsumableBonus>().consumeItem();
                        inventory.itemList.Remove(displayInfo.gameObject);
                    }
                }
            }
            else
            {
                if (PlayerProperties.playerScript.shipHealth + displayInfo.GetComponent<ConsumableBonus>().restoredHealth > PlayerProperties.playerScript.shipHealthMAX)
                {
                    inventory.consumableConfirmationWindow.gameObject.SetActive(true);
                    inventory.consumableConfirmationWindow.objectInQuestion = displayInfo.gameObject;
                }
                else
                {
                    if (displayInfo.gameObject.GetComponent<ConsumableBonus>().restoredHealth > 0)
                    {
                        FindObjectOfType<AudioManager>().PlaySound("Consume Heal Item");
                    }
                    else
                    {
                        FindObjectOfType<AudioManager>().PlaySound("Consume Non Heal Item");
                    }
                    displayInfo.gameObject.GetComponent<ConsumableBonus>().consumeItem();
                    inventory.itemList.Remove(displayInfo.gameObject);
                }
            }
        }
    }

    public void storeItemVault()
    {
        if(!Input.GetKeyDown(KeyCode.LeftShift) && displayInfo != null && inventory.vaultDisplay != null && inventory.vaultDisplay.activeSelf == true && HubProperties.vaultItems.Count < HubProperties.maxNumberVaultItems)
        {
            inventory.itemList.Remove(displayInfo.gameObject);
            if (displayInfo.goldValue <= 0)
            {
                goldenVault.vaultItems.Add(displayInfo.gameObject);
                HubProperties.vaultItems.Add(displayInfo.gameObject.name);
                FindObjectOfType<AudioManager>().PlaySound("Store Items Golden Vault");
            }
            inventory.UpdateUI();
            goldenVault.UpdateUI();
        }
    }

    public void transferArtifact()
    {
        if(!Input.GetKeyDown(KeyCode.LeftShift) && displayInfo != null && displayInfo.isArtifact == true && artifacts.artifactsUI.activeSelf == true && PlayerProperties.playerScript.enemiesDefeated == true && PlayerProperties.playerInventory.consumableConfirmationWindow.gameObject.activeSelf == false)
        {
            if (artifacts.activeArtifacts.Count < 3)
            {
                FindObjectOfType<AudioManager>().PlaySound("Equip Artifact");
                displayInfo.isEquipped = true;
                displayInfo.GetComponent<ArtifactEffect>()?.artifactEquipped();
                artifacts.activeArtifacts.Add(displayInfo.gameObject);
                inventory.itemList.Remove(displayInfo.gameObject);
                PlayerProperties.playerScript.CheckAndUpdateHealth();
            }
            else
            {
                FindObjectOfType<AudioManager>().PlaySound("Equip Artifact");
                displayInfo.isEquipped = true;
                displayInfo.isEquipped = true;
                displayInfo.GetComponent<ArtifactEffect>()?.artifactEquipped();
                artifacts.activeArtifacts.Add(displayInfo.gameObject);
                inventory.itemList.Remove(displayInfo.gameObject);
                GameObject firstArtifact = artifacts.activeArtifacts[0];
                artifacts.activeArtifacts.RemoveAt(0);
                inventory.itemList.Add(firstArtifact);
            }
        }
    }

    public void MoveToDisenchant()
    {
        if (PlayerProperties.articraftingDisenchantingMenu != null && PlayerProperties.articraftingDisenchantingMenu.IsMenuOpened())
        {
            if (!Input.GetKeyDown(KeyCode.LeftShift) && displayInfo != null && displayInfo.isArtifact == true && PlayerProperties.playerScript.enemiesDefeated == true && PlayerProperties.playerInventory.consumableConfirmationWindow.gameObject.activeSelf == false)
            {
                FindObjectOfType<AudioManager>().PlaySound("Equip Artifact");
                inventory.itemList.Remove(displayInfo.gameObject);
                PlayerProperties.articraftingDisenchantingMenu.SetTargetArtifact(displayInfo);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (displayInfo != null)
        {
            toolTip.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (displayInfo != null)
        {   
            PlayerProperties.toolTip.SetTextAndPosition(displayInfo.GetComponent<Text>().text, transform.position);
        }
    }

    public void updateUIs()
    {
        inventory.UpdateUI();
        artifacts.UpdateUI();
    }
}
