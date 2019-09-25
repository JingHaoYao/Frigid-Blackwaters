using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrainingHouseWeaponIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image image;
    public int whichWeapon = 0;
    public GameObject leftWeapon, rightWeapon, frontWeapon;
    public GameObject[] templates;
    int index = 0;
    public GameObject toolTip;

    Text pickText()
    {
        if (whichWeapon == 1)
        {
            return frontWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.GetComponent<Text>();
        }
        else if (whichWeapon == 2)
        {
            return leftWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.GetComponent<Text>();
        }
        else
        {
            return rightWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.GetComponent<Text>();
        }
    }

    void setPicture()
    {
        if(whichWeapon == 1)
        {
            image.sprite = frontWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.GetComponent<ShipWeaponTemplate>().coolDownIcon;
        }
        else if(whichWeapon == 2)
        {
            image.sprite = leftWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.GetComponent<ShipWeaponTemplate>().coolDownIcon;
        }
        else if(whichWeapon == 3)
        {
            image.sprite = rightWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.GetComponent<ShipWeaponTemplate>().coolDownIcon;
        }
    }

    public void nextTemplate()
    {
        index++;
        if (index >= PlayerUpgrades.unlockLevel)
        {
            index = 0;
        }

        if (whichWeapon == 1)
        {
            frontWeapon.GetComponent<ShipWeaponScript>().swapTemplate(templates[index].GetComponent<ShipWeaponTemplate>());
            PlayerUpgrades.whichFrontWeaponEquipped = index;
        }
        else if (whichWeapon == 2)
        {
            leftWeapon.GetComponent<ShipWeaponScript>().swapTemplate(templates[index].GetComponent<ShipWeaponTemplate>());
            PlayerUpgrades.whichLeftWeaponEquipped = index;
        }
        else if (whichWeapon == 3)
        {
            rightWeapon.GetComponent<ShipWeaponScript>().swapTemplate(templates[index].GetComponent<ShipWeaponTemplate>());
            PlayerUpgrades.whichRightWeaponEquipped = index;
        }
        setPicture();
        FindObjectOfType<AudioManager>().PlaySound("Change Weapon");
        SaveSystem.SaveGame();
    }

    public void prevTemplate()
    {
        index--;
        if (index < 0)
        {
            index = PlayerUpgrades.unlockLevel - 1;
        }

        if (whichWeapon == 1)
        {
            frontWeapon.GetComponent<ShipWeaponScript>().swapTemplate(templates[index].GetComponent<ShipWeaponTemplate>());
            PlayerUpgrades.whichFrontWeaponEquipped = index;
        }
        else if (whichWeapon == 2)
        {
            leftWeapon.GetComponent<ShipWeaponScript>().swapTemplate(templates[index].GetComponent<ShipWeaponTemplate>());
            PlayerUpgrades.whichLeftWeaponEquipped = index;
        }
        else if (whichWeapon == 3)
        {
            rightWeapon.GetComponent<ShipWeaponScript>().swapTemplate(templates[index].GetComponent<ShipWeaponTemplate>());
            PlayerUpgrades.whichRightWeaponEquipped = index;
        }
        setPicture();
        FindObjectOfType<AudioManager>().PlaySound("Change Weapon");
        SaveSystem.SaveGame();
    }

	void Start () {
        if (whichWeapon == 1)
        {
            index = PlayerUpgrades.whichFrontWeaponEquipped;
        }
        else if (whichWeapon == 2)
        {
            index = PlayerUpgrades.whichLeftWeaponEquipped;
        }
        else if (whichWeapon == 3)
        {
            index = PlayerUpgrades.whichRightWeaponEquipped;
        }

        image = GetComponent<Image>();
        setPicture();
	}

    public void OnPointerExit(PointerEventData eventData)
    {
        if (toolTip.activeSelf == true)
            toolTip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toolTip.activeSelf == false)
        {
            pickText();
            toolTip.SetActive(true);
            toolTip.transform.position = this.transform.position;
            toolTip.GetComponentInChildren<Text>().text = pickText().text;
        }
    }
}
