using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GambleMenuGoldSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Inventory inventory;
    int price = 0;
    public GameObject toolTip;
    public DungeonGamble gamble;
    PlayerScript playerScript;
    public Text text;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        playerScript = FindObjectOfType<PlayerScript>();
        text = GetComponentInChildren<Text>();
    }

    public void gambleTakeGold()
    {
        int gold = tallyGold();
        price = gamble.gamblePrice;
        int remainder = price;
        if
        (
           gold >= price
        )
        {
            int index = 0;
            while (remainder > 0)
            {
                while (inventory.itemList[index].GetComponent<DisplayItem>().goldValue == 0)
                {
                    index++;
                }

                if (remainder >= inventory.itemList[index].GetComponent<DisplayItem>().goldValue)
                {
                    remainder -= inventory.itemList[index].GetComponent<DisplayItem>().goldValue;
                    inventory.itemList.Remove(inventory.itemList[index]);
                }
                else
                {
                    inventory.itemList[index].GetComponent<DisplayItem>().goldValue -= remainder;
                    remainder = 0;
                }
            }
            gamble.PlayEndingAnimation();
            playerScript.windowAlreadyOpen = false;
            Time.timeScale = 1;
            PlayerProperties.playerScript.removeRootingObject();
            FindObjectOfType<AudioManager>().PlaySound("Pick Up Gold");
            gamble.gamble();
        }
    }

    int tallyGold()
    {
        int totalGold = 0;
        if (inventory.itemList.Count > 0)
        {
            for (int i = 0; i < inventory.itemList.Count; i++)
            {
                totalGold += inventory.itemList[i].GetComponent<DisplayItem>().goldValue;
            }
        }
        return totalGold;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.SetActive(true);
        toolTip.transform.position = this.transform.position;
        toolTip.GetComponentInChildren<Text>().text = "Offer " + gamble.gamblePrice.ToString() + " Gold.";
    }
}
