using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperUpgradeManager : WeaponFireTemplate
{
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    public GameObject highVelocityBullet1, highVelocityBullet2, highVelocityBullet3;
    public GameObject regularPlume;
    public GameObject extremeFocusShot1, extremeFocusShot2;
    GameObject empoweredWeaponFlare;

    int prevNumberUpgrades;
    float origCoolDownTime;
    bool highVelocityBullets = false, extremeFocus = false;
    int numberExtremeFocus = 2, numberShotsThreshold = 4;
    public Sprite extremeFocusIcon1, extremeFocusIcon2, regularSniperIcon;
    Sprite extremeFocusIcon;

    void applyUpgrades()
    {
        if (PlayerUpgrades.sniperUpgrades.Count == 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            highVelocityBullets = false;
            weaponScript.weaponPlume = regularPlume;
            extremeFocus = false;
        }
        else if (PlayerUpgrades.sniperUpgrades.Count > 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            if (PlayerUpgrades.sniperUpgrades[3] == "unlock_high_velocity_bullets")
            {
                if (PlayerUpgrades.sniperUpgrades.Count == 4)
                {
                    highVelocityBullets = true;
                    empoweredWeaponFlare = highVelocityBullet1;
                }
                else if (PlayerUpgrades.sniperUpgrades.Count == 5)
                {
                    highVelocityBullets = true;
                    empoweredWeaponFlare = highVelocityBullet2;
                }
                else
                {
                    highVelocityBullets = true;
                    empoweredWeaponFlare = highVelocityBullet3;
                }
            }
            else
            {
                if (PlayerUpgrades.sniperUpgrades.Count == 4)
                {
                    extremeFocus = true;
                    empoweredWeaponFlare = extremeFocusShot1;
                    numberShotsThreshold = 4;
                    extremeFocusIcon = extremeFocusIcon1;
                }
                else if (PlayerUpgrades.sniperUpgrades.Count == 5)
                {
                    extremeFocus = true;
                    empoweredWeaponFlare = extremeFocusShot1;
                    numberShotsThreshold = 3;
                    extremeFocusIcon = extremeFocusIcon1;
                }
                else
                {
                    extremeFocus = true;
                    empoweredWeaponFlare = extremeFocusShot2;
                    numberShotsThreshold = 3;
                    extremeFocusIcon = extremeFocusIcon2;
                }
            }
        }
        else
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
            highVelocityBullets = false;
            extremeFocus = false;
            weaponScript.weaponPlume = regularPlume;
        }
    }

    void Start()
    {
        PlayerUpgrades.sniperUpgrades.Add(" ");
        PlayerUpgrades.sniperUpgrades.Add(" ");
        PlayerUpgrades.sniperUpgrades.Add(" ");
        PlayerUpgrades.sniperUpgrades.Add("unlock_high_velocity_bullets");
        PlayerUpgrades.sniperUpgrades.Add(" ");
        PlayerUpgrades.sniperUpgrades.Add("target_weaknesses_unlocked");

        prevNumberUpgrades = PlayerUpgrades.sniperUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.sniperUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.sniperUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
        }

        if (extremeFocus)
        {
            if (weaponScript.numberShots == numberShotsThreshold - 1)
            {
                weaponScript.weaponPlume = empoweredWeaponFlare;
                weaponScript.weaponIcon.sprite = extremeFocusIcon;
            }

            if (weaponScript.numberShots >= numberShotsThreshold)
            {
                weaponScript.numberShots = 0;
                weaponScript.weaponPlume = regularPlume;
                weaponScript.weaponIcon.sprite = regularSniperIcon;
            }
        }

        if (highVelocityBullets)
        {
            if (weaponScript.weaponPlume != empoweredWeaponFlare)
            {
                weaponScript.weaponPlume = empoweredWeaponFlare;
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
