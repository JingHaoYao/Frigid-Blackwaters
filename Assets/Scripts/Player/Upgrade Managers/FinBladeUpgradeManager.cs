using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinBladeUpgradeManager : WeaponFireTemplate
{
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    int prevNumberUpgrades;

    [SerializeField] GameObject finBlade;

    FinBlade finBladeInstant;
    Transform finBladeTransform;
    SpriteRenderer finBladeRenderer;
    

    void Start()
    {
        prevNumberUpgrades = PlayerUpgrades.finBladeUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        weaponScript.setTemplate();


        GameObject newBladeInstant = Instantiate(finBlade, transform.position, Quaternion.identity);
        finBladeInstant = newBladeInstant.GetComponent<FinBlade>();
        finBladeTransform = newBladeInstant.transform;
        finBladeRenderer = newBladeInstant.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.finBladeUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.finBladeUpgrades.Count;
            weaponScript.setTemplate();
            finBladeInstant.ResetAnimationsAndSpawnedEffects();
        }

        finBladeTransform.position = weaponScript.transform.position;
        setRotation((360 + PlayerProperties.playerScript.whatAngleTraveled) % 360);
    }

    void setRotation(float angleOrientation)
    {
        if (PlayerProperties.playerScript.isShipRooted() || PlayerProperties.playerScript.playerDead)
        {
            return;
        }

        switch (weaponScript.whichSide)
        {
            case 1:
                finBladeRenderer.sortingOrder = PlayerProperties.spriteRenderer.sortingOrder + 10;
                if (angleOrientation > 15 && angleOrientation <= 75)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 225);
                }
                else if (angleOrientation > 75 && angleOrientation <= 105)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 270);
                }
                else if (angleOrientation > 105 && angleOrientation <= 165)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 315);
                }
                else if (angleOrientation > 165 && angleOrientation <= 195)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if (angleOrientation > 195 && angleOrientation <= 255)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 45);
                }
                else if (angleOrientation > 255 && angleOrientation <= 285)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else if (angleOrientation > 285 && angleOrientation <= 345)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 135);
                }
                else
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 180);
                }
                break;
            case 2:
                finBladeRenderer.sortingOrder = PlayerProperties.spriteRenderer.sortingOrder + 10;
                if (angleOrientation > 15 && angleOrientation <= 75)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 135);
                }
                else if (angleOrientation > 75 && angleOrientation <= 105)
                {
                    finBladeRenderer.sortingOrder = PlayerProperties.spriteRenderer.sortingOrder - 2;
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 180);
                }
                else if (angleOrientation > 105 && angleOrientation <= 165)
                {
                    finBladeRenderer.sortingOrder = PlayerProperties.spriteRenderer.sortingOrder - 2;
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 200);
                }
                else if (angleOrientation > 165 && angleOrientation <= 195)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 270);
                }
                else if (angleOrientation > 195 && angleOrientation <= 255)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 315);
                }
                else if (angleOrientation > 255 && angleOrientation <= 285)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if (angleOrientation > 285 && angleOrientation <= 345)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 45);
                }
                else
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 90);
                }
                break;
            case 3:
                finBladeRenderer.sortingOrder = PlayerProperties.spriteRenderer.sortingOrder + 5;
                if (angleOrientation > 15 && angleOrientation <= 75)
                {
                    finBladeRenderer.sortingOrder = PlayerProperties.spriteRenderer.sortingOrder - 2;
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 340);
                }
                else if (angleOrientation > 75 && angleOrientation <= 105)
                {
                    finBladeRenderer.sortingOrder = PlayerProperties.spriteRenderer.sortingOrder - 2;
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if (angleOrientation > 105 && angleOrientation <= 165)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 45);
                }
                else if (angleOrientation > 165 && angleOrientation <= 195)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else if (angleOrientation > 195 && angleOrientation <= 255)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 135);
                }
                else if (angleOrientation > 255 && angleOrientation <= 285)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 180);
                }
                else if (angleOrientation > 285 && angleOrientation <= 345)
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 225);
                }
                else
                {
                    finBladeTransform.rotation = Quaternion.Euler(0, 0, 270);
                }
                break;
        }
    }

    public override GameObject fireWeapon(int whichSide, float angleOrientation, GameObject weaponPlume)
    {
        return null;
    }

    private void OnDestroy()
    {
        Destroy(finBladeInstant.gameObject);
    }
}
