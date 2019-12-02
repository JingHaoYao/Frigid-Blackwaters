using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableConfirm : MonoBehaviour
{
    // What item the confirmation is about
    public GameObject objectInQuestion;

    public void confirmConsume()
    {
        if (objectInQuestion.GetComponent<ConsumableBonus>() != null)
        {
            Inventory inventory = FindObjectOfType<Inventory>();
            if (objectInQuestion.GetComponent<ConsumableBonus>().restoredHealth > 0)
            {
                FindObjectOfType<AudioManager>().PlaySound("Consume Heal Item");
            }
            else
            {
                FindObjectOfType<AudioManager>().PlaySound("Consume Non Heal Item");
            }
            objectInQuestion.GetComponent<ConsumableBonus>().consumeItem();
            inventory.itemList.Remove(objectInQuestion);
            inventory.UpdateUI();
            this.gameObject.SetActive(false);
            objectInQuestion = null;
        }
    }

    public void noConsume()
    {
        this.gameObject.SetActive(false);
    }
}
