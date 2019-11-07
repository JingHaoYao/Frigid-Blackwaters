using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadshotUpgradeManager : WeaponFireTemplate {
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
            if (weaponScript.weaponPlume != empoweredWeaponFlare)
            {
                weaponScript.weaponPlume = empoweredWeaponFlare;
                weaponScript.weaponIcon.sprite = sprayShotIcon;
            }
        }

        if (concentratedShot)
        {
            if (weaponScript.weaponPlume != empoweredWeaponFlare)
            {
                weaponScript.weaponPlume = empoweredWeaponFlare;
                weaponScript.weaponIcon.sprite = concentratedShotIcon;
            }

            if (numberPrevShots != weaponScript.numberShots)
            {
                numberPrevShots = weaponScript.numberShots;
                weaponScript.coolDownPeriod -= coolDownReduction;
            }
        }
    }

    public override GameObject fireWeapon(int whichSide, float angleOrientation, GameObject weaponPlume)
    {
        GameObject instant;
        if (whichSide == 1)
        {
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0.339f, 0.24f, 0), Quaternion.Euler(0, 0, 225));
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0, 0.42f, 0), Quaternion.Euler(0, 0, 270));
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(-0.339f, 0.24f, 0), Quaternion.Euler(0, 0, 315));
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(-0.55f, -.05f, 0), Quaternion.Euler(0, 0, 0));
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(-0.379f, -0.34f, 0), Quaternion.Euler(0, 0, 45));
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0, -0.55f, 0), Quaternion.Euler(0, 0, 90));
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0.379f, -0.34f, 0), Quaternion.Euler(0, 0, 135));
            }
            else
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0.55f, -.05f, 0), Quaternion.Euler(0, 0, 180));
            }
        }
        else if (whichSide == 2)
        {
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0.196f, -0.21f, 0), Quaternion.Euler(0, 0, 135));
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0.55f, -.05f, 0), Quaternion.Euler(0, 0, 180));
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0.619f, 0.24f, 0), Quaternion.Euler(0, 0, 200));
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0, 0.42f, 0), Quaternion.Euler(0, 0, 270));
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(-0.339f, 0.24f, 0), Quaternion.Euler(0, 0, 315));
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(-0.55f, -.05f, 0), Quaternion.Euler(0, 0, 0));
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(-0.379f, -0.34f, 0), Quaternion.Euler(0, 0, 45));
            }
            else
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0, -0.55f, 0), Quaternion.Euler(0, 0, 90));
            }
        }
        else
        {
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(-0.639f, 0.24f, 0), Quaternion.Euler(0, 0, 340));
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(-0.55f, -.05f, 0), Quaternion.Euler(0, 0, 0));
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(-0.189f, -0.15f, 0), Quaternion.Euler(0, 0, 45));
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0, -0.55f, 0), Quaternion.Euler(0, 0, 90));
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0.379f, -0.34f, 0), Quaternion.Euler(0, 0, 135));
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0.55f, -.05f, 0), Quaternion.Euler(0, 0, 180));
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0.339f, 0.24f, 0), Quaternion.Euler(0, 0, 225));
            }
            else
            {
                instant = Instantiate(weaponPlume, weaponScript.transform.position + new Vector3(0, 0.42f, 0), Quaternion.Euler(0, 0, 270));
            }
        }

        return instant;
    }
}
