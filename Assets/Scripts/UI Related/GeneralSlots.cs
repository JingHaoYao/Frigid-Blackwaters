
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class GeneralSlots : MonoBehaviour
{
    [SerializeField] GeneralSlot[] generalSlots;
    ItemTemplates itemTemplates;
    GameObject presentItems;
    GameObject toolTip;

    public void spawnAllItems(string[] itemsToLoad)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(waitUntilEndOfFrame(itemsToLoad));
    }

    IEnumerator waitUntilEndOfFrame(string[] itemsToLoad)
    {
        yield return new WaitForEndOfFrame();
        itemTemplates = FindObjectOfType<ItemTemplates>();
        presentItems = GameObject.Find("PresentItems");
        toolTip = PlayerProperties.playerInventory.toolTip;
        PlayerProperties.playerScript.playerDead = true;
        PlayerProperties.playerScript.windowAlreadyOpen = true;
        for (int i = 0; i < generalSlots.Length; i++)
        {
            GameObject spawnedItem = itemTemplates.loadItem(itemsToLoad[i]);
            generalSlots[i].Initialize(spawnedItem.GetComponent<DisplayItem>(), OnPointerEnter, OnPointerExit, transferItem);
        }
    }

    void transferItem(DisplayItem displayInfo)
    {
        if (displayInfo != null)
        {
            FindObjectOfType<AudioManager>().PlaySound("Receive Item");
            if (PlayerProperties.playerInventory.itemList.Count < PlayerItems.maxInventorySize)
            {
                if (displayInfo.goldValue <= 0)
                {
                    PlayerProperties.playerInventory.itemList.Add(displayInfo.gameObject);
                }
            }
            else
            {
                HubProperties.vaultItems.Add(displayInfo.name);
            }
            PlayerProperties.playerInventory.UpdateUI();
        }
        PlayerProperties.playerScript.playerDead = false;
        this.gameObject.SetActive(false);
        PlayerProperties.playerScript.windowAlreadyOpen = false;
    }

    void OnPointerExit(DisplayItem displayInfo)
    {
        if (displayInfo != null)
        {
            toolTip.SetActive(false);
        }
    }

    void OnPointerEnter(DisplayItem displayInfo)
    {
        if (displayInfo != null)
        {
            toolTip.SetActive(true);
            toolTip.transform.position = this.transform.position;
            toolTip.GetComponentInChildren<Text>().text = displayInfo.GetComponent<Text>().text;
        }
    }
}
