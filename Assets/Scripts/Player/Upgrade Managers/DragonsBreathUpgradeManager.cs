using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonsBreathUpgradeManager : WeaponFireTemplate {
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    int prevNumberUpgrades;
    bool blueFire = false, longerFlames = false;
    public GameObject longerFlames1, longerFlames2, longerFlames3, blueFlames1, blueFlames2, blueFlames3;
    GameObject empoweredWeaponFlare;
    float origCoolDownTime;
    public Sprite blueFireIcon, longerFlamesIcon;
    int numberPrevShots;

    void applyUpgrades()
    {
        if (PlayerUpgrades.dragonBreathUpgrades.Count == 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            longerFlames = false;
            blueFire = false;
        }
        else if (PlayerUpgrades.dragonBreathUpgrades.Count > 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            if (PlayerUpgrades.dragonBreathUpgrades[3] == "blue_flames_unlocked")
            {
                if (PlayerUpgrades.dragonBreathUpgrades.Count == 4)
                {
                    longerFlames = true;
                    empoweredWeaponFlare = blueFlames1;
                }
                else if (PlayerUpgrades.dragonBreathUpgrades.Count == 5)
                {
                    empoweredWeaponFlare = blueFlames2;
                    longerFlames = true;
                }
                else
                {
                    empoweredWeaponFlare = blueFlames3;
                    longerFlames = true;
                }
            }
            else
            {
                if (PlayerUpgrades.dragonBreathUpgrades.Count == 4)
                {
                    blueFire = true;
                    empoweredWeaponFlare = longerFlames1;
                }
                else if (PlayerUpgrades.dragonBreathUpgrades.Count == 5)
                {
                    empoweredWeaponFlare = longerFlames2;
                    blueFire = true;
                }
                else
                {
                    empoweredWeaponFlare = longerFlames3;
                    blueFire = true;
                }
            }
        }
        else
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
            longerFlames = false;
            blueFire = false;
        }
    }

    void Start()
    {
        prevNumberUpgrades = PlayerUpgrades.dragonBreathUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.dragonBreathUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.dragonBreathUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
        }

        if (blueFire || longerFlames)
        {
            if (weaponScript.weaponPlume != empoweredWeaponFlare)
            {
                weaponScript.weaponPlume = empoweredWeaponFlare;
                if (blueFire)
                {
                    weaponScript.weaponIcon.sprite = longerFlamesIcon;
                }
                else
                {
                    weaponScript.weaponIcon.sprite = blueFireIcon;
                }
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
