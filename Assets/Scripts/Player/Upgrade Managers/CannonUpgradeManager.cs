using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonUpgradeManager : WeaponFireTemplate
{
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    int prevNumberUpgrades;
    public GameObject regularCannonPlume, momentumBlast1, momentumBlast2, redHotPlume;
    bool momentumBlastUnlocked = false, synchroFiring = false;
    GameObject empoweredWeaponFlare;
    float origCoolDownTime;
    int prevNumShots = 0;
    float cycleReduction = 1;
    GameObject leftWeapon, rightWeapon;
    public Sprite momentumBlastIcon, redHotIcon, regularCannonIcon;

    void applyUpgrades()
    {
        if (PlayerUpgrades.cannonUpgrades.Count == 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            momentumBlastUnlocked = false;
            synchroFiring = false;
            weaponScript.weaponIcon.sprite = regularCannonIcon;
        }
        else if (PlayerUpgrades.cannonUpgrades.Count > 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            if (PlayerUpgrades.cannonUpgrades[3] == "momentum_blast_unlocked")
            {
                if (PlayerUpgrades.cannonUpgrades.Count == 4)
                {
                    empoweredWeaponFlare = momentumBlast1;
                    momentumBlastUnlocked = true;
                }
                else if (PlayerUpgrades.cannonUpgrades.Count == 5)
                {
                    empoweredWeaponFlare = momentumBlast1;
                    momentumBlastUnlocked = true;
                    weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.7f * 100f) / 100f;
                }
                else
                {
                    empoweredWeaponFlare = momentumBlast2;
                    momentumBlastUnlocked = true;
                    weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.7f * 100f) / 100f;
                }
            }
            else
            {
                if (PlayerUpgrades.cannonUpgrades.Count == 4)
                {
                    synchroFiring = true;
                    cycleReduction = 1f;
                    empoweredWeaponFlare = regularCannonPlume;
                    prevNumShots = weaponScript.numberShots;
                }
                else if (PlayerUpgrades.cannonUpgrades.Count == 5)
                {
                    synchroFiring = true;
                    cycleReduction = 2f;
                    empoweredWeaponFlare = regularCannonPlume;
                    prevNumShots = weaponScript.numberShots;
                }
                else
                {
                    synchroFiring = true;
                    empoweredWeaponFlare = redHotPlume;
                    cycleReduction = 2f;
                    prevNumShots = weaponScript.numberShots;
                }
            }
        }
        else
        {
            momentumBlastUnlocked = false;
            synchroFiring = false;
            weaponTemplate.coolDownTime = origCoolDownTime;
        }
    }

    void Start()
    {
        prevNumberUpgrades = PlayerUpgrades.cannonUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        leftWeapon = GameObject.Find("PlayerShip").GetComponent<ShipStats>().leftWeapon;
        rightWeapon = GameObject.Find("PlayerShip").GetComponent<ShipStats>().rightWeapon;
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.cannonUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.cannonUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
        }

        if (momentumBlastUnlocked)
        {
            if (weaponScript.whichSide == 2)
            {
                weaponScript.weaponPlume = empoweredWeaponFlare;
                weaponScript.weaponIcon.sprite = momentumBlastIcon;
            }
            else if (weaponScript.whichSide == 3)
            {
                weaponScript.weaponPlume = empoweredWeaponFlare;
                weaponScript.weaponIcon.sprite = momentumBlastIcon;
            }
        }

        if (synchroFiring)
        {
            if (weaponScript.whichSide == 1)
            {
                if (weaponScript.mouseHovering && Input.GetMouseButtonDown(0))
                {
                    if (prevNumShots != weaponScript.numberShots)
                    {
                        weaponScript.numberShots = 0;
                        prevNumShots = 0;
                        if(leftWeapon.GetComponent<ShipWeaponScript>().onCooldown == false && leftWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.name ==
                            "Cannon Weapon Template")
                        {
                            leftWeapon.GetComponent<ShipWeaponScript>().forceFire();
                            leftWeapon.GetComponent<ShipWeaponScript>().coolDownPeriod -= cycleReduction;
                        }

                        if (rightWeapon.GetComponent<ShipWeaponScript>().onCooldown == false && rightWeapon.GetComponent<ShipWeaponScript>().shipWeaponTemplate.name ==
                            "Cannon Weapon Template")
                        {
                            rightWeapon.GetComponent<ShipWeaponScript>().forceFire();
                            rightWeapon.GetComponent<ShipWeaponScript>().coolDownPeriod -= cycleReduction;
                        }
                        weaponScript.coolDownPeriod -= cycleReduction;
                    }
                }
            }

            if(weaponScript.weaponPlume != empoweredWeaponFlare)
            {
                weaponScript.weaponPlume = empoweredWeaponFlare;
                weaponScript.weaponIcon.sprite = redHotIcon;
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
