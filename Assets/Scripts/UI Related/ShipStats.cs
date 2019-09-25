using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipStats : MonoBehaviour {
    PlayerScript playerScript;
    public GameObject healthIcon, speedIcon, attackIcon, defenseIcon, periodicHealingIcon;
    public GameObject leftWeaponIcon, rightWeaponIcon, frontWeaponIcon;
    public GameObject leftWeapon, rightWeapon, frontWeapon;
    public GameObject shipStatsDisplay;
    GameObject toolTip;

    public void UpdateUI()
    {
        healthIcon.GetComponentInChildren<Text>().text = playerScript.shipHealth + " / " + playerScript.shipHealthMAX;
        speedIcon.GetComponentInChildren<Text>().text = (playerScript.boatSpeed + playerScript.speedBonus + playerScript.conSpeedBonus + playerScript.upgradeSpeedBonus).ToString();
        attackIcon.GetComponentInChildren<Text>().text = (1 + playerScript.attackBonus + playerScript.conAttackBonus).ToString();
        defenseIcon.GetComponentInChildren<Text>().text = ((1 - playerScript.defenseBonus - playerScript.conDefenseBonus - playerScript.upgradeDefenseBonus) * 100).ToString() + "%";
        periodicHealingIcon.GetComponentInChildren<Text>().text = playerScript.periodicHealing.ToString();
        leftWeaponIcon.GetComponentInChildren<Image>().sprite = leftWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.GetComponent<ShipWeaponTemplate>().coolDownIcon;
        leftWeaponIcon.GetComponentInChildren<Text>().text = leftWeapon.GetComponent<ShipWeaponScript>().coolDownThreshold + " s";
        rightWeaponIcon.GetComponentInChildren<Image>().sprite = rightWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.GetComponent<ShipWeaponTemplate>().coolDownIcon;
        rightWeaponIcon.GetComponentInChildren<Text>().text = rightWeapon.GetComponent<ShipWeaponScript>().coolDownThreshold + " s";
        frontWeaponIcon.GetComponentInChildren<Image>().sprite = frontWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.GetComponent<ShipWeaponTemplate>().coolDownIcon;
        frontWeaponIcon.GetComponentInChildren<Text>().text = frontWeapon.GetComponent<ShipWeaponScript>().coolDownThreshold + " s";
    }

	void Start () {
        playerScript = GetComponent<PlayerScript>();
        toolTip = GameObject.Find("PlayerShip").GetComponent<Inventory>().toolTip;
        shipStatsDisplay.SetActive(false);
	}

	void LateUpdate () {
        if (shipStatsDisplay.activeSelf == false)
        {
            if (Input.GetKeyDown(KeyCode.U) && playerScript.windowAlreadyOpen == false)
            {
                playerScript.windowAlreadyOpen = true;
                UpdateUI();
                shipStatsDisplay.SetActive(true);
                Time.timeScale = 0;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.U))
            {
                playerScript.windowAlreadyOpen = false;
                shipStatsDisplay.SetActive(false);
                Time.timeScale = 1;
                if(toolTip.activeSelf == true)
                {
                    toolTip.SetActive(false);
                }
            }
        }
    }
}
