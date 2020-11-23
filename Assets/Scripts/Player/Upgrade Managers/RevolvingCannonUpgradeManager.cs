using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevolvingCannonUpgradeManager : WeaponFireTemplate
{
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    int maxRounds = 7;
    int numberRoundsStockPiled = 0;
    private float origCoolDownTime;
    private int prevNumberUpgrades;
    bool cartridgeUpgrade = false;
    int bonusRounds = 0;
    bool canFire = true;
    bool inFocus = false;

    void applyUpgrades()
    {
        if (PlayerUpgrades.revolvingCannonUpgrades.Count >= 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            maxRounds = 7;
            cartridgeUpgrade = false;
            if(PlayerUpgrades.revolvingCannonUpgrades.Count > 3)
            {
                if (PlayerUpgrades.revolvingCannonUpgrades[3] == "bullet_cartridge_upgrade")
                {
                    cartridgeUpgrade = true;
                    weaponTemplate.coolDownTime = 10;
                    if (PlayerUpgrades.revolvingCannonUpgrades.Count == 4)
                    {
                        maxRounds = 9;
                    }
                    else if(PlayerUpgrades.revolvingCannonUpgrades.Count == 5)
                    {
                        maxRounds = 11;
                    }
                    else
                    {
                        maxRounds = 13;
                    }
                }
            }
        }
        else if (PlayerUpgrades.revolvingCannonUpgrades.Count <= 2)
        {
            maxRounds = 7;
            cartridgeUpgrade = false;
            weaponTemplate.coolDownTime = origCoolDownTime;
        }
    }

    public override void TookDamage(int damage, Enemy enemy)
    {
        bonusRounds += Mathf.FloorToInt(damage / 500);
    }

    void Start()
    {
        prevNumberUpgrades = PlayerUpgrades.revolvingCannonUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
    }

    public override void InitializeTextIcon(Text text)
    {
        text.enabled = true;
        text.text = numberRoundsStockPiled.ToString();
        weaponScript.noFireNormally = true;
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.revolvingCannonUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.revolvingCannonUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
        }

        if (cartridgeUpgrade == false)
        {
            if (numberRoundsStockPiled < maxRounds)
            {
                if (weaponScript.coolDownPeriod <= 0)
                {
                    weaponScript.coolDownPeriod = weaponScript.coolDownThreshold;
                    numberRoundsStockPiled++;
                    weaponScript.onCooldown = true;
                    weaponScript.weaponNumberText.text = numberRoundsStockPiled.ToString();
                }
            }
            else
            {
                weaponScript.coolDownPeriod = weaponScript.coolDownThreshold;
                weaponScript.onCooldown = false;
            }
        }
        else
        {
            if (numberRoundsStockPiled <= 0)
            {
                if (weaponScript.coolDownPeriod <= 0)
                {
                    weaponScript.coolDownPeriod = weaponScript.coolDownThreshold;
                    numberRoundsStockPiled = maxRounds;
                    if(PlayerUpgrades.revolvingCannonUpgrades.Count >= 6)
                    {
                        numberRoundsStockPiled += bonusRounds;
                    }
                    bonusRounds = 0;

                    weaponScript.weaponNumberText.text = numberRoundsStockPiled.ToString();
                }
            }
            else
            {
                weaponScript.coolDownPeriod = weaponScript.coolDownThreshold;
                weaponScript.onCooldown = false;
            }
        }

        if (weaponScript.mouseHovering == true && PlayerProperties.playerScript.playerDead == false && PlayerProperties.playerScript.windowAlreadyOpen == false)
        {

            if(Input.GetMouseButtonDown(0))
            {
                inFocus = true;
            }

            if (Input.GetMouseButton(0) && numberRoundsStockPiled > 0 && canFire && inFocus)
            {
                weaponScript.forceFire();
                numberRoundsStockPiled--;
                weaponScript.weaponNumberText.text = numberRoundsStockPiled.ToString();
                StartCoroutine(canFireRoutine());
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            inFocus = false;
            canFire = true;
        }
    }

    IEnumerator canFireRoutine()
    {
        canFire = false;
        yield return new WaitForSeconds(0.1f);
        canFire = true;
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
