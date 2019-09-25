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
        inventory = GameObject.Find("PlayerShip").GetComponent<Inventory>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        toolTip = inventory.toolTip;
        if(GameObject.Find("Golden Vault"))
        {
            goldenVault = GameObject.Find("Golden Vault").GetComponent<GoldenVault>();
        }
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
        if (displayInfo != null && Input.GetKey(KeyCode.LeftShift) && SceneManager.GetActiveScene().name != "Tutorial")
        {
            FindObjectOfType<AudioManager>().PlaySound("Destroy Item");
            inventory.itemList.Remove(displayInfo.gameObject);
            Destroy(displayInfo.gameObject);
            displayInfo = null;
        }
    }

    public void consumeItem()
    {
        if(!Input.GetKeyDown(KeyCode.LeftShift) && displayInfo != null && displayInfo.isConsumable == true)
        {
            if (goldenVaultDisplay != null)
            {
                if (goldenVaultDisplay.activeSelf != true)
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
            else
            {
                if(displayInfo.gameObject.GetComponent<ConsumableBonus>().restoredHealth > 0)
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

    public void storeItemVault()
    {
        if(!Input.GetKeyDown(KeyCode.LeftShift) && displayInfo != null && inventory.vaultDisplay != null && inventory.vaultDisplay.activeSelf == true && HubProperties.vaultItems.Count < 8)
        {
            inventory.itemList.Remove(displayInfo.gameObject);
            if (displayInfo.goldValue <= 0)
            {
                goldenVault.vaultItems.Add(displayInfo.gameObject);
                FindObjectOfType<AudioManager>().PlaySound("Store Items Golden Vault");
            }
            else
            {
                HubProperties.storeGold += displayInfo.goldValue;
                Destroy(displayInfo.gameObject);
                FindObjectOfType<AudioManager>().PlaySound("Store Gold Golden Vault");
            }
            inventory.UpdateUI();
            goldenVault.UpdateUI();
        }
    }

    public void transferArtifact()
    {
        if(!Input.GetKeyDown(KeyCode.LeftShift) && displayInfo != null && displayInfo.isArtifact == true && artifacts.artifactsUI.activeSelf == true && GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated == true)
        {
            if (artifacts.activeArtifacts.Count < 3)
            {
                FindObjectOfType<AudioManager>().PlaySound("Equip Artifact");
                displayInfo.isEquipped = true;
                artifacts.activeArtifacts.Add(displayInfo.gameObject);
                inventory.itemList.Remove(displayInfo.gameObject);
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
            toolTip.SetActive(true);
            toolTip.transform.position = this.transform.position;
            toolTip.GetComponentInChildren<Text>().text = displayInfo.GetComponent<Text>().text;
        }
    }

    public void updateUIs()
    {
        inventory.UpdateUI();
        artifacts.UpdateUI();
    }
}
