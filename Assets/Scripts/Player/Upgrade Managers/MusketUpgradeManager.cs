using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketUpgradeManager : WeaponFireTemplate {
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    int prevNumberUpgrades;
    bool precisionShot = false, scatterShot = false;
    public GameObject precisionBlast1, precisionBlast2, regularweaponPlume, scatterBlast1, scatterBlast2;
    GameObject empoweredWeaponFlare;
    int numberShotsThreshold = 4;
    float origCoolDownTime;
    public Sprite regularMusketIcon, scatterShotIcon, precisionShotIcon;

    void applyUpgrades()
    {
        if(PlayerUpgrades.musketUpgrades.Count == 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            precisionShot = false;
            scatterShot = false;
        }
        else if(PlayerUpgrades.musketUpgrades.Count > 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            if (PlayerUpgrades.musketUpgrades[3] == "precision_shot_unlocked")
            {
                if(PlayerUpgrades.musketUpgrades.Count == 4)
                {
                    precisionShot = true;
                    empoweredWeaponFlare = precisionBlast1;
                    numberShotsThreshold = 4;
                }
                else if(PlayerUpgrades.musketUpgrades.Count == 5)
                {
                    precisionShot = true;
                    empoweredWeaponFlare = precisionBlast1;
                    numberShotsThreshold = 3;
                }
                else
                {
                    precisionShot = true;
                    empoweredWeaponFlare = precisionBlast2;
                    numberShotsThreshold = 3;
                }
            }
            else
            {
                if (PlayerUpgrades.musketUpgrades.Count == 4)
                {
                    scatterShot = true;
                    empoweredWeaponFlare = scatterBlast1;
                    numberShotsThreshold = 4;
                }
                else if (PlayerUpgrades.musketUpgrades.Count == 5)
                {
                    scatterShot = true;
                    empoweredWeaponFlare = scatterBlast1;
                    numberShotsThreshold = 3;
                }
                else
                {
                    scatterShot = true;
                    empoweredWeaponFlare = scatterBlast2;
                    numberShotsThreshold = 3;
                }
            }
        }
        else
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
            precisionShot = false;
            scatterShot = false;
        }
    }

	void Start () {
        prevNumberUpgrades = PlayerUpgrades.musketUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
    }

	void Update () {
		if(prevNumberUpgrades != PlayerUpgrades.musketUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.musketUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
        }

        if (precisionShot || scatterShot)
        {
            if (weaponScript.numberShots == numberShotsThreshold - 1)
            {
                weaponScript.weaponPlume = empoweredWeaponFlare;
                if (scatterShot)
                {
                    weaponScript.weaponIcon.sprite = scatterShotIcon;
                }
                else
                {
                    weaponScript.weaponIcon.sprite = precisionShotIcon;
                }
            }

            if (weaponScript.numberShots >= numberShotsThreshold)
            {
                weaponScript.numberShots = 0;
                weaponScript.weaponPlume = regularweaponPlume;
                weaponScript.weaponIcon.sprite = regularMusketIcon;
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
