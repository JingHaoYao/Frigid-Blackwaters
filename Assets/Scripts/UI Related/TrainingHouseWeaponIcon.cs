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
    public ShipWeaponTemplate[] templates;
    int index = 0;
    public GameObject toolTip;
    public GameObject weaponMenu;
    public static int whichWeaponToEquip = 0;

    MenuSlideAnimation menuSlideAnimation = new MenuSlideAnimation();

    void SetAnimation()
    {
        menuSlideAnimation.SetOpenAnimation(new Vector3(0, -585, 0), new Vector3(0, 0, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(0, 0, 0), new Vector3(0, -585, 0), 0.25f);
    }

    void OnEnable()
    {
        weaponMenu.SetActive(false);
    }

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

    public void setTemplate(int whichTemplate)
    {
        if (menuSlideAnimation.IsAnimating == false)
        {
            ShipWeaponTemplate template = templates[whichTemplate];
            if (whichWeaponToEquip == whichWeapon && MiscData.dungeonLevelUnlocked >= template.whichLevelUnlock)
            {
                if (whichWeapon == 1)
                {
                    frontWeapon.GetComponent<ShipWeaponScript>().swapTemplate(template);
                    PlayerUpgrades.whichFrontWeaponEquipped = whichTemplate;
                }
                else if (whichWeapon == 2)
                {
                    leftWeapon.GetComponent<ShipWeaponScript>().swapTemplate(template);
                    PlayerUpgrades.whichLeftWeaponEquipped = whichTemplate;
                }
                else if (whichWeapon == 3)
                {
                    rightWeapon.GetComponent<ShipWeaponScript>().swapTemplate(template);
                    PlayerUpgrades.whichRightWeaponEquipped = whichTemplate;
                }
                setPicture();
                FindObjectOfType<AudioManager>().PlaySound("Change Weapon");
                SaveSystem.SaveGame();
                menuSlideAnimation.PlayEndingAnimation(weaponMenu, () => { weaponMenu.SetActive(false); });
            }
        }
    }

    public void turnOnMenu()
    {
        if (menuSlideAnimation.IsAnimating == false)
        {
            whichWeaponToEquip = whichWeapon;
            FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
            weaponMenu.SetActive(true);
            menuSlideAnimation.PlayOpeningAnimation(weaponMenu);
        }
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
        SetAnimation();
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
