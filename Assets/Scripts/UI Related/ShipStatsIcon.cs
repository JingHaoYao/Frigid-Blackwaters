using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipStatsIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ShipStats shipStats;
    GameObject toolTip;
    Text text;
    public bool frontWeapon = false, leftWeapon = false, rightWeapon = false;

    void pickText()
    {
        if (frontWeapon)
        {
            text = shipStats.frontWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.GetComponent<Text>();
        }
        else if (leftWeapon)
        {
            text = shipStats.leftWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.GetComponent<Text>();
        }
        else if (rightWeapon)
        {
            text = shipStats.rightWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.GetComponent<Text>();
        }
        else
        {
            text = transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        }
    }

	void Start () {
        toolTip = GameObject.Find("PlayerShip").GetComponent<Inventory>().toolTip;
        shipStats = GameObject.Find("PlayerShip").GetComponent<ShipStats>();
        pickText();
	}

	void Update () {
		
	}

    public void OnPointerExit(PointerEventData eventData)
    {
        if(toolTip.activeSelf == true)
            toolTip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toolTip.activeSelf == false)
        {
            pickText();
            toolTip.SetActive(true);
            toolTip.transform.position = this.transform.position;
            toolTip.GetComponentInChildren<Text>().text = text.text;
        }
    }
}
