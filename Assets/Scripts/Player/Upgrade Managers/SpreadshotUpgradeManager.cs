using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadshotUpgradeManager : MonoBehaviour {
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    int prevNumberUpgrades;
    bool concentratedShot = false, sprayShot = false;
    public GameObject regularSpreadShot, sprayShot1, sprayShot2, sprayShot3, concentratedPlume1, concentratedPlume2;
    int coolDownReduction = 0;
    GameObject empoweredWeaponFlare;
    float origCoolDownTime;
    public Sprite concentratedShotIcon, sprayShotIcon;
    int numberPrevShots;

    void applyUpgrades()
    {
        if (PlayerUpgrades.spreadshotUpgrades.Count == 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            sprayShot = false;
            concentratedShot = false;
            coolDownReduction = 0;
        }
        else if (PlayerUpgrades.spreadshotUpgrades.Count > 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            if (PlayerUpgrades.spreadshotUpgrades[3] == "spreadshot_five_spray_unlocked")
            {
                if (PlayerUpgrades.spreadshotUpgrades.Count == 4)
                {
                    sprayShot = true;
                    empoweredWeaponFlare = sprayShot1;
                }
                else if (PlayerUpgrades.spreadshotUpgrades.Count == 5)
                {
                    sprayShot = true;
                    empoweredWeaponFlare = sprayShot2;
                }
                else
                {
                    sprayShot = true;
                    empoweredWeaponFlare = sprayShot3;
                }
            }
            else
            {
                if (PlayerUpgrades.spreadshotUpgrades.Count == 4)
                {
                    concentratedShot = true;
                    empoweredWeaponFlare = concentratedPlume1;
                    coolDownReduction = 0;
                    numberPrevShots = weaponScript.numberShots;
                }
                else if (PlayerUpgrades.spreadshotUpgrades.Count == 5)
                {
                    concentratedShot = true;
                    empoweredWeaponFlare = concentratedPlume1;
                    coolDownReduction = 1;
                    numberPrevShots = weaponScript.numberShots;
                }
                else
                {
                    concentratedShot = true;
                    empoweredWeaponFlare = concentratedPlume2;
                    coolDownReduction = 1;
                    numberPrevShots = weaponScript.numberShots;
                }
            }
        }
        else
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
            sprayShot = false;
            concentratedShot = false;
            coolDownReduction = 0;
        }
    }

    void Start()
    {
        prevNumberUpgrades = PlayerUpgrades.spreadshotUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.spreadshotUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.spreadshotUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
        }

        if (sprayShot)
        {
            if(weaponScript.musketSmoke != empoweredWeaponFlare)
            {
                weaponScript.musketSmoke = empoweredWeaponFlare;
                weaponScript.weaponIcon.sprite = sprayShotIcon;
            }
        }

        if (concentratedShot)
        {
            if(weaponScript.musketSmoke != empoweredWeaponFlare)
            {
                weaponScript.musketSmoke = empoweredWeaponFlare;
                weaponScript.weaponIcon.sprite = concentratedShotIcon;
            }

            if(numberPrevShots != weaponScript.numberShots)
            {
                numberPrevShots = weaponScript.numberShots;
                weaponScript.coolDownPeriod -= coolDownReduction;
            }
        }
    }
}
