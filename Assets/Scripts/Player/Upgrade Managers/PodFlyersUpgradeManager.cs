using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodFlyersUpgradeManager : WeaponFireTemplate
{
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    [SerializeField] GameObject regularPod1, regularPod2, spikyPod1, spikyPod2, spikyPod3, stuffedPod1, stuffedPod2, stuffedPod3;
    GameObject empoweredWeaponFlare;

    int prevNumberUpgrades;
    float origCoolDownTime;

    void applyUpgrades()
    {
        weaponTemplate.weaponFlare = regularPod1;
        if (PlayerUpgrades.podFlyersUpgrades.Count == 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
        }
        else if(PlayerUpgrades.podFlyersUpgrades.Count == 2)
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
            weaponTemplate.weaponFlare = regularPod2;
        }
        else if (PlayerUpgrades.podFlyersUpgrades.Count > 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            if (PlayerUpgrades.podFlyersUpgrades[3] == "unlock_spiky_pods")
            {
                if (PlayerUpgrades.podFlyersUpgrades.Count == 4)
                {
                    empoweredWeaponFlare = spikyPod1;
                }
                else if (PlayerUpgrades.podFlyersUpgrades.Count == 5)
                {
                    empoweredWeaponFlare = spikyPod2;
                }
                else
                {
                    empoweredWeaponFlare = spikyPod3;
                }
            }
            else
            {
                if (PlayerUpgrades.podFlyersUpgrades.Count == 4)
                {
                    empoweredWeaponFlare = stuffedPod1;
                }
                else if (PlayerUpgrades.podFlyersUpgrades.Count == 5)
                {
                    empoweredWeaponFlare = stuffedPod2;
                }
                else
                {
                    empoweredWeaponFlare = stuffedPod3;
                }
            }
            weaponTemplate.weaponFlare = empoweredWeaponFlare;
        }
        else
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
        }
    }

    void Start()
    {
        prevNumberUpgrades = PlayerUpgrades.podFlyersUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.podFlyersUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.podFlyersUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
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
